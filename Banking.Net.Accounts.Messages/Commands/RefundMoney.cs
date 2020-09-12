using NServiceBus;

namespace Banking.Net.Accounts.Messages.Commands
{
    public class RefundMoney : ICommand
    {
        public string BankAccountId { get; private set; }
        public decimal Amount { get; private set; }

        public RefundMoney(string bankAccountId, decimal amount)
        {
            BankAccountId = bankAccountId;
            Amount = amount;
        }
    }
}