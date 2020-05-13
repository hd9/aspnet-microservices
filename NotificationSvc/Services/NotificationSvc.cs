using NotificationSvc.Models;
using NotificationSvc.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationSvc.Services
{
    public class NotificationSvc : INotificationSvc
    {
        private INotificationRepository _repo;

        public NotificationSvc(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Notification>> GetNotifications(int recs = 10)
        {
            return await _repo.GetNotifications(recs);
        }

        public async Task LogNotification(string name, string email, char type)
        {
            await _repo.Insert(name, email, type);
        }
    }
}
