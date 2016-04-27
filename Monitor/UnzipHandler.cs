using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Monitor
{
    class UnzipHandler : IHandleMessages<UnzipFile>
    {
        static ILog log = LogManager.GetLogger<UnzipHandler>();
        
        public Task Handle(UnzipFile message, IMessageHandlerContext context)
        {
            log.InfoFormat("Unzipping File.");

            CreateDirectory(message.ExtractPath);
            ZipFile.ExtractToDirectory(message.ZipPath, message.ExtractPath);

            var files = GetFiles(message.ExtractPath);
            
            log.InfoFormat("{0} files unzipped.", files.Count);
            
            return context.Reply(new FileUnzipped { Files = files });
        }

        private List<string> GetFiles(string extractPath)
        {
            return Directory.GetFiles(Path.Combine(extractPath, @"Data\Csv")).ToList();
        }

        private static void CreateDirectory(string extractPath)
        {
            if (!Directory.Exists(extractPath))
            {
                Directory.Exists(extractPath);
            }
        }
    }
}
