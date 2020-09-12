using NServiceBus;

namespace Banking.Net.Transactions.Messages.Events
{
    public class TransferCompleted : IEvent
    {
        public string TransactionId { get; protected set; }

        public TransferCompleted(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}