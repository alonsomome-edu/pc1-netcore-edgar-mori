using NServiceBus;

namespace Banking.Net.Transactions.Messages.Commands
{
    public class StartTransfer : ICommand
    {
        public string TransactionId { get; private set; }
        public string FromBankAccountNumber { get; private set; }
        public string ToBankAccountNumber { get; private set; }
        public decimal Amount { get; private set; }

        public StartTransfer(string transactionId, string fromBankAccountNumber, string toBankAccountNumber, decimal amount)
        {
            TransactionId = transactionId;
            FromBankAccountNumber = fromBankAccountNumber;
            ToBankAccountNumber = toBankAccountNumber;
            Amount = amount;
        }
    }
}