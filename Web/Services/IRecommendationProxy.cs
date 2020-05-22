using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;
using Web.Models.Recommendation;

namespace Web.Services
{
    public interface IRecommendationProxy
    {
        Task<List<Recommendation>> GetByProductSlug(string slug);
        Task<List<Recommendation>> GetByAccountId(string accountId);
    }
}