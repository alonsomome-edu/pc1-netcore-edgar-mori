using Banking.Net.Accounts.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Banking.Net.Accounts.Handlers.Events
{
    public class MoneyRefundedHandler : IHandleMessages<MoneyRefunded>
    {
        static readonly ILog log = LogManager.GetLogger<MoneyRefundedHandler>();

        public Task Handle(MoneyRefunded moneyRefunded, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"MoneyRefundedHandler, BankAccountId = {moneyRefunded.BankAccountId}");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
            return Task.CompletedTask;
        }
    }
}