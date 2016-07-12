using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class Status
    {
        public int StatusId { get; set; }
        public String StatusName { get; set; }
        public List<Metrics> Metrics { get; set; }
    }
}
