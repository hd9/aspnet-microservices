using Core.Models;
using Dapper;
using MassTransit;
using MySql.Data.MySqlClient;
using NotificationSvc.Infrastructure;
using NotificationSvc.Models;
using NotificationSvc.Repositories;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationSvc.Services
{
    public class NotificationSvc
    {
        private INotificationRepository _repo;

        public NotificationSvc(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task LogNotification(string name, string email, char type)
        {
            await _repo.LogNotification(name, email, type);
        }
    }
}
