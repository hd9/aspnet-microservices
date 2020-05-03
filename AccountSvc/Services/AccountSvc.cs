using AccountSvc.Models;
using AccountSvc.Repositories;
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

        public async Task CreateAccount(Account account)
        {
            await _repo.CreateAccount(account);
        }

        public async Task UpdateAccount(Account account)
        {
            await _repo.UpdateAccount(account);
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
    }
}
