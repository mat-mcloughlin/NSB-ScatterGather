using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Monitor
{
    class SendEmailHandler : IHandleMessages<SendEmail>
    {
        static ILog log = LogManager.GetLogger<SendEmailHandler>();

        public Task Handle(SendEmail message, IMessageHandlerContext context)
        {
            log.Info("Sending Email.");
            return Task.FromResult(0);
        }
    }
}
