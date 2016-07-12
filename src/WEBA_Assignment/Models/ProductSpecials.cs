using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class ProductSpecials
    {
        public int ProdId { get; set; }
        public int SpecialId { get; set; }
        public Product Products { get; set; }
        public Specials Specials { get; set; }
    }
}
