using AccountSvc.Services;
using Microservices.Core.Contracts.Account;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountSvc.Consumers
{
    /// <summary>
    /// AccountInfoRequestConsumer consumes to AccountInfoRequest events
    /// and responds asynchronously with account information.
    /// </summary>
    public class AccountInfoRequestConsumer
        : IConsumer<AccountInfoRequest>
    {

        readonly IAccountSvc _svc;

        public AccountInfoRequestConsumer(IAccountSvc svc)
        {
            _svc = svc;
        }

        public async Task Consume(ConsumeContext<AccountInfoRequest> context)
        {
            var acctId = context.Message.AccountId;
            if (acctId <= 0)
            {
                await context.RespondAsync(null);
                return;
            }

            var acct = await _svc.GetAccountById(acctId.ToString());
            await context.RespondAsync(new AccountInfoResponse
            {
                AccountInfo = new AccountInfo
                {
                    Id = acctId,
                    Name = acct.Name,
                    Email = acct.Email
                }
            });
        }
    }
}
