using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Commands
{
    public class SendMail
    {
        public string ToName { get; set; }
        public string FromName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
