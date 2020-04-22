using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Options
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string Db { get; set; }
        public string Collection { get; set; }
    }
}
