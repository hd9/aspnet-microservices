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
using Core.Commands.Catalog;
using Core.Infrastructure.Extentions;

namespace RecommendationSvc.Services
{
    public class RecommendationSvc : IRecommendationSvc
    {

        private readonly IRecommendationRepository _repo;
        private readonly IRequestClient<ProductInfoRequest> _client;

        public RecommendationSvc(IRecommendationRepository repo, IRequestClient<ProductInfoRequest> client)
        {
            _repo = repo;
            _client = client;
        }

        public async Task<List<Recommendation>> GetByProductSlug(string slug)
        {
            return await _repo.GetByProductSlug(slug);
        }

        public async Task<List<Recommendation>> GetByAccountId(string accountId)
        {
            return await _repo.GetByAccountId(accountId);
        }

        public async Task BuildRecommendation(OrderSubmitted message)
        {
            using (var request = _client.Create(new ProductInfoRequest { Slugs = message.Slugs }))
            {
                var response = await request.GetResponse<ProductInfoResponse>();
                var productInfos = response.Message.ProductInfos;

                if (!productInfos.HasAny())
                    return;

                // todo :: build recommendation
            }
        }
    }
}
