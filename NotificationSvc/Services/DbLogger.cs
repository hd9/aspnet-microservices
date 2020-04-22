using Core.Shared;
using Dapper;
using MassTransit;
using MySql.Data.MySqlClient;
using NotificationSvc.Infrastructure;
using NotificationSvc.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationSvc.Services
{
    public class DbLogger
    {

        private readonly string connStr;
        private readonly string insert = "INSERT INTO notifications (name, email, created_at, type) values (@name, @email, sysdate(), @type)";

        public DbLogger(string connStr)
        {
            this.connStr = connStr;
        }

        /// <summary>
        /// Logs a newsletter event int the database.
        /// </summary>
        /// <param name="name">Name of the user</param>
        /// <param name="email">Email address</param>
        /// <param name="type">[E]mail or [S]MS</param>
        /// <returns></returns>
        public async Task Log(string name, string email, char type)
        {
            // todo :: get logger
            using (var conn = new MySqlConnection(connStr))
            {
                // todo :: log - Console.Write(" [x] Inserting record: ");
                var cnt = await conn.ExecuteAsync(insert, new { name, email, type });
                // todo :: log - Console.WriteLine("[ OK ]");
            }
        }
    }
}
