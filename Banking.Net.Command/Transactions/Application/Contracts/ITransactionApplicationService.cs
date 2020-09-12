using Banking.Net.Command.Transactions.Application.Dtos;
using NServiceBus;
using System.Threading.Tasks;

namespace Banking.Net.Command.Transactions.Application.Contracts
{
    public interface ITransactionApplicationService
    {
        void SetMessageSession(IMessageSession messageSession);
        Task<PerformTransferResponseDto> PerformTransfer(PerformTransferRequestDto dto);
        Task<PerformDepositResponseDto> PerformDeposit(PerformDepositRequestDto dto);
    }
}