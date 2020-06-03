using ShippingSvc.Models;
using ShippingSvc.Services;
using System.Threading.Tasks;

namespace ShippingSvc.Repositories
{
    public interface IShippingRepository
    {
        Task Insert(Shipping pmt);
        Task<Shipping> GetById(string id);
        Task<Shipping> GetByAccountId(string email);
    }
}