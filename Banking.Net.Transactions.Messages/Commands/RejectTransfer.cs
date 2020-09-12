using NServiceBus;

namespace Banking.Net.Transactions.Messages.Commands
{
    public class RejectTransfer : ICommand
    {
        public string TransactionId { get; private set; }

        public RejectTransfer(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}