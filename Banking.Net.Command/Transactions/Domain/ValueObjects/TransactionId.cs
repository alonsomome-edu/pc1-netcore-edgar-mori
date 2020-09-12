using Banking.Net.Common.Domain.Entities;

namespace Banking.Net.Command.Transactions.Domain.ValueObjects
{
    public class TransactionId : Identity
    {
        protected TransactionId()
        {
        }

        private TransactionId(string referencedId) : base(referencedId)
        {
        }

        public static TransactionId FromExisting(string referencedId)
        {
            return new TransactionId(referencedId);
        }
    }
}