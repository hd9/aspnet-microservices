using System.Collections.Generic;
using System.Threading.Tasks;
using AccountSvc.Models;
using Web.Models;

namespace AccountSvc.Services
{
    public interface IAccountSvc
    {
        Task CreateAccount(CreateAccount account);
        Task UpdateAccount(UpdateAccount updAccount);
        Task UpdatePassword(UpdatePassword updPassword);
        Task<Account> GetAccountById(string id);
        Task<Account> GetAccountByEmail(string email);
        Task<Address> GetAddressById(string addrId);
        Task AddAddress(Address addr);
        Task UpdateAddress(Address addr);
        Task SubscribeToNewsletter(string email);
        Task RemoveAddress(string addressId);
        Task<IList<Address>> GetAddressesByAccountId(string acctId);
        Task SetDefultAddress(string acctId, int addressId);
        Task<PaymentInfo> GetPaymentInfoById(string pmtId);
        Task AddPaymentInfo(PaymentInfo pmtId);
        Task UpdatePaymentInfo(PaymentInfo pmtId);
        Task RemovePaymentInfo(string pmtId);
        Task<IList<PaymentInfo>> GetPaymentInfosByAccountId(string accountId);
        Task SetDefaultPaymentInfo(string accountId, int pmtId);
        Task<IList<AccountHistory>> GetAccountHistory(string acctId);
        Task<Account> TrySignIn(SignIn signin);
    }
}