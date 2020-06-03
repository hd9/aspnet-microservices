using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Contracts.Base
{
    public abstract class CommandBase
    {
        public string RequestId { get; set; }
    }
}
