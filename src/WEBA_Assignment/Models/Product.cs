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
        public String imgLink { get; set; }        
        public String Description { get; set; }
        public double Price { get; set; }
        public int BrandId { get; set; }
        //Defining a Brand class property to indicate that
        //there is a one to one relationship with the Brand entity.
        public Brands Brand { get; set; }
        public int Quantity { get; set; }
        public int ThresholdInvertoryQuantity { get; set; }
        public int Published { get; set; }
        // We don't need to have SpecialId to have a value.
        public int? SpecialId { get; set; }
        //Defining a Special class property to indicate that
        //there is a one to one relationship with the Product entity.
        public Specials Special { get; set; }
        public int MetricId { get; set; }
        public Metrics Metrics { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
