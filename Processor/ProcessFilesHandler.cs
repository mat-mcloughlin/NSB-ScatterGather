using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Monitor
{
    class ProcessFilesHandler : IHandleMessages<ProcessFiles>
    {
        static ILog log = LogManager.GetLogger<ProcessFilesHandler>();
        static Random rand = new Random();


        public async Task Handle(ProcessFiles message, IMessageHandlerContext context)
        {
            var sleep = rand.Next(0, 5000);
           await Task.Delay(sleep);

            log.InfoFormat("Handling Batch : {0}, slept for {1}", message.BatchId, sleep);
            await context.Reply(new FilesProcessed { BatchId = message.BatchId});
        }
    }
}
