using Newtonsoft.Json;
using System;

namespace WEBA_ASSIGNMENT.Models
{
    public class Price
    {
        public int PriceId { get; set; }
        public int MetricId { get; set; }
        [JsonIgnore]
        public Metrics Metric { get; set; }
        public decimal Value { get; set; }
        // This must be nullable incase there is an in-house brand
        public decimal? RRP { get; set; }
        public DateTime CreatedAt { get; set; }        
        //Defining a property with a ? symbol after the DateTime datatype,
        //to tell the .NET engine's Entity Framework that, this is a Nullable property.
        public DateTime? DeletedAt { get; set; }
        public string CreatedById { get; set; }
        public string DeletedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser DeletedBy { get; set; }
    }
}
