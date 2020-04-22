using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Shared
{
    public abstract class EventBase : RequestBase
    {
        public DateTime CreatedOn => DateTime.UtcNow;
    }
}
