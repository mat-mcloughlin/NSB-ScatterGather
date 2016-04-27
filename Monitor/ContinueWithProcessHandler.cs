using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Monitor
{
    class ContinueWithProcessHandler : IHandleMessages<ContinueWithProcess>
    {
        static ILog log = LogManager.GetLogger<ContinueWithProcessHandler>();

        public Task Handle(ContinueWithProcess message, IMessageHandlerContext context)
        {
            log.Info("Continue with the process.");
            return Task.FromResult(0);
        }
    }
}
