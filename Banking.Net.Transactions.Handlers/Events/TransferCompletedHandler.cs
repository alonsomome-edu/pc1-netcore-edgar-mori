using Banking.Net.Transactions.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Banking.Net.Transactions.Handlers.Events
{
    public class TransferCompletedHandler : IHandleMessages<TransferCompleted>
    {
        static readonly ILog log = LogManager.GetLogger<TransferCompletedHandler>();

        public Task Handle(TransferCompleted transferCompleted, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"TransferCompletedHandler, TransactionId = {transferCompleted.TransactionId}");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
            return Task.CompletedTask;
        }
    }
}