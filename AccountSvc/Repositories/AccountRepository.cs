using AccountSvc.Models;
using Core.Infrastructure.Extentions;
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

        // act
        private readonly string insAcct = "INSERT INTO account (name, email, password, created_at, last_updated, subscribe_newsletter) values (@name, @email, @password, sysdate(), sysdate(), @subscribe_newsletter)";
        private readonly string updAccount = "UPDATE account set name = @name, email = @email, last_updated = sysdate() WHERE id = @id";
        private readonly string updPwd = "UPDATE account set password = @password WHERE id = @id";
        private readonly string selAcctById = "SELECT * FROM account WHERE id = @id";
        private readonly string selAcctByEmail = "SELECT * FROM account WHERE email = @email";
        
        // address
        private readonly string insAddress = "INSERT INTO address (account_id, name, is_default, street, city, region, country, postal_code, created_at, last_updated) values (@account_id, @name, @is_default, @street, @city, @region, @country, @postal_code, sysdate(), sysdate());";
        private readonly string updAddress = "UPDATE address set name = @name, street = @street, city = @city, region = @region, postal_code = @postal_code, country = @country, last_updated = sysdate() WHERE id = @id";
        private readonly string updDefaultAddress = "UPDATE address set is_default = false where account_id = @account_id; UPDATE address set is_default = true where id = @id and account_id = @account_id";
        private readonly string delAddress = "DELETE FROM address WHERE id = @id";
        private readonly string selAddressById = "SELECT * FROM address WHERE id = @id";
        private readonly string selGetAddressesByAccountId = "SELECT * FROM address WHERE account_id = @account_id";

        // pmtinfo
        private readonly string insPmtInfo = "INSERT INTO payment_info (account_id, name, is_default, number, cvv, exp_date, method, created_at, last_updated) VALUES (@account_id, @name, @is_default, @number, @cvv, @exp_date, @method, sysdate(), sysdate())";
        private readonly string updPmtInfo = "UPDATE payment_info set name = @name, is_default = @is_default, number = @number, cvv = @cvv, exp_date = @exp_date, method = @method, last_updated = sysdate() where id = @id";
        private readonly string delPmtInfo = "DELETE FROM payment_info where id = @id";
        private readonly string selPmtInfoById = "SELECT * FROM payment_info where id = @id";
        private readonly string selPaymentInfosByAccountId = "SELECT * FROM payment_info where account_id = @account_id";
        private readonly string updDefaultPaymentInfo = "UPDATE payment_info SET is_default = false where account_id = @account_id; UPDATE payment_info SET is_default = true where id = @id AND account_id = @account_id";

        // log
        private readonly string insLog = "insert into log (event_type_id, requested_by_id, ref_id, ip, info, created_at) values (@event_type_id, @requested_by_id, @ref_id, @ip, @info, sysdate());";
        private readonly string selLog = "select * from log where requested_by_id=@account_id order by created_at DESC limit @limit";

        public AccountRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task CreateAccount(CreateAccount cmd)
        {
            // todo :: salt + hash pwd
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.OpenAsync();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {

                    await conn.ExecuteAsync(insAcct, new
                    {
                        name = cmd.Name,
                        email = cmd.Email,
                        password = cmd.Password,
                        subscribe_newsletter = cmd.SubscribedToNewsletter
                    });

                    var accountId = await GetLastInsertId<string>(conn);
                    await InsertLog(conn, EventType.AccountCreated, accountId, data: $"name '{cmd.Name}' and email '{cmd.Email}'");
                    await InsertLog(conn, EventType.PasswordCreated, accountId);

                    await conn.ExecuteAsync(insAddress, new
                    {
                        account_id = accountId,
                        name = cmd.Name,
                        is_default = true,
                        street = cmd.Street,
                        city = cmd.City,
                        region = cmd.Region,
                        postal_code = cmd.PostalCode,
                        country = cmd.Country
                    });

                    var addrId = await GetLastInsertId<string>(conn);

                    await InsertLog(
                        conn, 
                        EventType.AddressCreated, 
                        accountId, 
                        addrId, 
                        data: $"{cmd.Street}, {cmd.City} - {cmd.Region}, {cmd.Country}");

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task UpdateAccount(UpdateAccount cmd)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(updAccount, new
                {
                    id = cmd.Id,
                    name = cmd.Name,
                    email = cmd.Email
                });

               await InsertLog(
                   conn, 
                   EventType.AccountUpdated, 
                   cmd.Id, 
                   cmd.Id, 
                   data: $"name '{cmd.Name}' and email '{cmd.Email}'");
            }
        }

        public async Task<Account> GetAccountById(string id)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Account>(selAcctById, new { id });
            }
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Account>(selAcctByEmail, new { email });
            }
        }

        public async Task UpdatePassword(UpdatePassword cmd)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.QuerySingleOrDefaultAsync<Account>(updPwd, new
                {
                    password = cmd.NewPassword,
                    id = cmd.AccountId
                });

                await InsertLog(
                   conn,
                   EventType.PasswordUpdated,
                   cmd.AccountId,
                   cmd.AccountId);
            }
        }

        public async Task AddAddress(Address cmd)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(insAddress, new
                {
                    account_id = cmd.AccountId,
                    name = cmd.Name,
                    is_default = cmd.IsDefault,
                    street = cmd.Street,
                    city = cmd.City,
                    region = cmd.Region,
                    postal_code = cmd.PostalCode,
                    country = cmd.Country
                });

                var addrId = await GetLastInsertId<string>(conn);

                await InsertLog(
                   conn,
                   EventType.AddressCreated,
                   cmd.AccountId.ToString(),
                   addrId,
                   data: $"{cmd.Street}, {cmd.City} - {cmd.Region}, {cmd.Country}");
            }
        }

        public async Task UpdateAddress(Address addr)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
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

                await InsertLog(
                   conn,
                   EventType.AddressUpdated,
                   addr.AccountId.ToString(),
                   addr.Id.ToString(),
                   data: $"{addr.Street}, {addr.City} - {addr.Region}, {addr.Country}");
            }
        }

        public async Task RemoveAddress(string addressId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                // todo :: log account history
                var addr = await conn.QuerySingleOrDefaultAsync<Address>(selAddressById, new { id = addressId });

                await InsertLog(
                   conn,
                   EventType.AddressRemoved,
                   addr.AccountId.ToString(),
                   addr.Id.ToString(),
                   data: addr != null ? $"{addr.Street}, {addr.City} - {addr.Region}, {addr.Country}" : null
                );

                await conn.ExecuteAsync(delAddress, new { id = addressId });
            }
        }

        public async Task<Address> GetAddressById(string id)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Address>(selAddressById, new { id });
            }
        }

        public async Task<IList<Address>> GetAddressesByAccountId(string acctId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return (await conn.QueryAsync<Address>(
                    selGetAddressesByAccountId,
                    new { account_id = acctId }
                )).ToList();
            }
        }

        public async Task SetDefaultAddress(string acctId, int addressId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                var addr = await conn.QuerySingleOrDefaultAsync<Address>(selAddressById, new { id = addressId });

                await conn.ExecuteAsync(updDefaultAddress, new
                {
                    id = addressId,
                    account_id = acctId,
                });

                await InsertLog(
                   conn,
                   EventType.AddressSetDefault,
                   acctId,
                   addressId.ToString(),
                   data: addr != null ? $"{addr.Street}, {addr.City} - {addr.Region}, {addr.Country}" : null
                );
            }
        }

        public async Task AddPaymentInfo(PaymentInfo pmtInfo)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(insPmtInfo, new
                {
                    account_id = pmtInfo.AccountId,
                    name = pmtInfo.Name,
                    is_default = pmtInfo.IsDefault,
                    number = pmtInfo.Number,
                    cvv = pmtInfo.CVV,
                    exp_date = pmtInfo.ExpDate,
                    method = (int)pmtInfo.Method
                });

                var pmtInfoId = await GetLastInsertId<string>(conn);

                await InsertLog(
                   conn,
                   EventType.PaymentInfoCreated,
                   pmtInfo.AccountId.ToString(),
                   pmtInfoId,
                   data: $"Name: '{pmtInfo.Name}', Method: '{pmtInfo.Method}', Number: '{MaskCC(pmtInfo.Number)}'"
                );
            }
        }


        public async Task<PaymentInfo> GetPaymentInfoById(string pmtId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<PaymentInfo>(selPmtInfoById, new { id = pmtId });
            }
        }

        public async Task UpdatePaymentInfo(PaymentInfo pmtInfo)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(updPmtInfo, new
                {
                    id = pmtInfo.Id,
                    account_id = pmtInfo.AccountId,
                    name = pmtInfo.Name,
                    is_default = pmtInfo.IsDefault,
                    number = pmtInfo.Number,
                    cvv = pmtInfo.CVV,
                    exp_date = pmtInfo.ExpDate,
                    method = (int)pmtInfo.Method
                });

                await InsertLog(
                   conn,
                   EventType.PaymentInfoUpdated,
                   pmtInfo.AccountId.ToString(),
                   pmtInfo.Id.ToString(),
                   data: $"Name: '{pmtInfo.Name}', Method: '{pmtInfo.Method}', Number: '{MaskCC(pmtInfo.Number)}'"
                );
            }
        }

        public async Task RemovePaymentInfo(string pmtId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                var pmtInfo = await conn.QuerySingleOrDefaultAsync<PaymentInfo>(selPmtInfoById, new { id = pmtId });

                await conn.ExecuteAsync(delPmtInfo, new { id = pmtId });

                await InsertLog(
                   conn,
                   EventType.PaymentInfoRemoved,
                   pmtInfo.AccountId.ToString(),
                   pmtInfo.Id.ToString(),
                   data: $"Name: '{pmtInfo.Name}', Method: '{pmtInfo.Method}', Number: '{MaskCC(pmtInfo.Number)}'"
                );
            }
        }

        public async Task<IList<PaymentInfo>> GetPaymentInfosByAccountId(string acctId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return (await conn.QueryAsync<PaymentInfo>(
                    selPaymentInfosByAccountId,
                    new { account_id = acctId }
                )).ToList();
            }
        }

        public async Task SetDefaultPaymentInfo(string accountId, int pmtId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(updDefaultPaymentInfo, new
                {
                    id = pmtId,
                    account_id = accountId
                });

                var pmtInfo = await conn.QuerySingleOrDefaultAsync<PaymentInfo>(selPmtInfoById, new { id = pmtId });
                
                await conn.ExecuteAsync(updDefaultPaymentInfo, new
                {
                    id = pmtId,
                    account_id = accountId
                });

                await InsertLog(
                   conn,
                   EventType.PaymentInfoSetDefault,
                   pmtInfo.AccountId.ToString(),
                   pmtInfo.Id.ToString(),
                   data: $"Name: '{pmtInfo.Name}', Method: '{pmtInfo.Method}', Number: '{MaskCC(pmtInfo.Number)}'"
                );
            }
        }

        public async Task<IList<AccountHistory>> GetAccountHistory(string acctId, int limit = 10)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return (await conn.QueryAsync<AccountHistory>(
                    selLog,
                    new { account_id = acctId, limit }
                )).ToList();
            }
        }

        private async Task<T> GetLastInsertId<T>(MySqlConnection conn)
        {
            return (await conn.QueryAsync<T>("select LAST_INSERT_ID();")).Single();
        }

        private async Task InsertLog(
            MySqlConnection conn,
            EventType et,
            string requestedById,
            string referenceId = null,
            string ip = null,
            string data = null)
        {
            await conn.ExecuteAsync(insLog, new
            {
                event_type_id = (int)et,
                requested_by_id = requestedById,
                ref_id = referenceId,
                ip,
                info = FormatMsg(et, requestedById, data)
            });
        }

        private string FormatMsg(EventType et, string accountId, string data)
        {
            switch (et)
            {
                case EventType.AccountCreated:
                    return $"Account created with {data}";
                case EventType.AccountUpdated:
                    return $"Account updated with {data}";
                case EventType.PasswordCreated:
                    return $"A new password was created for this account";
                case EventType.PasswordUpdated:
                    return $"The password was updated for this account";
                case EventType.AddressCreated:
                    return $"A new address was registered at '{data}'";
                case EventType.AddressUpdated:
                    return $"Address updated to '{data}'";
                case EventType.AddressRemoved:
                    return $"An existing address registered at '{data}' was removed";
                case EventType.AddressSetDefault:
                    return $"The address '{data}' was set default for this account";
                case EventType.PaymentInfoCreated:
                    return $"A new payment info was added with {data}";
                case EventType.PaymentInfoUpdated:
                    return $"Payment info was updated to {data}";
                case EventType.PaymentInfoRemoved:
                    return $"Payment info with {data} was removed";
                case EventType.PaymentInfoSetDefault:
                    return $"Payment info with {data} was set default";
                default:
                    return $"AccountId: {accountId} requested a {et}.{(data.HasValue() ? " Data: {data}" : "")}";
            }
        }

        private string MaskCC(string number)
        {
            return $"{number.Substring(0, 2)}-xxxx-xxxx-{number.Substring(number.Length - 2, 2)}";
        }
    }
}
