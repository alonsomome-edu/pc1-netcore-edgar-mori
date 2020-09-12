using Banking.Net.Command.Transactions.Domain.Entities;
using Banking.Net.Command.Transactions.Domain.ValueObjects;
using Banking.Net.Transactions.Messages.Commands;
using Banking.Net.Transactions.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Banking.Net.Transactions.Handlers.Commands
{
    public class RejectTransferHandler : IHandleMessages<RejectTransfer>
    {
        static readonly ILog log = LogManager.GetLogger<RejectTransferHandler>();

        public async Task Handle(RejectTransfer rejectTransfer, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"RejectTransferHandler, TransactionId = {rejectTransfer.TransactionId}");
                var nHibernateSession = context.SynchronizedStorageSession.Session();
                var transactionId = TransactionId.FromExisting(rejectTransfer.TransactionId);
                var transaction = nHibernateSession.Get<Transaction>(transactionId) ?? Transaction.NonExisting();
                if (transaction.DoesNotExist())
                {
                    return;
                }
                transaction.Reject();
                transaction.ChangeUpdatedAt();
                nHibernateSession.Save(transaction);
                var transferRejected = new TransferRejected
                (
                    rejectTransfer.TransactionId
                );
                await context.Publish(transferRejected);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }
    }
}