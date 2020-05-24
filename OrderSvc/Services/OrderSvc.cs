using HildenCo.Core.Contracts.Orders;
using Core = HildenCo.Core.Contracts.Payment;
using MassTransit;
using OrderSvc.Models;
using OrderSvc.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using HildenCo.Core.Contracts.Shipping;
using HildenCo.Core.Contracts.Notification;
using HildenCo.Core.Contracts.Account;
using HildenCo.Core.Infrastructure.Options;
using HildenCo.Core.Infrastructure.Extensions;

namespace OrderSvc.Services
{
    public class OrderSvc : IOrderSvc
    {

        readonly IOrderRepository _repo;
        readonly IBusControl _bus;
        readonly IRequestClient<AccountInfoRequest> _client;
        readonly List<EmailTemplate> _emailTemplates;

        public OrderSvc(IOrderRepository repo, IBusControl bus, IRequestClient<AccountInfoRequest> client, List<EmailTemplate> emailTemplates)
        {
            _repo = repo;
            _bus = bus;
            _client = client;
            _emailTemplates = emailTemplates;
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            return await _repo.GetOrdersByAccountId(accountId);
        }

        public async Task<int> SubmitOrder(Order order)
        {
            var orderId = await _repo.Insert(order);

            await _bus.Publish(
                new OrderSubmitted 
                {
                    Slugs = order.LineItems.Select(li => li.Slug).ToList()
                });

            // todo :: automapper
            await _bus.Publish(
                new Core.PaymentRequest
                {
                    AccountId = order.AccountId,
                    OrderId = orderId,
                    Currency = order.Currency,
                    Amount = order.TotalPrice,
                    Method = order.PaymentInfo.Method.ToString(),
                    Name = order.PaymentInfo.Name,
                    Number = order.PaymentInfo.Number,
                    ExpDate = order.PaymentInfo.ExpDate,
                    CVV = order.PaymentInfo.CVV,
                    FakeDelay = 2000,
                    FakeResult = true
                }
            );

            return orderId;
        }

        public async Task OnPaymentProcessed(Core.PaymentResponse msg)
        {
            if (msg == null)
                return;

            // load the order
            var order = await _repo.GetById(msg.OrderId);
            var acctInfo = await GetAccount(msg.AccountId);
            
            if (order == null || acctInfo == null || order.AccountId != msg.AccountId)
            {
                return;
            }

            var paymentStatus = Enum.Parse<PaymentStatus>(msg.Status.ToString());

            switch (paymentStatus)
            {
                case PaymentStatus.Authorized:
                    await OnPaymentAutorized(order, acctInfo);
                    break;
                case PaymentStatus.Cancelled:
                    await OnPaymentCancelled(order, acctInfo);
                    break;
                case PaymentStatus.Declined:
                    await OnPaymentDeclined(order, acctInfo);
                    break;
                default:
                    // other statuses not currently implemented
                    break;
            }
        }

        /// <summary>
        /// Gets account information asyncrhonously from the account service
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private async Task<AccountInfo> GetAccount(int accountId)
        {
            using (var request = _client.Create(new AccountInfoRequest { AccountId = accountId }))
            {
                var response = await request.GetResponse<AccountInfoResponse>();

                if (response == null || response.Message == null || response.Message.AccountInfo == null)
                    return null;

                return response.Message.AccountInfo;
            }
        }

        private async Task OnPaymentDeclined(Order order, AccountInfo acctInfo)
        {
            // on payment declined, just send the email and cancel the order
            // note: ideally we'd notify the user and provide them an opportunity to
            //       continue the workflow but that feature is not implemented atm 
            await _repo.Update(
                order.Id,
                OrderStatus.PaymentDeclined,
                PaymentStatus.Declined,
                ShippingStatus.Pending);

            await SendMail(
                acctInfo,
                _emailTemplates.Single(x => x.TemplateName == "PaymentDeclined"));
        }

        private async Task OnPaymentCancelled(Order order, AccountInfo acctInfo)
        {
            // on payment cancelled, just cancel the order and send email
            await _repo.Update(
                order.Id,
                OrderStatus.Cancelled,
                PaymentStatus.Cancelled,
                ShippingStatus.Cancelled);

            await SendMail(
                acctInfo,
                _emailTemplates.Single(x => x.TemplateName == "PaymentCancelled"));
        }

        private async Task OnPaymentAutorized(Order order, AccountInfo acctInfo)
        {
            // on payment authorized, keep the workflow by requesting shipping
            await _repo.Update(
                order.Id,
                OrderStatus.PaymentApproved,
                PaymentStatus.Authorized,
                ShippingStatus.Pending);

            await _bus.Publish(new ShippingRequest
            {
                OrderId = order.Id,
                AccountId = order.AccountId,
                // todo :: add address fields
            });

            await SendMail(
                acctInfo,
                _emailTemplates.Single(x => x.TemplateName == "PaymentAuthorized"));
        }

        private async Task SendMail(AccountInfo acct, EmailTemplate tpl)
        {
            await _bus.Send(new SendMail
            {
                ToName = acct.Name,
                Email = acct.Email,
                FromName = tpl.FromName,
                Body = tpl.Body.FormatWith(acct.Name),
                Subject = tpl.Subject
            });
        }
    }
}
