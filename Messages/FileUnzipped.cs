using System.Collections.Generic;
using NServiceBus;

namespace Messages
{
    public class FileUnzipped : IMessage
    {
        public List<Batch> Batches { get; set; }
    }
}
