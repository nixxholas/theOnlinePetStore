using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBACA.Models
{
    public class Visibility
    {
        public int VisibilityId { get; set; }
        public String VisibilityName { get; set; }
        public List<Category> Categories { get; set; }
    }
}
