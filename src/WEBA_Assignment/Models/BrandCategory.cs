using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class BrandCategory
    {
        public int CatId { get; set; }
        public int BrandId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Category Category { get; set; }
        public Brands Brand { get; set; }
    }
}
