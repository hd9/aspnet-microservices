using NotificationSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationSvc.Repositories
{
    public interface INotificationRepository
    {
        Task Insert(string name, string email, char type);
        Task<List<Notification>> GetNotifications(int recs);
    }
}