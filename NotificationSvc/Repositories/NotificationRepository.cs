using Core.Models;
using Dapper;
using MassTransit;
using MySql.Data.MySqlClient;
using NotificationSvc.Infrastructure;
using NotificationSvc.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationSvc.Repositories
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly string connStr;
        private readonly string insert = "INSERT INTO notifications (name, email, created_at, type) values (@name, @email, sysdate(), @type)";

        public NotificationRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            this.connStr = connStr;
        }

        public async Task LogNotification(string name, string email, char type)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                var cnt = await conn.ExecuteAsync(insert, new { name, email, type });
            }
        }
    }
}
