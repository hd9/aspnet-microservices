using Microservices.Core.Contracts.Catalog;
using Microservices.Core.Contracts.Orders;
using Microservices.Core.Infrastructure.Extensions;
using MassTransit;
using RecommendationSvc.Models;
using RecommendationSvc.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSvc.Services
{
    public class RecommendationSvc : IRecommendationSvc
    {

        readonly IRecommendationRepository _repo;
        readonly IRequestClient<ProductInfoRequest> _client;

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

                await _repo.InsertProducts(productInfos);

                // build recoms
                var recomms = new List<RecommendationDto>();
                productInfos.ForEach(pi =>
                {
                    var relatedProducts = productInfos.Where(pi2 => pi2.Slug != pi.Slug).ToList();
                    relatedProducts.ForEach(rp => {
                        recomms.Add(
                            new RecommendationDto { 
                                ProductSlug = pi.Slug, 
                                RelatedSlug = rp.Slug, 
                                Hits = 1 
                            });
                    });
                });

                if (!recomms.HasAny())
                    return;

                await _repo.InsertRecommendations(recomms);
            }
        }
    }
}
