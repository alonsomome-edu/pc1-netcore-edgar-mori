using System;
using System.Collections.Generic;
using static System.String;

namespace Banking.Net.Common.Domain.ValueObjects
{
    public class Name : ValueObject<Name>
    {
        public virtual string FirstName { get; private set; }
        public virtual string LastName { get; private set; }

        public virtual string FullName
        {
            get
            {
                return Format("{0} {1}", FirstName, LastName);
            }
        }

        public static void CheckValidity(string name)
        {
            if (IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Value cannot be empty");
            }

            if (name.Length < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Value cannot be shorter than 10 characters");
            }

            if (name.Length > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Value cannot be longer than 100 characters");
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}