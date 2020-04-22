using Core.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSvc.Infrastructure.Options
{
    public class MailOptions : SmtpOptions
    {
        public string FromName { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
    }
}
