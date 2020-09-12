using Banking.Net.Command.Accounts.Domain.Entities;
using Banking.Net.Common.Domain.ValueObjects;
using Banking.Net.Command.Transactions.Domain.Entities;
using Banking.Net.Command.Transactions.Domain.Enums;
using Banking.Net.Command.Transactions.Domain.ValueObjects;
using Banking.Net.Transactions.Messages.Commands;
using Banking.Net.Transactions.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Transactions.Handlers.Commands
{
    public class StartTransferHandler : IHandleMessages<StartTransfer>
    {
        static readonly ILog log = LogManager.GetLogger<StartTransferHandler>();

        public async Task Handle(StartTransfer startTransfer, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"StartTransferHandler, TransactionId = {startTransfer.TransactionId}");
                var nHibernateSession = context.SynchronizedStorageSession.Session();
                var transactionId = TransactionId.FromExisting(startTransfer.TransactionId);
                var fromBankAccount = nHibernateSession.Query<BankAccount>().FirstOrDefault
                    (x => x.BankAccountNumber.Number == startTransfer.FromBankAccountNumber) ?? BankAccount.NonExisting();
                if (fromBankAccount.DoesNotExist())
                {
                    return;
                }
                var toBankAccount = nHibernateSession.Query<BankAccount>().FirstOrDefault
                    (x => x.BankAccountNumber.Number == startTransfer.ToBankAccountNumber) ?? BankAccount.NonExisting();
                if (toBankAccount.DoesNotExist())
                {
                    return;
                }
                var money = Money.Dollars(startTransfer.Amount);
                var transactionState = TransactionStateId.STARTED;
                var now = DateTime.UtcNow;
                var transaction = new Transaction(
                    transactionId,
                    fromBankAccount.BankAccountId,
                    toBankAccount.BankAccountId,
                    money,
                    transactionState,
                    now,
                    now
                );
                nHibernateSession.Save(transaction);
                var transferStarted = new TransferStarted
                (
                    startTransfer.TransactionId,
                    fromBankAccount.BankAccountId.Id,
                    toBankAccount.BankAccountId.Id,
                    startTransfer.Amount
                );
                await context.Publish(transferStarted);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }
    }
}