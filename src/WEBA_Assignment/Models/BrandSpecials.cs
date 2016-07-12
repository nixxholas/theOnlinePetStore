using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class BrandSpecials
    {
        public int BrandId { get; set; }
        public int SpecialId { get; set; }
        public Brands Brands { get; set; }
        public Specials Specials { get; set; }
    }
}
