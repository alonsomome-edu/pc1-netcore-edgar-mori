using Banking.Net.Command.Accounts.Domain.ValueObjects;
using Banking.Net.Command.Transactions.Domain.Enums;
using Banking.Net.Command.Transactions.Domain.ValueObjects;
using Banking.Net.Common.Domain.ValueObjects;
using System;

namespace Banking.Net.Command.Transactions.Domain.Entities
{
    public class Transaction
    {
        public virtual TransactionId TransactionId { get; protected set; }
        public virtual BankAccountId FromBankAccountId { get; protected set; }
        public virtual BankAccountId ToBankAccountId { get; protected set; }
        public virtual Money Amount { get; protected set; }
        public virtual TransactionStateId TransactionStateId { get; protected set; }
        public virtual DateTime CreatedAt { get; protected set; }
        public virtual DateTime UpdatedAt { get; protected set; }


        protected Transaction()
        {
        }

        public Transaction(
            TransactionId transactionId,
            BankAccountId fromBankAccountId,
            BankAccountId toBankAccountId,
            Money amount,
            TransactionStateId transactionStateId,
            DateTime createdAt,
            DateTime updatedAt
            )
        {
            TransactionId = transactionId;
            FromBankAccountId = fromBankAccountId;
            ToBankAccountId = toBankAccountId;
            Amount = amount;
            TransactionStateId = transactionStateId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Transaction NonExisting()
        {
            DateTime Now = DateTime.Now;
            TransactionId transactionId = TransactionId.FromExisting(null);
            BankAccountId fromBankAccountId = BankAccountId.FromExisting(null);
            BankAccountId toBankAccountId = BankAccountId.FromExisting(null);
            return new Transaction(
                transactionId,
                fromBankAccountId,
                toBankAccountId,
                null,
                TransactionStateId.NULL,
                Now,
                Now);
        }

        public virtual bool DoesNotExist()
        {
            return TransactionId == null || !TransactionId.Ok();
        }

        public virtual bool Exist()
        {
            return TransactionId != null && TransactionId.Ok();
        }

        public virtual void Complete()
        {
            TransactionStateId = TransactionStateId.COMPLETED;
        }

        public virtual void Reject()
        {
            TransactionStateId = TransactionStateId.REJECTED;
        }

        public virtual void ChangeUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}