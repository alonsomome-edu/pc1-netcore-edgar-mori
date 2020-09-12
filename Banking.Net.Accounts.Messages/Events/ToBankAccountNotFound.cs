using NServiceBus;

namespace Banking.Net.Accounts.Messages.Events
{
    public class ToBankAccountNotFound : IEvent
    {
        public string TransactionId { get; protected set; }

        public ToBankAccountNotFound(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}