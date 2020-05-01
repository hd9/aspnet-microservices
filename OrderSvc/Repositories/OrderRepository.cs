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
        private readonly string insertLineItem = "insert into lineitem (order_id, name, price, qty) values (@orderId, @name, @price, @qty)";
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
            int orderId;

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

                    orderId = (await conn.QueryAsync<int>("select LAST_INSERT_ID();")).Single();

                    order.LineItems.ForEach(async (li) =>
                    {
                        await conn.ExecuteAsync(insertLineItem, new
                        {
                            orderId,
                            @name = li.Name,
                            @price = li.Price,
                            @qty = li.Qty
                        });
                    });

                    await transaction.CommitAsync();
                }
            }

            return orderId;
        }
    }
}
