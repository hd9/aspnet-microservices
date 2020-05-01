using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSvc.Models
{
    public class Notification
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
