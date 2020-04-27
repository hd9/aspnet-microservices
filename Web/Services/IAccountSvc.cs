﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IAccountSvc
    {
        Task<Account> TrySignIn(SignIn request);
        Task CreateAccount(Account acct);
        Task UpdateAccount(Account acct);
        Task<Account> GetAccountById(string acctId);
    }
}