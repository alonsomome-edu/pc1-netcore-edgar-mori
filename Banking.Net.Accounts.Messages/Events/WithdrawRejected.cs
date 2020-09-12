using NServiceBus;

namespace Banking.Net.Accounts.Messages.Events
{
    public class WithdrawRejected : IEvent
    {
        public string TransactionId { get; protected set; }

        public WithdrawRejected(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}