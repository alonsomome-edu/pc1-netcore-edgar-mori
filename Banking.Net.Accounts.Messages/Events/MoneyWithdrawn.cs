using NServiceBus;

namespace Banking.Net.Accounts.Messages.Events
{
    public class MoneyWithdrawn : IEvent
    {
        public string TransactionId { get; protected set; }
        public string FromBankAccountId { get; protected set; }
        public decimal Amount { get; protected set; }
        public decimal Balance { get; protected set; }

        public MoneyWithdrawn(string transactionId, string fromBankAccountId, decimal amount, decimal balance)
        {
            TransactionId = transactionId;
            FromBankAccountId = fromBankAccountId;
            Amount = amount;
            Balance = balance;
        }
    }
}