using NServiceBus;

namespace Banking.Net.Accounts.Messages.Events
{
    public class MoneyRefunded : IEvent
    {
        public string BankAccountId { get; protected set; }
        public decimal Amount { get; protected set; }

        public MoneyRefunded(string bankAccountId, decimal amount)
        {
            BankAccountId = bankAccountId;
            Amount = amount;
        }
    }
}