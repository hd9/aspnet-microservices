using AccountSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountSvc.Repositories
{
    public interface IAccountRepository
    {
        Task CreateAccount(CreateAccount account);
        Task UpdateAccount(UpdateAccount updAccount);
        Task UpdatePassword(UpdatePassword updPassword);
        Task<Account> GetAccountByEmail(string email);
        Task<Account> GetAccountById(string id);
        Task<Address> GetAddressById(string addrId);
        Task AddAddress(Address addr);
        Task UpdateAddress(Address addr);
        Task RemoveAddress(string addressId);
        Task<IList<Address>> GetAddressesByAccountId(string acctId);
        Task SetDefaultAddress(string acctId, int addressId);
    }
}