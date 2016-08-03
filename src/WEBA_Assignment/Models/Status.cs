using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WEBA_ASSIGNMENT.Models
{
    public class Status
    {
        public int StatusId { get; set; }
        public String StatusName { get; set; }
        [JsonIgnore]
        public List<Metrics> Metrics { get; set; }
    }
}
