using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Net.Command.Transactions.Application.Dtos
{
    class PerformDepositRequestDto
    {
        public string FromBankAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
