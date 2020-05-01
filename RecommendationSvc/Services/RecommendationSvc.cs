using RecommendationSvc.Models;
using RecommendationSvc.Repositories;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSvc.Services
{
    public class RecommendationSvc : IRecommendationSvc
    {

        private readonly IRecommendationRepository _repo;

        public RecommendationSvc(IRecommendationRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Create(Recommendation recomm)
        {
            return await _repo.Insert(recomm);
        }

        public async Task<Recommendation> GetByProductId(string productId)
        {
            return await _repo.GetByProductId(productId);
        }

        public async Task<Recommendation> GetByAccountId(string accountId)
        {
            return await _repo.GetByAccountId(accountId);
        }
    }
}
