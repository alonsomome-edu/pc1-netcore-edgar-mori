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
    public class DepositMoneyHandler : IHandleMessages<DepositMoney>
    {
        static readonly ILog log = LogManager.GetLogger<DepositMoneyHandler>();

        public async Task Handle(DepositMoney depositMoney, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"DepositMoneyHandler, TransactionId = {depositMoney.TransactionId}");
                var nHibernateSession = context.SynchronizedStorageSession.Session();
                var bankAccountId = BankAccountId.FromExisting(depositMoney.ToBankAccountId);
                var toBankAccount = nHibernateSession.Get<BankAccount>(bankAccountId) ?? BankAccount.NonExisting();
                if (toBankAccount.DoesNotExist())
                {
                    var toBankAccountNotFound = new ToBankAccountNotFound(depositMoney.TransactionId);
                    await context.Publish(toBankAccountNotFound);
                    return;
                }
                toBankAccount.Deposit(depositMoney.Amount);
                toBankAccount.ChangeUpdatedAt();
                nHibernateSession.Save(toBankAccount);
                var moneyDeposited = new MoneyDeposited
                (
                    depositMoney.TransactionId,
                    toBankAccount.BankAccountId.Id,
                    depositMoney.Amount,
                    toBankAccount.Balance.Amount
                );
                await context.Publish(moneyDeposited);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }
    }
}