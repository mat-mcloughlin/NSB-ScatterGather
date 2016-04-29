using System;
using NServiceBus;

namespace Messages
{
    public class NewZipFile : IMessage
    {
        public Guid FileId { get; set; }
        public string ZipPath { get; set; }
        public string ExtractPath { get; set; }
    }
}