using AccountSvc.Infrastructure.Options;
using AccountSvc.Models;
using AccountSvc.Repositories;
using Core.Commands;
using Core.Events.Newsletter;
using Core.Infrastructure.Extentions;
using Core.Infrastructure.Options;
using MassTransit;
using NewsletterSvc.Infrastructure.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountSvc.Services
{
    public class AccountSvc : IAccountSvc
    {
        private readonly IAccountRepository _repo;
        private readonly IBusControl _bus;
        private readonly List<EmailTemplate> _emailTemplates;

        public AccountSvc(IAccountRepository acctRepo, IBusControl bus, List<EmailTemplate> emailTemplates)
        {
            _repo = acctRepo;
            _bus = bus;
            _emailTemplates = emailTemplates;
        }

        public async Task CreateAccount(CreateAccount cmd)
        {
            await _repo.CreateAccount(cmd);

            var tpl = _emailTemplates.Single(m =>
                m.TemplateName == "AccountCreated");

            await _bus.Publish(
                new SendMail
                {
                    ToName = cmd.Name,
                    Email = cmd.Email,
                    Subject = tpl.Subject,
                    Body = tpl.Body.FormatWith(cmd.Name)
                });

            await _bus.Publish(
                new AccountCreated
                {
                    Name = cmd.Name,
                    Email = cmd.Email
                });
        }

        public async Task UpdateAccount(UpdateAccount cmd)
        {
            await _repo.UpdateAccount(cmd);

            var tpl = _emailTemplates.Single(m =>
                m.TemplateName == "AccountUpdated");

            await _bus.Publish(
                new SendMail
                {
                    ToName = cmd.Name,
                    Email = cmd.Email,
                    Subject = tpl.Subject,
                    Body = tpl.Body.FormatWith(cmd.Name)
                });
        }

        public async Task UpdatePassword(UpdatePassword cmd)
        {
            var acct = await _repo.GetAccountById(cmd.AccountId);

            if (acct.Password != cmd.CurrentPassword)
            {
                // todo :: log
                return;
            }

            await _repo.UpdatePassword(cmd);

            var tpl = _emailTemplates.Single(m =>
                m.TemplateName == "PasswordUpdated");

            await _bus.Publish(
                new SendMail
                {
                    ToName = acct.Name,
                    Email = acct.Email,
                    Subject = tpl.Subject,
                    Body = tpl.Body.FormatWith(acct.Name)
                });
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

        public async Task<IList<AccountHistory>> GetAccountHistory(string acctId)
        {
            return await _repo.GetAccountHistory(acctId);
        }

        public async Task SubscribeToNewsletter(string email)
        {
            await _repo.UpdateNewsletterSubscription(email);
        }
    }
}
