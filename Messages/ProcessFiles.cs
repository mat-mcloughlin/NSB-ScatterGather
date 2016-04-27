using System;
using System.Collections.Generic;
using NServiceBus;

namespace Messages
{
    public class ProcessFiles : IMessage
    {
        public string File { get; set; }
    }
}