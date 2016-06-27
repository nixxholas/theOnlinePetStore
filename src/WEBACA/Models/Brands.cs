using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBACA.Models
{
    public class Brands
    {
        public int BrandId { get; set; }
        public String BrandName { get; set; }
        public List<BrandCategory> BrandCategory { get; set; }
        public int? NoOfProducts { get; set; }
        //public List<Product> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public BrandPhoto BrandPhoto { get; set; }
    }
}
