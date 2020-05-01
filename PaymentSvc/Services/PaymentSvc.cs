using PaymentSvc.Models;
using PaymentSvc.Repositories;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSvc.Services
{
    public class PaymentSvc : IPaymentSvc
    {

        private readonly IPaymentRepository _repo;

        public PaymentSvc(IPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> SubmitPayment(PaymentInfo pmt)
        {
            return await _repo.Insert(pmt);
        }

        public async Task<PaymentInfo> GetById(string id)
        {
            return await _repo.GetById(id);
        }

        public async Task<PaymentInfo> GetByAccountId(string email)
        {
            return await _repo.GetByAccountId(email);
        }
    }
}
