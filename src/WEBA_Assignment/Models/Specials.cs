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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        // Let create 3 foreign key types should the user
        // require a brand-wide special discount, product discount
        // or even a category-wide special discount
        public List<BrandSpecials> BrandSpecials { get; set; }
        public List<CategorySpecials> CategorySpecials { get; set; }
        public List<ProductSpecials> ProductSpecials { get; set; }
        // Create two columns to either store a percentage type discount,
        // or a numerical type discount. Both of them do not have to be
        // nulled as they can be set to 0 to tell the Web API that the column's data
        // can be ignored.
        public double percentageDiscount { get; set; }
        public double numericalDiscount { get; set; }
        public string CreatedById { get; set; }

        public string UpdatedById { get; set; }

        public string DeletedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser DeletedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        //Defining a property with a ? symbol after the DateTime datatype,
        //to tell the .NET engine's Entity Framework that, this is a Nullable property.
        public DateTime? DeletedAt { get; set; }
    }
}
