using System;
using Dapper;
using ShippingSvc.Models;
using ShippingSvc.Repositories;
using System.Threading.Tasks;
using MassTransit;
using Core = HildenCo.Core.Contracts.Shipping;
using HildenCo.Core.Infrastructure.Extensions;

namespace ShippingSvc.Services
{
    public class ShippingSvc : IShippingSvc
    {

        readonly IShippingRepository _repo;
        readonly IBusControl _bus;

        public ShippingSvc(IShippingRepository repo, IBusControl bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task RequestShipping(Core.ShippingRequest sr)
        {
            if (sr == null)
                return;

            var shipping = Shipping.Create(sr);
            await _repo.Insert(shipping);

            // yes, shipping svc is slow... =) 
            await Task.Delay(5000);

            // set shipping delivered so order can be marked as complete
            // Note: other workflows not implemented
            shipping.Status = ShippingStatus.Delivered;

            await _bus.Publish(new Core.ShippingResponse
            {
                Number = shipping.Number,
                AccountId = shipping.AccountId,
                OrderId = shipping.OrderId,
                Status = shipping.Status.Parse<Core.ShippingStatus>()
            });
        }

        public async Task<Shipping> GetById(string id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Shipping> GetByAccountId(string email)
        {
            return await _repo.GetByAccountId(email);
        }
    }
}
