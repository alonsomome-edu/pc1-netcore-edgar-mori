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
    public class CompleteTransferHandler : IHandleMessages<CompleteTransfer>
    {
        static readonly ILog log = LogManager.GetLogger<CompleteTransferHandler>();

        public async Task Handle(CompleteTransfer completeTransfer, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"CompleteTransferHandler, TransferId = {completeTransfer.TransactionId}");
                var nHibernateSession = context.SynchronizedStorageSession.Session();
                var transactionId = TransactionId.FromExisting(completeTransfer.TransactionId);
                var transaction = nHibernateSession.Get<Transaction>(transactionId) ?? Transaction.NonExisting();
                if (transaction.DoesNotExist())
                {
                    return;
                }
                transaction.Complete();
                transaction.ChangeUpdatedAt();
                nHibernateSession.Save(transaction);
                var transferCompleted = new TransferCompleted
                (
                    completeTransfer.TransactionId
                );
                await context.Publish(transferCompleted);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }
    }
}