using AccountSvc.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountSvc.Services
{
    public class AccountSvc : IAccountSvc
    {
        private readonly string _connStr;
        private readonly string insert = "INSERT INTO account (name, email, password, created_at, last_updated, address, city, region, country, subscribe_newsletter) values (@name, @email, @password, sysdate(), sysdate(), @address, @city, @region, @country, @subscribe_newsletter)";
        private readonly string update = "UPDATE account (name, email, last_updated, address, city, region, country, subscribe_newsletter) values (@name, @email, sysdate(), @address, @city, @region, @country, @subscribe_newsletter) WHERE id = @id";
        private readonly string queryAcctById = "SELECT * FROM account WHERE id = @id";
        private readonly string queryAcctByEmail = "SELECT * FROM account WHERE email = @email";

        public AccountSvc(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task CreateAccount(Account account)
        {
            // todo :: salt + hash pwd
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(insert, new { 
                    name = account.Name,
                    email = account.Email,
                    password = account.Password,
                    address = account.Address,
                    city = account.City,
                    region = account.Region,
                    country = account.Country,
                    subscribe_newsletter = account.SubscribedToNewsletter
                });
            }
        }

        public async Task UpdateAccount(Account account)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(update, new
                {
                    id = account.Id,
                    name = account.Name,
                    email = account.Email,
                    address = account.Address,
                    city = account.City,
                    region = account.Region,
                    country = account.Country,
                    subscribe_newsletter = account.SubscribedToNewsletter
                });
            }
        }

        public async Task<Account> GetAccountById(string id)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Account>(queryAcctById, new { id });
            }
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Account>(queryAcctByEmail, new { email });
            }
        }
    }
}
