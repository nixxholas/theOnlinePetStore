using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientSide_CaseStudy_2_Practise.Models
{
    public class CompanyType
    {
				public int CompanyTypeId { get; set; }
				public string TypeName { get; set; }
				public List<Company> Companies { get; set; }
				public DateTime CreatedAt { get; set; }
				public DateTime UpdatedAt { get; set; }
				public DateTime? DeletedAt { get; set; }
    }
}
