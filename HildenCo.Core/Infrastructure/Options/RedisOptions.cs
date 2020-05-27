using System;
using System.Collections.Generic;
using System.Text;

namespace HildenCo.Core.Infrastructure.Options
{
    public class RedisOptions
    {
        public string Configuration { get; set; }
        public string InstanceName { get; set; }

        /// <summary>
        /// To simplify logging
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"Configuration: {Configuration}, InstanceName: {InstanceName}";
    }
}
