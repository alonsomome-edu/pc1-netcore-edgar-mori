using NServiceBus;

namespace Banking.Net.Transactions.Messages.Commands
{
    public class CompleteTransfer : ICommand
    {
        public string TransactionId { get; private set; }

        public CompleteTransfer(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}