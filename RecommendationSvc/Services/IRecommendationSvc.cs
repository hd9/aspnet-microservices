using System.Collections.Generic;
using System.Threading.Tasks;
using RecommendationSvc.Models;

namespace RecommendationSvc.Services
{
    public interface IRecommendationSvc
    {
        Task<int> Create(Recommendation recomm);
        Task<Recommendation> GetByProductId(string productId);
        Task<Recommendation> GetByAccountId(string accountId);
    }
}