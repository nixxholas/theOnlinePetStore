using Microsoft.AspNet.Builder;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace ClientSide_CaseStudy_2_Practise.Models
{
    public static class DataSeeder
    {
        public static async void SeedData(this IApplicationBuilder app)
        {
            var db = app.ApplicationServices.GetService<ApplicationDbContext>();
			
            db.Database.Migrate();

			            CompanyType productDevelopmentType, softwareDevelopmentType, healthCareTechType;
    
                   productDevelopmentType = new CompanyType()
                  {
                      TypeName = "PRODUCT DEVELOPMENT"
                  };
                  db.CompanyTypes.Add(productDevelopmentType);

                  softwareDevelopmentType = new CompanyType()
                  {
                      TypeName = "SOFTWARE DEVELOPMENT"
                  };
                  db.CompanyTypes.Add(softwareDevelopmentType);

			              healthCareTechType = new CompanyType()
                  {
                      TypeName = "HEALTH CARE TECHNOLOGY"
                  };
                  db.CompanyTypes.Add(healthCareTechType);


            Company companyA, companyB = null;
            Company companyC, companyD = null;
            Company companyE, companyF = null;
            Company companyG, companyH = null;
            Company companyI, companyJ = null;

            companyA = new Company()
                {
                    CompanyName = "COMPANY A",
                    Address = "COMPANY A ADDRESS",
                    PostalCode = "888881",
					CompanyTypeId = healthCareTechType.CompanyTypeId
                };
                db.Companies.Add(companyA);
                companyB = new Company()
                {
                    CompanyName = "COMPANY B",
                    Address = "COMPANY B ADDRESS",
                    PostalCode = "888882",
					CompanyTypeId = productDevelopmentType.CompanyTypeId
                };
                db.Companies.Add(companyB);
                companyC = new Company()
                {
                    CompanyName = "COMPANY C",
                    Address = "COMPANY C ADDRESS",
                    PostalCode = "888883",
					CompanyTypeId = softwareDevelopmentType.CompanyTypeId
                };
                db.Companies.Add(companyC);
                companyD = new Company()
                {
                    CompanyName = "COMPANY D",
                    Address = "COMPANY D ADDRESS",
                    PostalCode = "888884",
					CompanyTypeId = productDevelopmentType.CompanyTypeId
                };
                db.Companies.Add(companyD);
                companyE = new Company()
                {
                    CompanyName = "COMPANY E",
                    Address = "COMPANY E ADDRESS",
                    PostalCode = "888885",
					CompanyTypeId = productDevelopmentType.CompanyTypeId
                };
                db.Companies.Add(companyE);
                companyF = new Company()
                {
                    CompanyName = "COMPANY F",
                    Address = "COMPANY F ADDRESS",
                    PostalCode = "888886",
					CompanyTypeId = productDevelopmentType.CompanyTypeId
                };
                db.Companies.Add(companyF);
                companyG = new Company()
                {
                    CompanyName = "COMPANY G",
                    Address = "COMPANY G ADDRESS",
                    PostalCode = "888887",
					CompanyTypeId = healthCareTechType.CompanyTypeId
                };
                db.Companies.Add(companyG);
                companyH = new Company()
                {
                    CompanyName = "COMPANY H",
                    Address = "COMPANY H ADDRESS",
                    PostalCode = "888888",
					CompanyTypeId = healthCareTechType.CompanyTypeId
                };
                db.Companies.Add(companyH);
                companyI = new Company()
                {
                    CompanyName = "COMPANY I",
                    Address = "COMPANY I ADDRESS",
                    PostalCode = "888889",
					CompanyTypeId = softwareDevelopmentType.CompanyTypeId
                };
                db.Companies.Add(companyI);
                companyJ = new Company()
                {
                    CompanyName = "COMPANY J",
                    Address = "COMPANY J ADDRESS",
                    PostalCode = "888810",
					CompanyTypeId = softwareDevelopmentType.CompanyTypeId
                };
                db.Companies.Add(companyJ);
            

			//Add Course records into the Course table

            Course ditCourse, dbitCourse, dismCourse;
    
                   ditCourse = new Course()
                  {
                      CourseAbbreviation = "DIT",
                      CourseName = "DIPLOMA IN INFORMATION TECHNOLOGY"
                  };
                  db.Courses.Add(ditCourse);
                   dbitCourse = new Course()
                  {
                      CourseAbbreviation = "DBIT",
                      CourseName = "DIPLOMA IN BUSINESS INFORMATION TECHNOLOGY"
                  };
                  db.Courses.Add(dbitCourse);
                   dismCourse = new Course()
                  {
                      CourseAbbreviation = "DISM",
                      CourseName = "DIPLOMA IN INFOCOMM SECURITY MANAGEMENT"
                  };
                  db.Courses.Add(dismCourse);
			   
			//Add Student records into the Student table

            //Declaring the Student objects and assign it to null.
            //Need to assign null first so that the annoying using unassigned variable
            //error is not surfaced by Visual Studio.
             Student georgeStudent = null,amyStudent = null, johnStudent = null, 
                  rachelStudent = null, simonStudent = null, steveStudent = null;
             Student larryStudent = null,davidStudent = null,cindyStudent = null,
                  richardStudent,desmondStudent = null;


                amyStudent = new Student()
                {
                    FullName = "AMY",
                    AdmissionId = "0208881",
                    MobileContact = "98001201",
                    Email = "AMY@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("24/01/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = dismCourse.CourseId
                };
                db.Students.Add(amyStudent);
                johnStudent = new Student()
                {
                    FullName = "JOHN",
                    AdmissionId = "0208882",
                    MobileContact = "98001202",
                    Email = "JOHN@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("30/03/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = ditCourse.CourseId
                };
                db.Students.Add(johnStudent);
                rachelStudent = new Student()
                {
                    FullName = "RACHEL",
                    AdmissionId = "0208883",
                    MobileContact = "98001203",
                    Email = "RACHEL@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("15/05/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = dbitCourse.CourseId
                };
                db.Students.Add(rachelStudent);
                simonStudent = new Student()
                {
                    FullName = "SIMON",
                    AdmissionId = "0208884",
                    MobileContact = "98001204",
                    Email = "SIMON@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("10/05/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = dismCourse.CourseId
                };
                db.Students.Add(simonStudent);
                steveStudent = new Student()
                {
                    FullName = "STEVE",
                    AdmissionId = "0208885",
                    MobileContact = "98001205",
                    Email = "STEVE@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("21/08/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = ditCourse.CourseId
                };
                db.Students.Add(steveStudent);
                larryStudent = new Student()
                {
                    FullName = "LARRY",
                    AdmissionId = "0208886",
                    MobileContact = "98001206",
                    Email = "LARRY@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("21/08/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = dbitCourse.CourseId
                };
                db.Students.Add(larryStudent);
                davidStudent = new Student()
                {
                    FullName = "DAVID",
                    AdmissionId = "0208887",
                    MobileContact = "98001207",
                    Email = "DAVID@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("15/01/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = ditCourse.CourseId
                };
                db.Students.Add(davidStudent);
                cindyStudent = new Student()
                {
                    FullName = "CINDY",
                    AdmissionId = "0208888",
                    MobileContact = "98001208",
                    Email = "CINDY@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("21/07/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = dismCourse.CourseId
                };
                db.Students.Add(cindyStudent);
                richardStudent = new Student()
                {
                    FullName = "RICHARD",
                    AdmissionId = "0208889",
                    MobileContact = "98001209",
                    Email = "RICHARD@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("14/02/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = dismCourse.CourseId
                };
                db.Students.Add(richardStudent);
                desmondStudent = new Student()
                {
                    FullName = "DESMOND",
                    AdmissionId = "0208890",
                    MobileContact = "98888810",
                    Email = "DESMOND@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("16/02/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = ditCourse.CourseId
                };
                db.Students.Add(desmondStudent);
                georgeStudent = new Student()
                {
                    FullName = "GEORGE",
                    AdmissionId = "0208891",
                    MobileContact = "98001211",
                    Email = "GEORGE@EMU.COM",
                    DateOfBirth = DateTime.ParseExact("23/02/1995", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CourseId = ditCourse.CourseId
                };
                db.Students.Add(georgeStudent);



            db.SaveChanges();
		            return;
        }//End of SeedData() method
    }//End of Static class
}//End of Namespace
