using Banking.Net.Command.Accounts.Domain.Entities;
using Banking.Net.Common.Domain.Entities;
using System;

namespace Banking.Net.Command.Transactions.Domain.Services
{
    public class TransferDomainService
    {
        public void PerformTransfer(BankAccount fromAccount, BankAccount toAccount, decimal amount)
        {
            Notification notification = TransferValidation(fromAccount, toAccount, amount);
            if (notification.HasErrors())
            {
                throw new ArgumentException(notification.ErrorMessage());
            }
            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);
        }

        public virtual Notification TransferValidation(BankAccount fromAccount, BankAccount toAccount, decimal amount)
        {
            Notification notification = new Notification();
            ValidateAmount(notification, amount);
            ValidateBankAcounts(notification, fromAccount, toAccount);
            return notification;
        }

        private void ValidateAmount(Notification notification, decimal amount)
        {
            if (amount <= 0)
            {
                notification.AddError("The amount must be greater than zero");
            }
        }

        private void ValidateBankAcounts(Notification notification, BankAccount fromAccount, BankAccount toAccount)
        {
            if (fromAccount == null)
            {
                notification.AddError("Invalid From Account");
            }
            if (toAccount == null)
            {
                notification.AddError("Invalid To Account");
            }
            if (fromAccount != null && toAccount != null && fromAccount.BankAccountNumber.Number.Equals(toAccount.BankAccountNumber.Number))
            {
                notification.AddError("Cannot transfer money to the same bank account");
            }
        }
    }
}