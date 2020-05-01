using System.Threading.Tasks;

namespace NotificationSvc.Repositories
{
    public interface INotificationRepository
    {
        Task LogNotification(string name, string email, char type);
    }
}