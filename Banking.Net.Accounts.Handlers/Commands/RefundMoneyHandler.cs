using Banking.Net.Command.Accounts.Domain.Entities;
using Banking.Net.Command.Accounts.Domain.ValueObjects;
using Banking.Net.Accounts.Messages.Commands;
using Banking.Net.Accounts.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Banking.Net.Accounts.Handlers.Commands
{
    public class RefundMoneyHandler : IHandleMessages<RefundMoney>
    {
        static readonly ILog log = LogManager.GetLogger<RefundMoneyHandler>();

        public async Task Handle(RefundMoney refundMoney, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"RefundMoneyHandler, BankAccountId = {refundMoney.BankAccountId}");
                var nHibernateSession = context.SynchronizedStorageSession.Session();
                var bankAccountId = BankAccountId.FromExisting(refundMoney.BankAccountId);
                var bankAccount = nHibernateSession.Get<BankAccount>(bankAccountId) ?? BankAccount.NonExisting();
                if (bankAccount.DoesNotExist())
                {
                    var fromBankAccountNotFound = new FromBankAccountNotFound(refundMoney.BankAccountId);
                    await context.Publish(fromBankAccountNotFound);
                    return;
                }
                bankAccount.Refund(refundMoney.Amount);
                nHibernateSession.Save(bankAccount);
                var moneyRefunded = new MoneyRefunded
                (
                    bankAccount.BankAccountId.Id,
                    refundMoney.Amount
                );
                await context.Publish(moneyRefunded);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }
    }
}