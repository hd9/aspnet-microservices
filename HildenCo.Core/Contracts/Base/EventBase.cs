using System;
using System.Collections.Generic;
using System.Text;

namespace HildenCo.Core.Contracts.Base
{
    public abstract class EventBase : RequestBase
    {
        public DateTime CreatedOn => DateTime.UtcNow;
        public string SubmittedBy { get; set; }
        public string IP { get; set; }
    }
}
