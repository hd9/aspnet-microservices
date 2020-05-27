using Dapper;
using HildenCo.Core.Contracts.Catalog;
using MySql.Data.MySqlClient;
using RecommendationSvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSvc.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        readonly string _connStr;
        readonly string selBySlug = "select p.slug, p.name, p.description from product p join recommendation r on r.related_slug = p.slug where r.product_slug = @slug order by hits desc";
        readonly string insProduct = "insert into product values (@slug, @name, @description, @price, sysdate(), sysdate()) on duplicate key update name = @name, description = @description, price = @price, last_update = sysdate();";
        readonly string insRecomm = "insert into recommendation (product_slug, related_slug, last_update) values (@product_slug, @related_slug, sysdate()) on duplicate key update hits = hits + 1, last_update = sysdate();";

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

        public async Task InsertProducts(List<ProductInfo> productInfos)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.OpenAsync();
                using (var trans = await conn.BeginTransactionAsync())
                {
                    foreach(var pi in productInfos)
                    {
                        await conn.ExecuteAsync(insProduct, new
                        {
                            slug = pi.Slug,
                            name = pi.Name,
                            description = pi.Description,
                            price = pi.Price
                        });
                    }

                    await trans.CommitAsync();
                }
            }
        }

        public async Task InsertRecommendations(List<RecommendationDto> recomms)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                await conn.OpenAsync();
                using (var trans = await conn.BeginTransactionAsync())
                {
                    foreach(var r in recomms)
                    {
                        await conn.ExecuteAsync(insRecomm, new
                        {
                            product_slug = r.ProductSlug,
                            related_slug = r.RelatedSlug
                        });
                    }

                    await trans.CommitAsync();
                }
            }
        }
    }
}
