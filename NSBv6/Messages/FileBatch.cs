using System;
using System.Collections.Generic;

namespace Messages
{
    public class Batch
    {
        public Guid BatchId { get; set; }
        public List<string> Files { get; set; }
    }
}