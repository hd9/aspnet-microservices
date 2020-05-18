using RecommendationSvc.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSvc.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly string _connStr;
        private readonly string selBySlug = "select p.slug, p.name, p.description from product p join recommendation r on r.related_id = p.id where r.product_id = (select id from product where slug = @slug) order by hits desc;";

        public RecommendationRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public Task<List<Recommendation>> GetByAccountId(string accountId)
        {
            // not implemented 
            return Task.FromResult(new List<Recommendation>());
        }

        public async Task<List<Recommendation>> GetByProductSlug(string slug)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                return (
                    await conn.QueryAsync<Recommendation>(
                        selBySlug, 
                        new { slug })
                ).ToList();
            }
        }

        public async Task<int> Insert(Recommendation recomm)
        {
            throw new NotImplementedException();
            // todo :: insert
            // todo :: log
        }

    }
}
