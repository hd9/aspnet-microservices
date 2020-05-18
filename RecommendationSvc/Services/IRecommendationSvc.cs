using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Events.Orders;
using RecommendationSvc.Models;

namespace RecommendationSvc.Services
{
    public interface IRecommendationSvc
    {
        Task<List<Recommendation>> GetByProductSlug(string slug);
        Task<List<Recommendation>> GetByAccountId(string accountId);
        Task BuildRecommendation(OrderSubmitted message);
    }
}