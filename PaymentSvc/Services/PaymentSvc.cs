using PaymentSvc.Models;
using PaymentSvc.Repositories;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HildenCo.Core.Contracts.Payment;

namespace PaymentSvc.Services
{
    public class PaymentSvc : IPaymentSvc
    {

        readonly IPaymentRepository _repo;
        readonly IPaymentGateway _pmtGateway;

        public PaymentSvc(IPaymentGateway pmtGateway, IPaymentRepository repo)
        {
            _repo = repo;
            _pmtGateway = pmtGateway;
        }

        public async Task RequestPayment(PaymentRequest pr)
        {
            if (pr == null)
                return;

            var pmt = Payment.Parse(pr);
            await _repo.InsertPayment(pmt);

            // todo :: automapper
            var pgr = new PaymentGatewayRequest
            {
                PaymentId = pmt.Id,
                Amount = pr.Amount,
                Currency = pr.Currency,
                Name = pr.Name,
                Number = pr.Number,
                CVV = pr.CVV,
                ExpDate = pr.ExpDate,
                Method = Enum.Parse<PaymentMethod>(pr.Method, true),
                FakeDelay = pr.FakeDelay,                               // fake stuff
                FakeResult = pr.FakeResult,                             // fake stuff
            };

            var resp = await _pmtGateway.Process(pgr);
            pgr.Status = resp.Status;
            pmt.AuthCode = pgr.AuthCode = resp.AuthCode;

            await _repo.InsertPaymentRequest(pgr);
            
            pmt.Status = 
                resp.Status == PaymentGatewayResponseStatus.Authorized ? 
                    PaymentStatus.Authorized : 
                    PaymentStatus.Declined;

            await _repo.UpdatePayment(pmt);
        }

        public async Task<Payment> GetById(string id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Payment> GetByAccountId(string email)
        {
            return await _repo.GetByAccountId(email);
        }
    }
}
