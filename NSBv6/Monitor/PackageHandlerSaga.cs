using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Monitor
{
    public class PackageHandlerSaga : Saga<PackageHandlerData>,
        IAmStartedByMessages<NewZipFile>,
        IHandleMessages<FileUnzipped>,
        IHandleMessages<FilesProcessed>, 
        IHandleMessages<FileFailedToProcess>
    {
        static ILog log = LogManager.GetLogger<PackageHandlerSaga>();

        public Task Handle(NewZipFile message, IMessageHandlerContext context)
        {
            log.InfoFormat("New zip file Id: {0}.", message.FileId);
            return context.SendLocal(new UnzipFile
            {
                ExtractPath = Path.Combine(message.ExtractPath, message.FileId.ToString()),
                ZipPath = message.ZipPath
            });
        }

        public async Task Handle(FileUnzipped message, IMessageHandlerContext context)
        {
            Data.FilesToProcess = string.Join("|", message.Files);

            log.InfoFormat("Processing files.");
            foreach (var file in message.Files)
            {
                await context.Send(new ProcessFile { File = file });
            }
        }

        public Task Handle(FilesProcessed message, IMessageHandlerContext context)
        {
            var files = Data.FilesToProcess.Split('|').ToList();
            files.Remove(message.File);

            log.InfoFormat("{0} Left to process.", files.Count);
            Data.FilesToProcess = string.Join("|", files);

            return CheckComplete(context);
        }

        public Task Handle(FileFailedToProcess message, IMessageHandlerContext context)
        {
            // Add some additional business logic here like sending an email

            var files = Data.FilesToProcess.Split('|').ToList();
            files.Remove(message.File);

            log.InfoFormat("{0} Left to process.", files.Count);

            Data.FilesToProcess = string.Join("|", files);
            return CheckComplete(context);
        }

        private async Task CheckComplete(IMessageHandlerContext context)
        {
            if (!Data.FilesToProcess.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Any())
            {
                await context.SendLocal(new SendEmail());
                MarkAsComplete();
            }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PackageHandlerData> mapper)
        {
        }
    }
}
