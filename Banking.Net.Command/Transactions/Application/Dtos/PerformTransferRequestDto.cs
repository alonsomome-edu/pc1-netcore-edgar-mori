namespace Banking.Net.Command.Transactions.Application.Dtos
{
    public class PerformTransferRequestDto
    {
        public string FromBankAccountNumber { get; set; }
        public string ToBankAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}