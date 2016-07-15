using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ClientSide_CaseStudy_2_Practise.Models
{
    public class Company
    {
				public int CompanyId { get; set; }
				public string CompanyName { get; set; }
				public string Address { get; set; }
				public string PostalCode { get; set; }
		      
				public int CompanyTypeId {get;set;}
				public CompanyType CompanyType {get;set;}

				public DateTime CreatedAt { get; set; }
				public DateTime UpdatedAt { get; set; }
				public DateTime? DeletedAt { get; set; }
    }
}

