using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class Consumable : Product
    {
        public Product Product { get; set; }
        public String TypicalAnalysis { get; set; }
        public String GuranteedAnalysis { get; set; }
        public String Ingredients { get; set; }
        // For supplements
        public String ActiveIngredients { get; set; }
        // For supplements
        public String InActiveIngredients { get; set; }

    }
}
