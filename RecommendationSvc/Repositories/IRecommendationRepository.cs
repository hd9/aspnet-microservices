using RecommendationSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecommendationSvc.Repositories
{
    public interface IRecommendationRepository
    {
        Task<int> Insert(Recommendation recomm);
        Task<List<Recommendation>> GetByProductSlug(string slug);
        Task<List<Recommendation>> GetByAccountId(string accountId);
    }
}