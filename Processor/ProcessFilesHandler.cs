using System;
using System.IO;
using System.Linq;
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

            //ValidatePostcodes(message.File);

            log.InfoFormat("Handling File : {0}, slept for {1}", message.File, sleep);
            await context.Reply(new FilesProcessed { File = message.File });
        }

        void ValidatePostcodes(string fileLocation)
        {
            var lines = File.ReadAllLines(fileLocation).Select(a => a.Split('\n'));

        }
    }
}


//(GIR 0AA)|((([A-Z-[QVX]][0-9][0-9]?)|(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|(([A-Z-[QVX]][0-9][A-HJKPSTUW])|([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY])))) [0-9][A-Z-[CIKMOV]]{2})