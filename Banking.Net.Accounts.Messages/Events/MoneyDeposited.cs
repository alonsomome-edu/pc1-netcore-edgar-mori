using NServiceBus;

namespace Banking.Net.Accounts.Messages.Events
{
    public class MoneyDeposited : IEvent
    {
        public string TransactionId { get; protected set; }
        public string ToBankAccountId { get; protected set; }
        public decimal Amount { get; protected set; }
        public decimal Balance { get; protected set; }

        public MoneyDeposited(string transactionId, string toBankAccountId, decimal amount, decimal balance)
        {
            TransactionId = transactionId;
            ToBankAccountId = toBankAccountId;
            Amount = amount;
            Balance = balance;
        }
    }
}