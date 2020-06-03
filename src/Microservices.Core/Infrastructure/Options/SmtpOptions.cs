using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Infrastructure.Options
{
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int  Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        /// <summary>
        /// If set, overrides the original email.
        /// Use for testing purposes.
        /// Please, don't spam others!
        /// </summary>
        public string EmailOverride { get; set; }
    }
}
