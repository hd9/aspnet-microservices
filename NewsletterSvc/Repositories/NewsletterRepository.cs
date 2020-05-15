using Dapper;
using MySql.Data.MySqlClient;
using NewsletterSvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsletterSvc.Repositories
{
    public class NewsletterRepository : INewsletterRepository
    {
        private readonly string _connStr;
        private readonly string insSignup = "INSERT INTO newsletter (name, email, created_at) values (@name, @email, sysdate())";
        private readonly string selSignup = "SELECT * FROM newsletter ORDER BY created_at DESC limit @limit";

        public NewsletterRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task Insert(Signup s)
        {
            if (s == null)
                return;

            using (var conn = new MySqlConnection(_connStr))
            {
                var cnt = await conn.ExecuteAsync(insSignup, new { s.Name, s.Email });
            }
        }

        public async Task<List<Signup>> GetSignups(int recs)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return (await conn.QueryAsync<Signup>(
                    selSignup,
                    new { limit = recs }
                )).ToList();
            }
        }
    }
}
