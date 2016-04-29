using System;
using NServiceBus.Saga;

namespace Monitor
{
    public class PackageHandlerData : IContainSagaData
    {
        public virtual Guid Id { get; set; }

        public virtual string Originator { get; set; }

        public virtual string OriginalMessageId { get; set; }

        public virtual string FilesToProcess { get; set; }
    }
}