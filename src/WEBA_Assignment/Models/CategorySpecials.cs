using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class CategorySpecials
    {
        public int CatId { get; set; }
        public int SpecialId { get; set; }
        public Category Categories { get; set; }
        public Specials Specials { get; set; }
    }
}
