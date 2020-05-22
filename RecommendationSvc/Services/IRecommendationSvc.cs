using HildenCo.Core.Contracts.Orders;
using RecommendationSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecommendationSvc.Services
{
    public interface IRecommendationSvc
    {
        Task<List<Recommendation>> GetByProductSlug(string slug);
        Task<List<Recommendation>> GetByAccountId(string accountId);
        Task BuildRecommendation(OrderSubmitted message);
    }
}