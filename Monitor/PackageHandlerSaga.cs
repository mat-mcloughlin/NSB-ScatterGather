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
        IHandleMessages<FilesProcessed>
    {
        static ILog log = LogManager.GetLogger<PackageHandlerSaga>();
        
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PackageHandlerData> mapper)
        {
        }

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
            Data.FilesToProcess = message.Batches.Select(s => s.BatchId).ToList();

            log.InfoFormat("Processing batches.");
            foreach (var batch in message.Batches)
            {
                await context.Send(new ProcessFiles { BatchId = batch.BatchId, Batch = batch.Files });
            }
        }
        
        public Task Handle(FilesProcessed message, IMessageHandlerContext context)
        {
            Data.FilesToProcess.Remove(message.BatchId);
            log.InfoFormat("Files left to process: {0}.", Data.FilesToProcess.Count);

            if (!Data.FilesToProcess.Any())
            {
                context.SendLocal(new ContinueWithProcess());
                MarkAsComplete();
            }

            return Task.FromResult(0);
        }
    }
}
