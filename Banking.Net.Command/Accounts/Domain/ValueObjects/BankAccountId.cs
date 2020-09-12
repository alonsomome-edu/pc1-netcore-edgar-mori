using Banking.Net.Common.Domain.Entities;

namespace Banking.Net.Command.Accounts.Domain.ValueObjects
{
    public class BankAccountId : Identity
    {
        protected BankAccountId()
        {
        }

        private BankAccountId(string referencedId) : base (referencedId)
        {
        }

        public static BankAccountId FromExisting(string referencedId)
        {
            return new BankAccountId(referencedId);
        }
    }
}