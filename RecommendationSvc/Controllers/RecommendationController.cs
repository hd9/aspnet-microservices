using Microsoft.AspNetCore.Mvc;
using RecommendationSvc.Models;
using RecommendationSvc.Services;
using System.Threading.Tasks;

namespace RecommendationSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationSvc _svc;
        const string help = @"The service is alive!";

        public RecommendationController(IRecommendationSvc svc)
        {
            _svc = svc;
        }

        [Route("/ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [Route("/help")]
        public IActionResult Help()
        {
            return Ok(help);
        }

        [Route("/recommendations/search")]
        public async Task<Recommendation> GetByProductId(string productId)
        {
            return await _svc.GetByProductId(productId);
        }

        [Route("/payments/search")]
        public async Task<Recommendation> GetByAccountId(string accountId)
        {
            return await _svc.GetByAccountId(accountId);
        }
    }
}
