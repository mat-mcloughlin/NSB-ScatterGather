using System;
using NServiceBus;

namespace Messages
{
    public class FilesProcessed : IMessage
    {
        public string File { get; set; }
    }
}