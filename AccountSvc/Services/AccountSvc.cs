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

        public async Task<PaymentInfo> GetPaymentInfoById(string pmtId)
        {
            return await _repo.GetPaymentInfoById(pmtId);
        }

        public async Task AddPaymentInfo(PaymentInfo pmtId)
        {
            await _repo.AddPaymentInfo(pmtId);
        }

        public async Task UpdatePaymentInfo(PaymentInfo pmtId)
        {
            await _repo.UpdatePaymentInfo(pmtId);
        }

        public async Task RemovePaymentInfo(string pmtId)
        {
            await _repo.RemovePaymentInfo(pmtId);
        }

        public async Task<IList<PaymentInfo>> GetPaymentInfosByAccountId(string accountId)
        {
            return await _repo.GetPaymentInfosByAccountId(accountId);
        }

        public async Task SetDefaultPaymentInfo(string accountId, int pmtId)
        {
            await _repo.SetDefaultPaymentInfo(accountId, pmtId);
        }
    }
}
