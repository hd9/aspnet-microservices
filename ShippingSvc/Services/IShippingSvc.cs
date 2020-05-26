using System.Collections.Generic;
using System.Threading.Tasks;
using HildenCo.Core.Contracts.Shipping;
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