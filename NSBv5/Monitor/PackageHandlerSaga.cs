using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

namespace Monitor
{
    public class PackageHandlerSaga : Saga<PackageHandlerData>,
        IAmStartedByMessages<NewZipFile>,
        IHandleMessages<FileUnzipped>,
        IHandleMessages<FilesProcessed>, 
        IHandleMessages<FileFailedToProcess>
    {
        static ILog log = LogManager.GetLogger<PackageHandlerSaga>();

        public void Handle(NewZipFile message)
        {
            log.InfoFormat("New zip file Id: {0}.", message.FileId);
            Bus.SendLocal(new UnzipFile
            {
                ExtractPath = Path.Combine(message.ExtractPath, message.FileId.ToString()),
                ZipPath = message.ZipPath
            });
        }

        public void Handle(FileUnzipped message)
        {
            Data.FilesToProcess = string.Join("|", message.Files);

            log.InfoFormat("Processing files.");
            foreach (var file in message.Files)
            {
                Bus.Send(new ProcessFile { File = file });
            }
        }

        public void Handle(FilesProcessed message)
        {
            var files = Data.FilesToProcess.Split('|').ToList();
            files.Remove(message.File);

            log.InfoFormat("{0} Left to process.", files.Count);
            Data.FilesToProcess = string.Join("|", files);

            CheckComplete();
        }

        public void Handle(FileFailedToProcess message)
        {
            // Add some additional business logic here like sending an email

            var files = Data.FilesToProcess.Split('|').ToList();
            files.Remove(message.File);

            log.InfoFormat("{0} Left to process.", files.Count);

            Data.FilesToProcess = string.Join("|", files);
            CheckComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PackageHandlerData> mapper)
        {
        }

        void CheckComplete()
        {
            if (!Data.FilesToProcess.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Any())
            {
                Bus.SendLocal(new SendEmail());
                MarkAsComplete();
            }
        }
    }
}
