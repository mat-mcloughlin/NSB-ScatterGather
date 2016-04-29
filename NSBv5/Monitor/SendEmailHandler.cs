using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Monitor
{
    class SendEmailHandler : IHandleMessages<SendEmail>
    {
        static ILog log = LogManager.GetLogger<SendEmailHandler>();

        public void Handle(SendEmail message)
        {
            log.Info("Sending Email.");
        }
    }
}
