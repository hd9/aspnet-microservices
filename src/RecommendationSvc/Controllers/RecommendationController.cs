using Microsoft.AspNetCore.Mvc;
using RecommendationSvc.Services;
using System.Threading.Tasks;

namespace RecommendationSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        readonly IRecommendationSvc _svc;
        const string help = @"The Recommendation service is alive! Try GET /api/v1/recommendations/{product-slug}";

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

        [Route("/api/v1/recommendations/{slug}")]
        public async Task<IActionResult> GetByProductSlug(string slug)
        {
            return Ok(await _svc.GetByProductSlug(slug));
        }

        [Route("/api/v1/recommendations/account/{accountId}")]
        public async Task<IActionResult> GetByAccountId(string accountId)
        {
            return Ok(await _svc.GetByAccountId(accountId));
        }
    }
}
