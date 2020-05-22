using Microsoft.AspNetCore.Mvc;
using RecommendationSvc.Services;
using System.Threading.Tasks;

namespace RecommendationSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationSvc _svc;
        const string help = @"The Recommendation service is alive! Try GET /recommendations/{product-slug}";

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

        [Route("/recommendations/{slug}")]
        public async Task<IActionResult> GetByProductSlug(string slug)
        {
            return Ok(await _svc.GetByProductSlug(slug));
        }

        [Route("/recommendations/account/{accountId}")]
        public async Task<IActionResult> GetByAccountId(string accountId)
        {
            return Ok(await _svc.GetByAccountId(accountId));
        }
    }
}
