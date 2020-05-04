using System.Collections.Generic;
using System.Threading.Tasks;
using AccountSvc.Models;

namespace AccountSvc.Services
{
    public interface IAccountSvc
    {
        Task CreateAccount(Account account);
        Task UpdateAccount(UpdateAccount updAccount);
        Task UpdatePassword(UpdatePassword updPassword);
        Task<Account> GetAccountById(string id);
        Task<Account> GetAccountByEmail(string email);
    }
}