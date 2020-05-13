﻿using Core.Models;
using Dapper;
using MassTransit;
using MySql.Data.MySqlClient;
using NotificationSvc.Infrastructure;
using NotificationSvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationSvc.Repositories
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly string _connStr;
        private readonly string insNotification = "INSERT INTO notification (name, email, created_at, type) values (@name, @email, sysdate(), @type)";
        private readonly string selNotification = "SELECT * FROM notification ORDER BY created_at DESC limit @limit";

        public NotificationRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task<List<Notification>> GetNotifications(int recs)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return (await conn.QueryAsync<Notification>(
                    selNotification,
                    new { limit = recs }
                )).ToList();
            }
        }

        public async Task Insert(string name, string email, char type)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                var cnt = await conn.ExecuteAsync(insNotification, new { name, email, type });
            }
        }
    }
}
