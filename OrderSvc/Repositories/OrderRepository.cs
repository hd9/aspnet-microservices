using Dapper;
using MySql.Data.MySqlClient;
using OrderSvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSvc.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connStr;
        private readonly string insertOrder = "insert into orders (account_id, created_at, last_modified, currency, price, tax, shipping, total_price, status) values (@accountId, sysdate(), sysdate(), @currency, @price, @tax, @shipping, @totalPrice, @status)";
        private readonly string insPmtInfo = "INSERT INTO payment_info (order_id, status, name, number, cvv, exp_date, method, created_at, last_updated) VALUES (@order_id, @status, @name, @number, @cvv, @exp_date, @method, sysdate(), sysdate())";
        private readonly string insShippingInfo = "INSERT INTO shipping_info (order_id, payment_info_id, status, name, street, city, region, postal_code, country, created_at, last_updated) values (@order_id, @payment_info_id, @status, @name, @street, @city, @region, @postal_code, @country, sysdate(), sysdate());";
        private readonly string insLineItem = "insert into lineitem (order_id, name, price, qty) values (@order_id, @name, @price, @qty)";
        private readonly string queryByAcctId = "select * from orders o inner join lineitem li on li.order_id = o.id where o.account_id=@accountId";

        public OrderRepository(string connStr)
        {
            _connStr = connStr;
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            var orderDictionary = new Dictionary<int, Order>();
            using (var conn = new MySqlConnection(_connStr))
            {
                //return await conn.QueryAsync<Order>(queryByAcctId, new { accountId });
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

                    order.Id = (await conn.QueryAsync<int>("select LAST_INSERT_ID();")).Single();

                    // insert order lines
                    order.LineItems.ForEach(async (li) =>
                    {
                        await conn.ExecuteAsync(insLineItem, new
                        {
                            @order_id = order.Id,
                            @name = li.Name,
                            @price = li.Price,
                            @qty = li.Qty
                        });
                    });

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

                    order.PaymentInfo.Id = (await conn.QueryAsync<int>("select LAST_INSERT_ID();")).Single();

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

                    await transaction.CommitAsync();
                }
            }

            return order.Id;
        }
    }
}
