using NServiceBus;

namespace Messages
{
    public class FileFailedToProcess : IMessage
    {
        public string File { get; set; }

    }
}
