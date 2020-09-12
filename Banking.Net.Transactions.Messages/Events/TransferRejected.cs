using NServiceBus;

namespace Banking.Net.Transactions.Messages.Events
{
    public class TransferRejected : IEvent
    {
        public string TransactionId { get; protected set; }

        public TransferRejected(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}