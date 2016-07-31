using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class Product
    {
        public int ProdId { get; set; }
        public String ProdName { get; set; }     
        public String Description { get; set; }
        public int BrandId { get; set; }
        //Defining a Brand class property to indicate that
        //there is a one to one relationship with the Brand entity.
        public Brands Brand { get; set; }
        public List<ProductCategory> ProductCategory { get; set; }
        public int Quantity { get; set; }
        // Threshold inventory quantity is the minimum amount
        // of inventory a company wants to have on hand.
        public int? ThresholdInvertoryQuantity { get; set; }
        public int Published { get; set; }
        //Defining a List of Special class property to indicate that
        //there is a one to one relationship with the Product entity.
        public List<ProductSpecials> Specials { get; set; }
        public int? isConsumable { get; set; }
        public Consumable Consumable { get; set; }
        public List<Metrics> Metrics { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }
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
        // Even though status is defined for each metric, we still need to have one
        // for each product in case the user wants the entire product's status unavailable
        // However, this is rectified through is published.
    }
}
