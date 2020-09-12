using NServiceBus;

namespace Banking.Net.Transactions.Messages.Events
{
    public class TransferStarted : IEvent
    {
        public string TransactionId { get; protected set; }
        public string FromBankAccountId { get; protected set; }
        public string ToBankAccountId { get; protected set; }
        public decimal Amount { get; protected set; }

        public TransferStarted(string transactionId, string fromBankAccountId, string toBankAccountId, decimal amount)
        {
            TransactionId = transactionId;
            FromBankAccountId = fromBankAccountId;
            ToBankAccountId = toBankAccountId;
            Amount = amount;
        }
    }
}