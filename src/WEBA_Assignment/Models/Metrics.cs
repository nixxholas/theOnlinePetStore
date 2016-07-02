using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class Metrics
    {
        // Primary key to identity the metric
        public int MetricId { get; set; }
        // For example, kilograms, tonnes or even grams
        public String MetricName { get; set; }
        // Store the data of the type into a list. i.e. KG
        // We need a list in case it's sizes, where there are
        // 3 different sizes like Small, Medium and Large
        public List<String> MetricData { get; set; }
        // Products that have this metric system
        public List<Product> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
