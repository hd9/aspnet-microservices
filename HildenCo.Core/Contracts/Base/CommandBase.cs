using System;
using System.Collections.Generic;
using System.Text;

namespace HildenCo.Core.Contracts.Base
{
    public abstract class CommandBase
    {
        public string RequestId { get; set; }
    }
}
