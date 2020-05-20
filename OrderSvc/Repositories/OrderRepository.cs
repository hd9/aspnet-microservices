using Dapper;
using MySql.Data.MySqlClient;
using OrderSvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Infrastructure.Extensions;

namespace OrderSvc.Repositories
{
    public class OrderRepository : IOrderRepository
    {

        readonly string _connStr;
        readonly string insertOrder = "insert into orders (account_id, created_at, last_modified, currency, price, tax, shipping, total_price, status) values (@accountId, sysdate(), sysdate(), @currency, @price, @tax, @shipping, @totalPrice, @status)";
        readonly string insPmtInfo = "INSERT INTO payment_info (order_id, status, name, number, cvv, exp_date, method, created_at, last_updated) VALUES (@order_id, @status, @name, @number, @cvv, @exp_date, @method, sysdate(), sysdate())";
        readonly string insShippingInfo = "INSERT INTO shipping_info (order_id, payment_info_id, status, name, street, city, region, postal_code, country, created_at, last_updated) values (@order_id, @payment_info_id, @status, @name, @street, @city, @region, @postal_code, @country, sysdate(), sysdate());";
        readonly string insLineItem = "insert into lineitem (order_id, name, slug, price, qty) values (@order_id, @name, @slug, @price, @qty)";
        readonly string queryByAcctId = "select * from orders o inner join lineitem li on li.order_id = o.id where o.account_id=@accountId";

        // order history
        private readonly string insOrderHistory = "insert into order_history (order_id, event_type_id, requested_by_id, ref_id, ref_type_id, ip, info, created_at) values (@order_id, @event_type_id, @requested_by_id, @ref_id, @ref_type_id, @ip, @info, sysdate());";

        public OrderRepository(string connStr)
        {
            _connStr = connStr;
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            var orderDictionary = new Dictionary<int, Order>();
            using (var conn = new MySqlConnection(_connStr))
            {
                var orders = await conn.QueryAsync<Order, LineItem, Order>(queryByAcctId, (order, lineItem) =>
                {
                    Order orderEntry;

                    if (!orderDictionary.TryGetValue(order.Id, out orderEntry))
                    {
                        orderEntry = order;
                        orderEntry.LineItems = new List<LineItem>();
                        orderDictionary.Add(orderEntry.Id, orderEntry);
                    }

                    orderEntry.LineItems.Add(lineItem);
                    return orderEntry;

                }, new { accountId });
            }

            return orderDictionary.Select(x => x.Value);
        }

        public async Task<int> Insert(Order order)
        {
            // todo :: order number
            // var orderNumber = $"O-{DateTime.UtcNow.Year}-{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper()}";
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.OpenAsync();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    await conn.ExecuteAsync(insertOrder, new
                    {
                        @accountId = order.AccountId,
                        @currency = order.Currency,
                        @price = order.Price,
                        @tax = order.Tax,
                        @shipping = order.Shipping,
                        @totalPrice = order.TotalPrice,
                        @status = (int)OrderStatus.Submitted
                    });

                    order.Id = await GetLastInsertId<int>(conn);

                    // insert order lines
                    order.LineItems.ForEach(async (li) =>
                    {
                        await conn.ExecuteAsync(insLineItem, new
                        {
                            @order_id = order.Id,
                            @name = li.Name,
                            @slug = li.Slug,
                            @price = li.Price,
                            @qty = li.Qty
                        });
                    });

                    await InsertLog(
                        conn, 
                        order.Id, 
                        EventType.OrderCreated,
                        order.AccountId.ToString(),
                        data: order.ToString()
                    );


                    // insert payment info
                    await conn.ExecuteAsync(insPmtInfo, new
                    {
                        @order_id = order.Id,
                        @status = (int)order.PaymentInfo.Status,
                        @name = order.PaymentInfo.Name,
                        @number = order.PaymentInfo.Number,
                        @cvv = order.PaymentInfo.CVV,
                        @exp_date = order.PaymentInfo.ExpDate,
                        @method = (int)order.PaymentInfo.Method
                    });

                    order.PaymentInfo.Id = await GetLastInsertId<int>(conn);

                    await InsertLog(
                        conn,
                        order.Id,
                        EventType.PaymentSubmitted,
                        order.AccountId.ToString(),
                        refId: order.PaymentInfo.Id,
                        refType: RefType.PaymentInfo,
                        data: order.PaymentInfo.ToString()
                    );

                    // insert shipping info
                    await conn.ExecuteAsync(insShippingInfo, new
                    {
                        @order_id = order.Id,
                        @payment_info_id = order.PaymentInfo.Id,
                        @status = (int)order.ShippingInfo.Status,
                        @name = order.ShippingInfo.Name,
                        @street = order.ShippingInfo.Street,
                        @city = order.ShippingInfo.City,
                        @region = order.ShippingInfo.Region,
                        @postal_code = order.ShippingInfo.PostalCode,
                        @country = order.ShippingInfo.Country
                    });

                    order.ShippingInfo.Id = await GetLastInsertId<int>(conn);
                    await InsertLog(
                        conn,
                        order.Id,
                        EventType.ShippingInfoSubmitted,
                        order.AccountId.ToString(),
                        refId: order.ShippingInfo.Id,
                        refType: RefType.ShippingInfo,
                        data: order.ShippingInfo.ToString()
                    );

                    await transaction.CommitAsync();
                }
            }

            return order.Id;
        }

        private async Task<T> GetLastInsertId<T>(MySqlConnection conn)
        {
            return (await conn.QueryAsync<T>("select LAST_INSERT_ID();")).Single();
        }

        private async Task InsertLog(
            MySqlConnection conn,
            int orderId,
            EventType et,
            string requestedById,
            int? refId = null,
            RefType? refType = null,
            string ip = null,
            string data = null)
        {
            await conn.ExecuteAsync(insOrderHistory, new
            {
                order_id = orderId,
                event_type_id = (int)et,
                requested_by_id = requestedById,
                ref_id = refId,
                ref_type_id = (int?)refType,
                ip,
                info = FormatMsg(orderId, et, requestedById, data)
            });
        }

        private string FormatMsg(int orderId, EventType et, string accountId, string data)
        {
            switch (et)
            {
                case EventType.OrderCreated:
                    return $"Order created with {data}";
                case EventType.PaymentSubmitted:
                    return $"Payment submitted for Order {orderId}: {data}";
                case EventType.ShippingInfoSubmitted:
                    return $"Shipping information submitted for Order {orderId}: {data}";
                default:
                    return $"Order: {orderId} requested a {et}.{(data.HasValue() ? "with data: {data}" : "")}";
            }
        }

    }
}
