using Banking.Net.Command.Accounts.Domain.Enums;
using Banking.Net.Command.Accounts.Domain.ValueObjects;
using Banking.Net.Command.Customers.Domain.ValueObjects;
using Banking.Net.Common.Domain.Entities;
using Banking.Net.Common.Domain.Enums;
using Banking.Net.Common.Domain.ValueObjects;
using System;

namespace Banking.Net.Command.Accounts.Domain.Entities
{
    public class BankAccount
    {
        public virtual BankAccountId BankAccountId { get; protected set; }
        public virtual BankAccountNumber BankAccountNumber { get; protected set; }
        public virtual Money Balance { get; protected set; }
        public virtual BankAccountStateId BankAccountStateId { get; protected set; }
        public virtual DateTime CreatedAt { get; protected set; }
        public virtual DateTime UpdatedAt { get; protected set; }
        public virtual CustomerId CustomerId { get; protected set; }

        protected BankAccount()
        {
        }
        protected BankAccount(
            BankAccountId bankAccountId,
            BankAccountNumber bankAccountNumber,
            Money balance,
            BankAccountStateId bankAccountStateId,
            DateTime createdAt,
            DateTime updatedAt,
            CustomerId customerId)
        {
            BankAccountId = bankAccountId;
            BankAccountNumber = bankAccountNumber;
            Balance = balance;
            BankAccountStateId = bankAccountStateId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            CustomerId = customerId;
        }

        public static BankAccount From(
            BankAccountId bankAccountId,
            BankAccountNumber bankAccountNumber,
            Money balance,
            BankAccountStateId bankAccountStateId,
            DateTime createdAt,
            DateTime updatedAt,
            CustomerId customerId)
        {
            return new BankAccount(
                bankAccountId,
                bankAccountNumber,
                balance,
                bankAccountStateId,
                createdAt,
                updatedAt,
                customerId);
        }

        public static BankAccount NonExisting()
        {
            DateTime Now = DateTime.Now;
            BankAccountId bankAccountId = BankAccountId.FromExisting(null);
            CustomerId customerId = CustomerId.FromExisting(null);
            return new BankAccount(
                bankAccountId,
                null, 
                null, 
                BankAccountStateId.NULL,
                Now, 
                Now,
                customerId);
        }

        public virtual bool DoesNotExist()
        {
            return BankAccountId == null || !BankAccountId.Ok();
        }

        public virtual bool Exist()
        {
            return BankAccountId != null && BankAccountId.Ok();
        }

        public virtual bool HasIdentity()
        {
            return Exist() && BankAccountNumber.HasValue();
        }

        public virtual void Deposit(decimal amount)
        {
            Notification notification = DepositValidation(amount);
            if (notification.HasErrors())
            {
                throw new ArgumentException(notification.ErrorMessage());
            }
            Money money = new Money(amount, Currency.USD);
            Balance = Balance.Add(money);
        }

        public virtual void Refund(decimal amount)
        {
            Deposit(amount);
        }

        public virtual void Withdraw(decimal amount)
        {
            Notification notification = WithdrawValidation(amount);
            if (notification.HasErrors())
            {
                throw new ArgumentException(notification.ErrorMessage());
            }
            Money money = Money.Dollars(amount);
            Balance = Balance.Subtract(money);
        }

        public virtual Notification DepositValidation(decimal amount)
        {
            Notification notification = new Notification();
            ValidateAmount(notification, amount);
            ValidateBankAcount(notification);
            return notification;
        }

        private void ValidateAmount(Notification notification, decimal amount)
        {
            if (amount <= 0)
            {
                notification.AddError("The amount must be greater than zero");
            }
        }

        private void ValidateBankAcount(Notification notification)
        {
            if (!HasIdentity())
            {
                notification.AddError("The account has no identity");
            }
            if (BankAccountStateId != BankAccountStateId.ACTIVE)
            {
                notification.AddError("Invaid BankAccountState");
            }
        }

        public virtual Notification WithdrawValidation(decimal amount)
        {
            Notification notification = new Notification();
            ValidateAmount(notification, amount);
            ValidateBankAcount(notification);
            ValidateBalance(notification, amount);
            return notification;
        }

        private void ValidateBalance(Notification notification, decimal amount)
        {
            if (!CanBeWithdrawed(amount))
            {
                notification.AddError("Cannot withdraw in the account, amount is greater than balance");
            }
        }

        public virtual bool CanBeWithdrawed(decimal amount)
        {
            return BankAccountStateId == BankAccountStateId.ACTIVE && Balance.GreaterOrEqualThan(amount);
        }

        public virtual void ChangeUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}