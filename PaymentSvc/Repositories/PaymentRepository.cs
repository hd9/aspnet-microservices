using PaymentSvc.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSvc.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _connStr;
        private readonly string insPmt = "INSERT INTO payment (account_id, created_at, last_modified, currency, amount, status) values (@account_id, sysdate(), sysdate(), @currency, @amount, @status)";
        private readonly string insLog = "INSERT INTO log (pmt_id, created_at) values (@pmt_id, sysdate())";
        private readonly string updPmt = "UPDATE payment (account_id, created_at, last_modified, currency, amount, status) values (@account_id, sysdate(), sysdate(), @currency, @amount, @status)";
        private readonly string queryPmtById = "SELECT * FROM payment WHERE id = @id";
        private readonly string queryPmtByAcctId = "SELECT * FROM payment WHERE account_id = @id";

        public PaymentRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task<int> Insert(PaymentInfo pmt)
        {
            // todo :: salt + hash pwd
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(insPmt, new
                {
                    account_id = pmt.AccountId,
                    currency = pmt.Currency,
                    amount = pmt.Amount,
                    status = (int)pmt.PaymentStatus
                });

                // todo :: get id

                // todo :: log
            }

            return -1; // todo
        }

        public async Task UpdatePayment(PaymentInfo pmt)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(updPmt, new
                {
                    account_id = pmt.AccountId,
                    currency = pmt.Currency,
                    amount = pmt.Amount,
                    status = (int)pmt.PaymentStatus
                });
            }
        }

        public async Task<PaymentInfo> GetById(string pmtId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<PaymentInfo>(queryPmtById, new { pmtId });
            }
        }

        public async Task<PaymentInfo> GetByAccountId(string accountId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<PaymentInfo>(queryPmtByAcctId, new { accountId });
            }
        }
    }
}
