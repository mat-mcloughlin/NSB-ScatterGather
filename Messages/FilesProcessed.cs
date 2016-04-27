using System;
using NServiceBus;

namespace Messages
{
    public class FilesProcessed : IMessage
    {
        public Guid BatchId { get; set; }
    }
}