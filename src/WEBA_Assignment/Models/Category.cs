using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class Category
    {
        public int CatId { get; set; }
        public String CatName { get; set; }
        // public int NoOfSubCategories { get; set; }
        public List<BrandCategory> BrandCategory { get; set; }
        public List<ProductCategory> ProductCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedById { get; set; }

        public string UpdatedById { get; set; }

        public string DeletedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser DeletedBy { get; set; }
        public int VisibilityId { get; set; }
        public Visibility Visibility { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
