using Banking.Net.Command.Transactions.Application.Contracts;
using Banking.Net.Command.Transactions.Application.Dtos;
using Banking.Net.Transactions.Messages.Commands;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Banking.Net.Command.Transactions.Application.Services
{
    public class TransactionApplicationService : ITransactionApplicationService
    {
        private IMessageSession _messageSession;

        public TransactionApplicationService(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public void SetMessageSession(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task<PerformTransferResponseDto> PerformTransfer(PerformTransferRequestDto performTransferRequestDto)
        {
            try
            {
                var transactionId = Guid.NewGuid().ToString();
                var performTransfer = new StartTransfer(
                    transactionId,
                    performTransferRequestDto.FromBankAccountNumber,
                    performTransferRequestDto.ToBankAccountNumber,
                    performTransferRequestDto.Amount
                );
                await _messageSession.Send(performTransfer).ConfigureAwait(false);
                return new PerformTransferResponseDto
                {
                    Response = "OK"
                };
            }
            catch (Exception ex)
            {
                return new PerformTransferResponseDto
                {
                    Response = "ERROR: " + ex.Message + " -- " + ex.StackTrace
                };
            }
        }


        public async Task<PerformDepositResponseDto> PerformDeposit(PerformDepositRequestDto performDepositRequestDto)
        {
            try
            {
                var transactionId = Guid.NewGuid().ToString();
                var performTransfer = new StartTransfer(
                    transactionId,
                    performDepositRequestDto.FromBankAccountNumber,
                    "",
                    performDepositRequestDto.Amount
                );
                await _messageSession.Send(performTransfer).ConfigureAwait(false);
                return new PerformDepositResponseDto
                {
                    Response = "OK"
                };
            }
            catch (Exception ex)
            {
                return new PerformDepositResponseDto
                {
                    Response = "ERROR: " + ex.Message + " -- " + ex.StackTrace
                };
            }
        }






    }
}