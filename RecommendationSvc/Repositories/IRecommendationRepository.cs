using RecommendationSvc.Models;
using System.Threading.Tasks;

namespace RecommendationSvc.Repositories
{
    public interface IRecommendationRepository
    {
        Task<int> Insert(Recommendation recomm);
        Task<Recommendation> GetByProductId(string productId);
        Task<Recommendation> GetByAccountId(string accountId);
    }
}