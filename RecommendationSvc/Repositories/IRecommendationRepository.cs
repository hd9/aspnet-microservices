using Core.Commands.Catalog;
using RecommendationSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecommendationSvc.Repositories
{
    public interface IRecommendationRepository
    {
        Task<List<Recommendation>> GetByProductSlug(string slug);
        Task<List<Recommendation>> GetByAccountId(string accountId);
        Task InsertProducts(List<ProductInfo> productInfos);
        Task InsertRecommendations(List<RecommendationDto> recomms);
    }
}