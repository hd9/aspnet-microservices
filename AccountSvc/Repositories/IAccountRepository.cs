using AccountSvc.Models;
using System.Threading.Tasks;

namespace AccountSvc.Repositories
{
    public interface IAccountRepository
    {
        Task CreateAccount(Account account);
        Task UpdateAccount(UpdateAccount updAccount);
        Task UpdatePassword(UpdatePassword updPassword);
        Task<Account> GetAccountByEmail(string email);
        Task<Account> GetAccountById(string id);
    }
}