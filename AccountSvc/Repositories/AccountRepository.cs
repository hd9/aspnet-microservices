using AccountSvc.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountSvc.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connStr;
        private readonly string insAcct = "INSERT INTO account (name, email, password, created_at, last_updated, subscribe_newsletter) values (@name, @email, @password, sysdate(), sysdate(), @subscribe_newsletter)";
        private readonly string insAddress = "INSERT INTO address (account_id, name, is_default, street, city, region, country, postal_code, created_at, last_updated) values (@account_id, @name, @is_default, @street, @city, @region, @country, @postal_code, sysdate(), sysdate());";
        private readonly string updAccount = "UPDATE account set name = @name, email = @email, last_updated = sysdate() WHERE id = @id";
        private readonly string updAddress = "UPDATE address set name = @name, street = @street, city = @city, region = @region, postal_code = @postal_code, country = @country, last_updated = sysdate() WHERE id = @id";
        private readonly string updDefaultAddress = "UPDATE address set is_default = true where id = @id and account_id = @account_id";
        private readonly string delAddress = "DELETE FROM address WHERE id = @id";
        private readonly string updPwd = "UPDATE account set password = @password WHERE id = @id";
        private readonly string queryAcctById = "SELECT * FROM account WHERE id = @id";
        private readonly string queryAcctByEmail = "SELECT * FROM account WHERE email = @email";
        private readonly string queryAddressById = "SELECT * FROM address WHERE id = @id";
        private readonly string queryGetAddressesByAccountId = "SELECT * FROM address WHERE account_id = @account_id";

        public AccountRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task CreateAccount(CreateAccount account)
        {
            // todo :: salt + hash pwd
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.OpenAsync();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {

                    await conn.ExecuteAsync(insAcct, new
                    {
                        name = account.Name,
                        email = account.Email,
                        password = account.Password,
                        subscribe_newsletter = account.SubscribedToNewsletter
                    });

                    var accountId = (await conn.QueryAsync<int>("select LAST_INSERT_ID();")).Single();

                    await conn.ExecuteAsync(insAddress, new
                    {
                        account_id = accountId,
                        name = account.Name,
                        is_default = true,
                        street = account.Street,
                        city = account.City,
                        region = account.Region,
                        postal_code = account.PostalCode,
                        country = account.Country
                    });

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task UpdateAccount(UpdateAccount updAccount)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                // todo :: log account history
                await conn.ExecuteAsync(this.updAccount, new
                {
                    id = updAccount.Id,
                    name = updAccount.Name,
                    email = updAccount.Email
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

        public async Task UpdatePassword(UpdatePassword updPassword)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                // todo :: log account history
                await conn.QuerySingleOrDefaultAsync<Account>(updPwd, new
                {
                    password = updPassword.NewPassword,
                    id = updPassword.AccountId
                });
            }
        }

        public async Task AddAddress(Address addr)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                // todo :: log account history
                await conn.ExecuteAsync(insAddress, new
                {
                    account_id = addr.AccountId,
                    name = addr.Name,
                    is_default = addr.IsDefault,
                    street = addr.Street,
                    city = addr.City,
                    region = addr.Region,
                    postal_code = addr.PostalCode,
                    country = addr.Country
                });
            }
        }

        public async Task UpdateAddress(Address addr)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                // todo :: log account history
                await conn.ExecuteAsync(updAddress, new
                {
                    id = addr.Id,
                    name = addr.Name,
                    is_default = addr.IsDefault,
                    street = addr.Street,
                    city = addr.City,
                    region = addr.Region,
                    postal_code = addr.PostalCode,
                    country = addr.Country
                });
            }
        }

        public async Task RemoveAddress(string addressId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                // todo :: log account history
                await conn.ExecuteAsync(delAddress, new
                {
                    id = addressId
                });
            }
        }

        public async Task<Address> GetAddressById(string id)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Address>(queryAddressById, new { id });
            }
        }

        public async Task<IList<Address>> GetAddressesByAccountId(string acctId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return (await conn.QueryAsync<Address>(
                    queryGetAddressesByAccountId,
                    new { account_id = acctId }
                )).ToList();
            }
        }

        public async Task SetDefaultAddress(string acctId, int addressId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(updDefaultAddress, new
                {
                    id = addressId,
                    account_id = acctId,
                });
            }
        }
    }
}
