using System;
using System.Collections.Generic;
using NServiceBus;

namespace Messages
{
    public class ProcessFiles : IMessage
    {
        public List<string> Batch { get; set; }
        public Guid BatchId { get; set; }
    }
}