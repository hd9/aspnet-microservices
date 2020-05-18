using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly IRecommendationProxy _svc;

        public RecommendationController(IRecommendationProxy svc)
        {
            _svc = svc;
        }


        /// <summary>
        /// Provides the list of recommendations based on product slug
        /// </summary>
        [Route("/api/recommendations/{slug}")]
        public async Task<IActionResult> GetRecommendations(string slug)
        {
            return Ok(await _svc.GetByProductSlug(slug));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
