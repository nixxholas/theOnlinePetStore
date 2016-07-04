using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class Metrics
    {
        // Primary key to identify the metric
        public int MetricId { get; set; }
        // Foreign key to identify which product the metrics is related to
        public int ProdId { get; set; }
        // Custom size/metric name/type
        // For example, kilograms, tonnes or even grams
        public String MetricName { get; set; }
        public int Quantity { get; set; }
        // Product that is binded to this metric system
        public Product Product { get; set; }
        // Price per metric
        public decimal Price { get; set; }
        public string CreatedById { get; set; }

        public string UpdatedById { get; set; }

        public string DeletedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser DeletedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        //Defining a property with a ? symbol after the DateTime datatype,
        //to tell the .NET engine's Entity Framework that, this is a Nullable property.
        public DateTime? DeletedAt { get; set; }
        // Pulls ALL of the Preset Metrics available for the
        // User to utilize
        public List<PresetMetric> PresetMetrics { get; set; }
    }
}
