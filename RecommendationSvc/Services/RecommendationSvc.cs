using RecommendationSvc.Models;
using RecommendationSvc.Repositories;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Events.Orders;
using MassTransit;

namespace RecommendationSvc.Services
{
    public class RecommendationSvc : IRecommendationSvc
    {

        private readonly IRecommendationRepository _repo;
        private readonly IBusControl _bus;

        public RecommendationSvc(IRecommendationRepository repo, IBusControl bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task<List<Recommendation>> GetByProductSlug(string slug)
        {
            return await _repo.GetByProductSlug(slug);
        }

        public async Task<List<Recommendation>> GetByAccountId(string accountId)
        {
            return await _repo.GetByAccountId(accountId);
        }

        public Task BuildRecommendation(OrderSubmitted message)
        {
            // todo :: build recommendation
            // todo :: from productId, get List<products> async then insert into recomm table

            return Task.FromResult(0);
        }
    }
}
