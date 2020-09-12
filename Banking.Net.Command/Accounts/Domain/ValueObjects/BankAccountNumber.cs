using Banking.Net.Common.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using static System.String;

namespace Banking.Net.Command.Accounts.Domain.ValueObjects
{
    public class BankAccountNumber : ValueObject<BankAccountNumber>
    {
        public virtual string Number { get; private set; }

        public static void CheckValidity(string number)
        {
            if (IsNullOrEmpty(number))
            {
                throw new ArgumentNullException(nameof(number), "Account Number cannot be empty");
            }

            if (number.Length < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Account Number cannot be shorter than 10 characters");
            }

            if (number.Length > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Account Number cannot be longer than 100 characters");
            }
        }

        public virtual bool HasValue()
        {
            return Number.Trim() != Empty;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }
    }
}