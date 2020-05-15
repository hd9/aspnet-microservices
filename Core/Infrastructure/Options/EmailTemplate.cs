using Core.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsletterSvc.Infrastructure.Options
{
    public class EmailTemplate
    {
        /// <summary>
        /// The name of the template (optional).
        /// Use only when more than one template is set in the config file.
        /// </summary>
        public string TemplateName { get; set; }
        public string FromName { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
