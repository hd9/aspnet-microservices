using AccountSvc.Models;
using AccountSvc.Repositories;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // todo :: hash pwd
            await _repo.CreateAccount(account);
        }

        public async Task UpdateAccount(Account account)
        {
            // todo :: acctRepo
            await _repo.UpdateAccount(account);
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
