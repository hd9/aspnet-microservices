using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Contracts.Base
{
    public abstract class EventBase : CommandBase
    {
        public DateTime CreatedOn => DateTime.UtcNow;
        public string SubmittedBy { get; set; }
        public string IP { get; set; }
    }
}
