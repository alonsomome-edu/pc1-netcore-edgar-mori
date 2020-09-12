using NServiceBus;

namespace Banking.Net.Accounts.Messages.Commands
{
    public class DepositMoney : ICommand
    {
        public string TransactionId { get; private set; }
        public string ToBankAccountId { get; private set; }
        public decimal Amount { get; private set; }

        public DepositMoney(string transactionId, string toBankAccountId, decimal amount)
        {
            TransactionId = transactionId;
            ToBankAccountId = toBankAccountId;
            Amount = amount;
        }
    }
}