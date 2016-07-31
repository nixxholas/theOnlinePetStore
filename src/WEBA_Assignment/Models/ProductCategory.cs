using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class ProductCategory
    {
        public int ProdId { get; set; }
        public int CatId { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
