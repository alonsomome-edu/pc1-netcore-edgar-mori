using NServiceBus;

namespace Banking.Net.Accounts.Messages.Events
{
    public class DepositRejected : IEvent
    {
        public string TransactionId { get; protected set; }

        public DepositRejected(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}