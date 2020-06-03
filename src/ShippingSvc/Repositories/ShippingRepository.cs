using ShippingSvc.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Core.Infrastructure.Extensions;

namespace ShippingSvc.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        readonly string _connStr;
        readonly string insShipping = "INSERT INTO shipping (number, account_id, order_id, name, amount, currency, street, city, region, postal_code, country, status, provider, created_at) values (@number, @account_id, @order_id, @name, @amount, @currency, @street, @city, @region, @postal_code, @country, @status, @provider, sysdate());";
        readonly string queryById = "SELECT * FROM shipping WHERE id = @id";
        readonly string queryByAcctId = "SELECT * FROM shipping WHERE account_id = @account_id";

        public ShippingRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task Insert(Shipping shipping)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(insShipping, new
                {
                    @number = shipping.Number,
                    @account_id = shipping.AccountId,
                    @order_id = shipping.OrderId,
                    @name = shipping.Name,
                    @amount = shipping.Amount,
                    @currency = shipping.Currency,
                    @street = shipping.Street,
                    @city = shipping.City,
                    @region = shipping.Region,
                    @postal_code = shipping.PostalCode,
                    @country = shipping.Country,
                    @status = (int)shipping.Status,
                    @provider = (int)shipping.Provider
                });

                shipping.Id = (await conn.QueryAsync<int>("select LAST_INSERT_ID();")).Single();
            }
        }

        public async Task<Shipping> GetById(string id)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Shipping>(queryById, new { id });
            }
        }

        public async Task<Shipping> GetByAccountId(string accountId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Shipping>(queryByAcctId, new { account_id = accountId });
            }
        }
        
    }
}
