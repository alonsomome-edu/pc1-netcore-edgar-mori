using NServiceBus;

namespace Banking.Net.Accounts.Messages.Events
{
    public class FromBankAccountNotFound : IEvent
    {
        public string TransactionId { get; protected set; }

        public FromBankAccountNotFound(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}