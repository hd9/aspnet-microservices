using PaymentSvc.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HildenCo.Core.Infrastructure.Extensions;

namespace PaymentSvc.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _connStr;
        private readonly string insPmt = "INSERT INTO payment (account_id, order_id, amount, currency, method, status, auth_code, created_at, last_modified) values (@account_id, @order_id, @amount, @currency, @method, @status, @auth_code, sysdate(), sysdate());";
        private readonly string updPmt = "update payment set status = @status, auth_code = @auth_code, last_modified = sysdate() where @id = id;";
        private readonly string insPmtRequest = "INSERT INTO payment_request (pmt_gateway_id, pmt_id, amount, currency, name, number, cvv, exp_date, method, status, auth_code, created_at) values (@pmt_gateway_id, @pmt_id, @amount, @currency, @name, @number, @cvv, @exp_date, @method, @status, @auth_code, sysdate());";
        private readonly string queryPmtById = "SELECT * FROM payment WHERE id = @id";
        private readonly string queryPmtByAcctId = "SELECT * FROM payment WHERE account_id = @id";

        public PaymentRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task InsertPayment(Payment pmt)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(insPmt, new
                {
                    @account_id = pmt.AccountId,
                    @order_id = pmt.OrderId,
                    @amount = pmt.Amount,
                    @currency = pmt.Currency,
                    @method = pmt.Method,
                    @status = (int)pmt.Status,
                    @auth_code = pmt.AuthCode
                });

                pmt.Id = (await conn.QueryAsync<int>("select LAST_INSERT_ID();")).Single();
            }
        }

        public async Task UpdatePayment(Payment pmt)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(updPmt, new
                {
                    id = pmt.Id,
                    @status = (int)pmt.Status,
                    @auth_code = pmt.AuthCode
                });
            }
        }

        public async Task InsertPaymentRequest(PaymentGatewayRequest pgr)
        {
            if (pgr == null)
                return;

            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.ExecuteAsync(insPmtRequest, new
                {
                    @pmt_gateway_id = pgr.PaymentGatewayId,
                    @pmt_id = pgr.PaymentId,
                    @currency = pgr.Currency,
                    @name = pgr.Name,
                    @number = pgr.Number.MaskCC(),
                    @cvv = pgr.CVV,
                    @exp_date = pgr.ExpDate,
                    @method = (int)pgr.Method,
                    @amount = pgr.Amount,
                    @status = (int)pgr.Status,
                    @auth_code = pgr.AuthCode
                });
            }
        }

        public async Task<Payment> GetById(string pmtId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Payment>(queryPmtById, new { pmtId });
            }
        }

        public async Task<Payment> GetByAccountId(string accountId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return await conn.QuerySingleOrDefaultAsync<Payment>(queryPmtByAcctId, new { accountId });
            }
        }

    }
}
