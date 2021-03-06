﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Contracts.Notification
{
    public class SendMail
    {
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }
}
