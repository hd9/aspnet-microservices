using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Infrastructure.Options
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string Db { get; set; }
        public string Collection { get; set; }

        /// <summary>
        /// Simplify logging
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"ConnStr: {ConnectionString}, Db: {Db}, Collection: {Collection}";
    }
}
