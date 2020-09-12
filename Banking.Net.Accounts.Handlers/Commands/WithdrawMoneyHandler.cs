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
    public class WithdrawMoneyHandler : IHandleMessages<WithdrawMoney>
    {
        static readonly ILog log = LogManager.GetLogger<WithdrawMoney>();

        public async Task Handle(WithdrawMoney withdrawMoney, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"WithdrawMoneyHandler, TransactionId = {withdrawMoney.TransactionId}");
                var nHibernateSession = context.SynchronizedStorageSession.Session();
                var bankAccountId = BankAccountId.FromExisting(withdrawMoney.FromBankAccountId);
                var fromBankAccount = nHibernateSession.Get<BankAccount>(bankAccountId) ?? BankAccount.NonExisting();
                if (fromBankAccount.DoesNotExist())
                {
                    var fromBankAccountNotFound = new FromBankAccountNotFound(withdrawMoney.TransactionId);
                    await context.Publish(fromBankAccountNotFound);
                    return;
                }
                if (fromBankAccount.CanBeWithdrawed(withdrawMoney.Amount))
                {
                    fromBankAccount.Withdraw(withdrawMoney.Amount);
                    fromBankAccount.ChangeUpdatedAt();
                    nHibernateSession.Save(fromBankAccount);
                    var moneyWithdrawn = new MoneyWithdrawn
                    (
                        withdrawMoney.TransactionId,
                        withdrawMoney.FromBankAccountId,
                        withdrawMoney.Amount,
                        fromBankAccount.Balance.Amount
                    );
                    await context.Publish(moneyWithdrawn);
                    return;
                }
                var withdrawRejected = new WithdrawRejected
                (
                    withdrawMoney.TransactionId
                );
                await context.Publish(withdrawRejected);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }
    }
}