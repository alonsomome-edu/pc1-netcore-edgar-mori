using Banking.Net.Transactions.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Banking.Net.Transactions.Handlers.Events
{
    public class TransferRejectedHandler : IHandleMessages<TransferRejected>
    {
        static readonly ILog log = LogManager.GetLogger<TransferRejectedHandler>();

        public Task Handle(TransferRejected transferRejected, IMessageHandlerContext context)
        {
            try
            {
                log.Info($"TransferRejectedHandler, TransactionId = {transferRejected.TransactionId}");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ** " + ex.StackTrace);
            }
            return Task.CompletedTask;
        }
    }
}