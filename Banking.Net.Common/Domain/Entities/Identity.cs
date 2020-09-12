using System;

namespace Banking.Net.Common.Domain.Entities
{
    public class Identity
    {
		public virtual string Id { get; protected set; }

		protected Identity() : this(Guid.NewGuid().ToString())
		{
		}

        protected Identity(string referencedId)
		{
			Id = referencedId;
		}

		public bool Ok()
		{
			return Id.Length == 36;
		}

		public override int GetHashCode()
		{
			return (GetType().GetHashCode() * 907) + Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Identity other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return Id.Equals(other.Id);
		}
	}
}