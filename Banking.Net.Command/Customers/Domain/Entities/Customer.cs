using System;
using Banking.Net.Command.Customers.Domain.ValueObjects;
using Banking.Net.Common.Domain.ValueObjects;

namespace Banking.Net.Command.Customers.Domain.Entities
{
    public class Customer
    {
        public virtual CustomerId CustomerId { get; protected set; }
        public virtual Name Name { get; protected set; }
        public virtual bool Active { get; protected set; }
        public virtual DateTime CreatedAt { get; protected set; }
        public virtual DateTime UpdatedAt { get; protected set; }

        protected Customer()
        {
        }

        protected Customer(
            CustomerId customerId,
            Name name,
            bool active,
            DateTime createdAt,
            DateTime updatedAt)
        {
            CustomerId = customerId;
            Name = name;
            Active = active;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Customer From(
            CustomerId customerId,
            Name name,
            bool active,
            DateTime createdAt,
            DateTime updatedAt)
        {
            return new Customer(
                customerId,
                name,
                active,
                createdAt, 
                updatedAt);
        }

        public static Customer NonExisting()
        {
            CustomerId customerId = CustomerId.FromExisting(null);
            DateTime Now = DateTime.Now;
            return new Customer(
                customerId, 
                null,
                true, 
                Now, 
                Now);
        }

        public virtual bool DoesNotExist()
        {
            return CustomerId == null || !CustomerId.Ok();
        }

        public virtual bool Exist()
        {
            return CustomerId != null && CustomerId.Ok();
        }
    }
}