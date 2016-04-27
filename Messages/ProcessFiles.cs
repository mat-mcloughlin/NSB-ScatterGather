using System;
using System.Collections.Generic;
using NServiceBus;

namespace Messages
{
    public class ProcessFile : IMessage
    {
        public string File { get; set; }
    }
}