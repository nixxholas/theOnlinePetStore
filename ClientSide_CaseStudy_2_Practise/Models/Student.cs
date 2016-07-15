using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientSide_CaseStudy_2_Practise.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string AdmissionId { get; set; }
        public string Email { get; set; }
        public string MobileContact { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CourseId { get; set; }

        //Defining a Course class property, Course to indicate that
        //there is a one to one relationship with the Course entity.
        public Course Course { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }//End of Student
}
