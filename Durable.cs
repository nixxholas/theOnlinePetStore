using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBACA.Models
{
    // Object for non-consumable products.
    public class Durable : Product
    {
        public Product Product { get; set; }
    }
}
