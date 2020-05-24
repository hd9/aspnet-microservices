using PaymentSvc.Models;
using PaymentSvc.Repositories;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Core = HildenCo.Core.Contracts.Payment;


namespace PaymentSvc.Services
{
    public class PaymentSvc : IPaymentSvc
    {

        readonly IPaymentRepository _repo;
        readonly IPaymentGateway _pmtGateway;
        readonly IBusControl _bus;

        public PaymentSvc(IPaymentGateway pmtGateway, IPaymentRepository repo, IBusControl bus)
        {
            _repo = repo;
            _pmtGateway = pmtGateway;
            _bus = bus;
        }

        public async Task RequestPayment(Core.PaymentRequest pr)
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

            await _bus.Publish(new Core.PaymentResponse
            {
                AccountId = pmt.AccountId,
                OrderId = pmt.OrderId,
                Status = Enum.Parse<Core.PaymentStatus>(pmt.Status.ToString())
            });
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
