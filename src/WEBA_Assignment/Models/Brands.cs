﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WEBA_ASSIGNMENT.Models
{
    public class Brands
    {
        public int BrandId { get; set; }
        public String BrandName { get; set; }
        public List<BrandCategory> BrandCategory { get; set; }
        public List<BrandSpecials> BrandSpecials { get; set; }
        public int? NoOfProducts { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedById { get; set; }

        public string UpdatedById { get; set; }

        public string DeletedById { get; set; }
        [JsonIgnore]
        public ApplicationUser CreatedBy { get; set; }
        [JsonIgnore]
        public ApplicationUser UpdatedBy { get; set; }
        [JsonIgnore]
        public ApplicationUser DeletedBy { get; set; }
        public BrandPhoto BrandPhoto { get; set; }
    }
}
