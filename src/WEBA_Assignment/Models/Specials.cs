using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class Specials
    {
        public int SpecialId { get; set; }
        public String SpecialName { get; set; }
        public List<Product> Products { get; set; }
        public double percentageDiscount { get; set; }
    }
}
