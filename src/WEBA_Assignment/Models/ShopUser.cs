using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace WEBA_ASSIGNMENT.Models
{
    public class ShopUser
    {

        public int UserId { get; set; }
        public string UserName { get; set; }

        public string FullName { get; set; }

        public int CourseId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string IdentityCode { get; set; }

        public string Email { get; set; }

        public string MobileContact { get; set; }
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

    }//End of Student
}
