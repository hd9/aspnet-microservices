using System;
using Dapper;
using PaymentSvc.Models;
using PaymentSvc.Repositories;
using System.Threading.Tasks;
using MassTransit;
using Core = Microservices.Core.Contracts.Payment;
using Microservices.Core.Infrastructure.Extensions;

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
                Method = Enum.Parse<PaymentMethod>(pr.Method),
                FakeDelay = pr.FakeDelay,                                   // fake stuff
                FakeStatus = pr.FakeStatus.Parse<PaymentStatus>()           // fake stuff
            };

            var resp = await _pmtGateway.Process(pgr);
            pgr.Status = resp.Status;
            pmt.AuthCode = pgr.AuthCode = resp.AuthCode;
            pmt.Status = resp.Status.Parse<PaymentStatus>();

            await _repo.InsertPaymentRequest(pgr);
            await _repo.UpdatePayment(pmt);

            await _bus.Publish(new Core.PaymentResponse
            {
                AccountId = pmt.AccountId,
                OrderId = pmt.OrderId,
                Status = pmt.Status.Parse<Core.PaymentStatus>()
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
