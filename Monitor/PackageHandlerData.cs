using System;
using System.Collections.Generic;
using NServiceBus;

namespace Monitor
{
    public class PackageHandlerData : IContainSagaData
    {
        public Guid Id { get; set; }

        public string Originator { get; set; }

        public string OriginalMessageId { get; set; }

        public List<Guid> FilesToProcess { get; set; }
    }
}