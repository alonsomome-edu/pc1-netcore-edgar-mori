using Banking.Net.Accounts.Messages.Commands;
using Banking.Net.Accounts.Messages.Events;
using Banking.Net.Transactions.Messages.Commands;
using Banking.Net.Transactions.Messages.Events;
using Banking.Net.Transactions.Messages.Sagas;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Banking.Net.Transactions.Handlers.Sagas
{
    public class TransferSaga :
        Saga<TransferSagaData>,
        IAmStartedByMessages<TransferStarted>,
        IHandleMessages<MoneyWithdrawn>,
        IHandleMessages<WithdrawRejected>,
        IHandleMessages<MoneyDeposited>,
        IHandleMessages<DepositRejected>,
        IHandleMessages<FromBankAccountNotFound>,
        IHandleMessages<ToBankAccountNotFound>
    {
        static readonly ILog log = LogManager.GetLogger<TransferSaga>();
        private readonly ILogger<TransferSaga> _logger;

        public TransferSaga(ILogger<TransferSaga> logger)
            => _logger = logger;

        public async Task Handle(TransferStarted transferStarted, IMessageHandlerContext context)
        {
            try 
            {
                _logger.LogInformation($"Saga TransferStarted, TransferId = {transferStarted.TransactionId}");
                Data.TransactionId = transferStarted.TransactionId;
                Data.FromBankAccountId = transferStarted.FromBankAccountId;
                Data.ToBankAccountId = transferStarted.ToBankAccountId;
                Data.Amount = transferStarted.Amount;
                var withdraw = new WithdrawMoney(
                    Data.TransactionId,
                    Data.FromBankAccountId,
                    Data.Amount
                );
                await context.Send(withdraw).ConfigureAwait(false);
            } 
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }

        public async Task Handle(FromBankAccountNotFound fromBankAccountNotFound, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"Saga FromBankAccountNotFound, TransactionId = {fromBankAccountNotFound.TransactionId}");
                var rejectPerformTransfer = new RejectTransfer(
                    Data.TransactionId
                );
                await context.SendLocal(rejectPerformTransfer).ConfigureAwait(false);
                MarkAsComplete();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }

        public async Task Handle(MoneyWithdrawn moneyWithdrawn, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"Saga MoneyWithdrawn, TransactionId = {moneyWithdrawn.TransactionId}");
                var deposit = new DepositMoney(
                    Data.TransactionId,
                    Data.ToBankAccountId,
                    Data.Amount
                );
                await context.Send(deposit).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }

        public async Task Handle(WithdrawRejected withdrawRejected, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"Saga WithdrawRejected, TransactionId = {withdrawRejected.TransactionId}");
                var rejectPerformTransfer = new RejectTransfer(
                    Data.TransactionId
                );
                await context.SendLocal(rejectPerformTransfer).ConfigureAwait(false);
                MarkAsComplete();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }

        public async Task Handle(ToBankAccountNotFound toBankAccountNotFound, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"Saga ToBankAccountNotFound, TransactionId = {toBankAccountNotFound.TransactionId}");
                var refundMoney = new RefundMoney(
                    Data.FromBankAccountId,
                    Data.Amount
                );
                await context.Send(refundMoney).ConfigureAwait(false);
                var rejectTransfer = new RejectTransfer(
                    Data.TransactionId
                );
                await context.SendLocal(rejectTransfer).ConfigureAwait(false);
                MarkAsComplete();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }

        public async Task Handle(MoneyDeposited moneyDeposited, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"Saga MoneyDeposited, TransactionId = {moneyDeposited.TransactionId}");
                var completeTransfer = new CompleteTransfer(
                    Data.TransactionId
                );
                await context.SendLocal(completeTransfer).ConfigureAwait(false);
                MarkAsComplete();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }

        public async Task Handle(DepositRejected depositRejected, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"Saga DepositRejected, TransactionId = {depositRejected.TransactionId}");
                var rejectTransfer = new RejectTransfer(
                    Data.TransactionId
                );
                await context.SendLocal(rejectTransfer).ConfigureAwait(false);
                MarkAsComplete();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TransferSagaData> mapper)
        {
            mapper.ConfigureMapping<TransferStarted>(message => message.TransactionId)
                .ToSaga(sagaData => sagaData.TransactionId);

            mapper.ConfigureMapping<MoneyWithdrawn>(message => message.TransactionId)
                .ToSaga(sagaData => sagaData.TransactionId);

            mapper.ConfigureMapping<WithdrawRejected>(message => message.TransactionId)
                .ToSaga(sagaData => sagaData.TransactionId);

            mapper.ConfigureMapping<MoneyDeposited>(message => message.TransactionId)
                .ToSaga(sagaData => sagaData.TransactionId);

            mapper.ConfigureMapping<DepositRejected>(message => message.TransactionId)
                .ToSaga(sagaData => sagaData.TransactionId);

            mapper.ConfigureMapping<FromBankAccountNotFound>(message => message.TransactionId)
                .ToSaga(sagaData => sagaData.TransactionId);

            mapper.ConfigureMapping<ToBankAccountNotFound>(message => message.TransactionId)
                .ToSaga(sagaData => sagaData.TransactionId);
        }
    }
}