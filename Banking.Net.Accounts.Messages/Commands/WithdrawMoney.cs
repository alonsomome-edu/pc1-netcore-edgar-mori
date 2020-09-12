using NServiceBus;

namespace Banking.Net.Accounts.Messages.Commands
{
    public class WithdrawMoney : ICommand
    {
        public string TransactionId { get; private set; }
        public string FromBankAccountId { get; private set; }
        public decimal Amount { get; private set; }

        public WithdrawMoney(string transactionId, string fromBankAccountId, decimal amount)
        {
            TransactionId = transactionId;
            FromBankAccountId = fromBankAccountId;
            Amount = amount;
        }
    }
}