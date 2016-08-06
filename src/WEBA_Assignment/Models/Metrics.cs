using Newtonsoft.Json;
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
        // Link Metrics with a Preset if Needed
        public int? PMetricId { get; set; }
        public PresetMetric PresetMetric { get; set; }
        // Custom Amount per Metric Type
        public int MetricAmount { get; set; }
        // Custom size/metric name/type
        // For example, kilograms, tonnes or even grams
        public String MetricType { get; set; }
        public int Quantity { get; set; }
        // Each metric has it's own status
        public int StatusId { get; set; }
        public Status Status { get; set; }
        // Product that is binded to this metric system
        [JsonIgnore]
        public Product Product { get; set; }
        public int PriceId { get; set; }
        // Price per metric
        public Price Price { get; set; }
        public string CreatedById { get; set; }
        public string DeletedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser DeletedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        //Defining a property with a ? symbol after the DateTime datatype,
        //to tell the .NET engine's Entity Framework that, this is a Nullable property.
        public DateTime? DeletedAt { get; set; }
        // Pulls ALL of the Preset Metrics available for the
        // User to utilize
    }
}
