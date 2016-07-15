using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientSide_CaseStudy_2_Practise.Models
{
    public class Course
    {
				public int CourseId { get; set; }
				public string CourseName { get; set; }
				public string CourseAbbreviation { get; set; }
				//Defining a List<Student> type Students navigation property
				//to indicate that there is a one-to-many relationship
				//with the Student entity.
				public List<Student> Students { get; set; }

				public DateTime CreatedAt { get; set; }
				public DateTime UpdatedAt { get; set; }
				public DateTime? DeletedAt { get; set; }
    }//End of Course
}
