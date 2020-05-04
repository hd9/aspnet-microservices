using AccountSvc.Models;
using AccountSvc.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountSvc.Services
{
    public class AccountSvc : IAccountSvc
    {

        private readonly IAccountRepository _repo;

        public AccountSvc(IAccountRepository acctRepo)
        {
            _repo = acctRepo;
        }

        public async Task CreateAccount(CreateAccount account)
        {
            await _repo.CreateAccount(account);
        }

        public async Task UpdateAccount(UpdateAccount updAccount)
        {
            await _repo.UpdateAccount(updAccount);
        }

        public async Task UpdatePassword(UpdatePassword updPassword)
        {
            var acc = await _repo.GetAccountById(updPassword.AccountId);

            if (acc.Password != updPassword.CurrentPassword)
            {
                // todo :: log
                return;
            }

            await _repo.UpdatePassword(updPassword);
        }

        public async Task<Account> GetAccountById(string id)
        {
            return await _repo.GetAccountById(id);
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            return await _repo.GetAccountByEmail(email);
        }

        public async Task<Address> GetAddressById(string addrId)
        {
            return await _repo.GetAddressById(addrId);
        }

        public async Task AddAddress(Address addr)
        {
            await _repo.AddAddress(addr);
        }

        public async Task UpdateAddress(Address addr)
        {
            await _repo.UpdateAddress(addr);
        }

        public async Task RemoveAddress(string addressId)
        {
            await _repo.RemoveAddress(addressId);
        }

        public async Task<IList<Address>> GetAddressesByAccountId(string acctId)
        {
            return await _repo.GetAddressesByAccountId(acctId);
        }

        public async Task SetDefultAddress(string acctId, int addressId)
        {
            await _repo.SetDefaultAddress(acctId, addressId);
        }
    }
}
