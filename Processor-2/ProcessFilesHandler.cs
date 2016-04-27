using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Processor_2
{
    class ProcessFilesHandler : IHandleMessages<ProcessFile>
    {
        static ILog log = LogManager.GetLogger<ProcessFilesHandler>();
        static Random rand = new Random();

        public async Task Handle(ProcessFile message, IMessageHandlerContext context)
        {
            log.InfoFormat("Handling File : {0}", message.File);

            if (!ValidatePostcodes(message.File))
            {
                throw new Exception("Postcode is invalid.");
                //await context.Reply(new FileFailedToProcess { File = message.File });
            }
            else
            {
                log.InfoFormat("Handling File : {0}", message.File);
                await context.Reply(new FilesProcessed { File = message.File });
            }
        }

        bool ValidatePostcodes(string fileLocation)
        {
            var regex = new Regex("^(GIR 0AA|[A-PR-UWYZ]([0-9]{1,2}|([A-HK-Y][0-9]|[A-HK-Y][0-9]([0-9]|[ABEHMNPRV-Y]))|[0-9][A-HJKPS-UW]) {0,1}[0-9][ABD-HJLNP-UW-Z]{2})$");
            var lines = File.ReadAllLines(fileLocation).Select(a => a.Split(','));
            var postcodes = lines.Select(l => l.First().Replace("\"", "").Replace(" ", ""));

            return postcodes.All(postcode => regex.IsMatch(postcode));
        }
    }
}