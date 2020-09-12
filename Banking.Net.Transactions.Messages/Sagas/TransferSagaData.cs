using NServiceBus;

namespace Banking.Net.Transactions.Messages.Sagas
{
    public class TransferSagaData : ContainSagaData
    {
        public virtual string TransactionId { get; set; }
        public virtual string FromBankAccountId { get; set; }
        public virtual string ToBankAccountId { get; set; }
        public virtual decimal Amount { get; set; }
    }
}