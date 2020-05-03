using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IAccountProxy
    {
        Task<Account> TrySignIn(SignIn request);
        Task CreateAccount(Account acct);
        Task UpdateAccount(Account acct);
        Task UpdatePassword(UpdatePassword changePassword);
        Task<Account> GetAccountById(string acctId);
    }
}