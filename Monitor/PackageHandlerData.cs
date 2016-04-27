using System;
using System.Collections.Generic;
using NServiceBus;

namespace Monitor
{
    public class PackageHandlerData : IContainSagaData
    {
        public PackageHandlerData()
        {
            FilesToProcess = new List<FileToProcess>();
        }

        public virtual Guid Id { get; set; }

        public virtual string Originator { get; set; }

        public virtual string OriginalMessageId { get; set; }

        public virtual IList<FileToProcess> FilesToProcess { get; set; }
    }

    public class FileToProcess
    {
        public FileToProcess()
        {
            Id = Guid.NewGuid();
        }

        public virtual Guid Id { get; set; }

        public virtual string FileLocation { get; set; }

        public virtual PackageHandlerData PackageHandlerData { get; set; }
    }
}