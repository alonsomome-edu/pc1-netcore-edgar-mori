using Banking.Net.Command.Transactions.Application.Contracts;
using Banking.Net.Command.Transactions.Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Banking.API.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "Transactions")]
    [Route("transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionApplicationService _transactionApplicationService;

        public TransactionController(ITransactionApplicationService transactionApplicationService)
        {
            _transactionApplicationService = transactionApplicationService;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> PerformTransfer([FromBody] PerformTransferRequestDto performTransferRequestDto)
        {
            try
            {
                PerformTransferResponseDto response = await _transactionApplicationService.PerformTransfer(performTransferRequestDto);
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> DepositMoney([FromBody] PerformDepositRequestDto performDepositRequestDto)
        {
            try
            {
                PerformDepositResponseDto response = await _transactionApplicationService.PerformDeposit(performDepositRequestDto);
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}