using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSvc.Models
{
    /// <summary>
    /// Defines a simple recommendation dto to insert into the recommendation table
    /// </summary>
    public class RecommendationDto
    {
        public string ProductSlug { get; set; }
        public string RelatedSlug { get; set; }
        public int Hits { get; set; }
        public DateTime LastUpdate => DateTime.UtcNow;
    }
}
