using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Core.Contracts.Shipping;
using ShippingSvc.Models;

namespace ShippingSvc.Services
{
    public interface IShippingSvc
    {
        Task RequestShipping(ShippingRequest shippingRequest);
        Task<Shipping> GetById(string id);
        Task<Shipping> GetByAccountId(string accountId);
    }
}