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
        // Let create 3 foreign key types should the user
        // require a brand-wide special discount, product discount
        // or even a category-wide special discount
        public int? BrandId { get; set; }
        public List<Brands> Brands { get; set; }
        public int? CatId { get; set; }
        public List<Category> Categories { get; set; }
        public int Product { get; set; }
        public List<Product> Products { get; set; }
        // Create two columns to either store a percentage type discount,
        // or a numerical type discount. Both of them do not have to be
        // nulled as they can be set to 0 to tell the Web API that the column's data
        // can be ignored.
        public double percentageDiscount { get; set; }
        public double numericalDiscount { get; set; }
    }
}
