using Banking.Net.Common.Domain.Entities;

namespace Banking.Net.Command.Customers.Domain.ValueObjects
{
    public class CustomerId : Identity
    {
        protected CustomerId()
        {
        }

        private CustomerId(string referencedId) : base(referencedId)
        {
        }

        public static CustomerId FromExisting(string referencedId)
        {
            return new CustomerId(referencedId);
        }
    }
}
