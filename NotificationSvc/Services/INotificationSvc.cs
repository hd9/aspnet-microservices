using NotificationSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationSvc.Services
{
    public interface INotificationSvc
    {
        Task LogNotification(string name, string email, char type);
        Task<List<Notification>> GetNotifications(int recs = 10);
    }
}   