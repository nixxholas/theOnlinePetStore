using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Newtonsoft.Json;
using System.Globalization;
using ClientSide_CaseStudy_2_Practise.Models;

namespace ClientSide_CaseStudy_2_Practise.APIs
{
		[Route("api/[controller]")]
		public class StudentsController : Controller
		{
				//Create a property Database so that the code in the API methods
				//can use this property to communicate with the database. Note that, this Database
				//property is required in every controller. The property is initiatialized in the Controller's
				//Constructor. (In this case, the public StudentsController() constructor has been created
				//so that the Database property is instantiated as an ApplicationDbContext type property.
				public ApplicationDbContext Database { get; }

				//Create a Constructor, so that the .NET engine can pass in the dbContext object
				//which represents the database session.
				public StudentsController()
				{
						Database = new ApplicationDbContext();
				}

				// GET: api/Students
				[HttpGet]
				public JsonResult Get()
				{
						//The code here is slightly different from the Companies Web API controller's 
						//Get() method.
						//Notice that, instead of using the foreach, the code here uses the Select() method.
						//There are many ways to produce the same set of data for the client-side logic to use.
						/*var studentList = Database.Students
                 .Where(eachStudent => eachStudent.DeletedAt == null)
						            .Include(eachStudent => eachStudent.Course)
						            .Select(eachStudent=> new {
						             StudentId = eachStudent.StudentId,
						             FullName = eachStudent.FullName,
						             AdmissionId = eachStudent.AdmissionId,
						             CourseAbbreviation = eachStudent.Course.CourseAbbreviation,
						             DateOfBirth = eachStudent.DateOfBirth,
						             Email = eachStudent.Email,
						             MobileContact = eachStudent.MobileContact
						            });*/
						List<object> studentList = new List<object>();
						var students = Database.Students
					 .Where(eachStudent => eachStudent.DeletedAt == null)
					 .Include(eachStudent => eachStudent.Course);
						foreach (var oneStudent in students)
						{
								studentList.Add(new
								{
										StudentId = oneStudent.StudentId,
										FullName = oneStudent.FullName,
										AdmissionId = oneStudent.AdmissionId,
										CourseAbbreviation = oneStudent.Course.CourseAbbreviation,
										DateOfBirth = oneStudent.DateOfBirth,
										Email = oneStudent.Email,
										MobileContact = oneStudent.MobileContact
								});
						}
						return new JsonResult(studentList);
				}//end of Get()

				// GET api/Students/5
				[HttpGet("{id}")]
				public JsonResult Get(int id)
				{
						var oneStudent = Database.Students
								 .Where(item => item.StudentId == id).Include(p => p.Course).Single();

						var response = new
						{
								StudentId = oneStudent.StudentId,
								FullName = oneStudent.FullName,
								Email = oneStudent.Email,
								AdmissionId = oneStudent.AdmissionId,
								MobileContact = oneStudent.MobileContact,
								DateOfBirth = oneStudent.DateOfBirth,
								CourseId = oneStudent.CourseId,
						};//end of creation of the response object
						return new JsonResult(response);
				}//End of Get(id) method

				// GET: api/Students/GetStudentsGroupByCourse
				[HttpGet("GetStudentsGroupByCourse")]
				public JsonResult GetStudentsGroupByCourse()
				{
						List<object> studentList = new List<object>();
						//The following command will create a nested List.
						//Which means, if there are thre 3 courses, there will be
						//three "sub-List" inside studentGroupQueryResult
						var studentGroupQueryResult = Database.Students
								 .Where(eachStudent => eachStudent.DeletedAt == null)
										 .Include(eachStudent => eachStudent.Course)
										 .GroupBy(eachStudent => eachStudent.CourseId);
						//The following nested foreach loop aims to combine all the
						//student information from each sub-List of student data
						//into a single List. The studentList is used to consolidate
						//all the student data.		 	        
						foreach (var studentGroup in studentGroupQueryResult)
						{
								foreach (var eachStudent in studentGroup)
								{
										//Create new anonymous object and add into the studentList
										//container.
										studentList.Add(new
										{
												StudentId = eachStudent.StudentId,
												FullName = eachStudent.FullName,
												AdmissionId = eachStudent.AdmissionId,
												CourseAbbreviation = eachStudent.Course.CourseAbbreviation
										});
								}//end of the inner foreach block
						}//end of the outer foreach block
						return new JsonResult(studentList);
				}//End of GetStudentsGroupByCourse()

				// PUT api/Students/5
				[HttpPut("{id}")]
				public IActionResult Put(int id, [FromBody]string value)
				{
						string customMessage = "";
						var studentChangeInput = JsonConvert.DeserializeObject<dynamic>(value);
						//To obtain the full name information, use studentChangeInput.FullName.value
						//To obtain the email information, use studentChangeInput.Email.value
						var foundOneStudent = Database.Students
								.Where(item => item.StudentId == id).FirstOrDefault();

						foundOneStudent.FullName = studentChangeInput.FullName.Value;
						foundOneStudent.Email = studentChangeInput.Email.Value;
						foundOneStudent.AdmissionId = studentChangeInput.AdmissionId.Value;
						foundOneStudent.MobileContact = studentChangeInput.MobileContact.Value;
						//DateTime datatype is not that straightforward.
						//Need to make a DateTime datatype conversion first because
						//the DateOfBirth of the foundOneStudent instance is DateTime datatype.
						foundOneStudent.DateOfBirth = DateTime.ParseExact(studentChangeInput.DateOfBirth.Value, "d/M/yyyy", CultureInfo.InvariantCulture);
						foundOneStudent.CourseId = Convert.ToInt32(studentChangeInput.CourseId.Value);
						foundOneStudent.UpdatedAt = DateTime.Now;

						try
						{
								Database.Update(foundOneStudent);
								Database.SaveChanges();
						}
						catch (Exception ex)
						{
								if (ex.InnerException.Message
														.Contains("Student_AdmissionId_UniqueConstraint") == true)
								{
										customMessage = "Unable to save student record due " +
													"to another record having the same admin id : " +
										studentChangeInput.AdmissionId.Value;
										//Create a fail fail message anonymous object that has one property, Message.
										//This anonymous object's Message property contains a simple string message
										object httpFailRequestResultMessage = new { Message = customMessage };
										//Return a bad http request message to the client
										return HttpBadRequest(httpFailRequestResultMessage);
								}
						}//End of try .. catch block on saving data
						 //Construct a custom message for the client
						 //Create a success message anonymous object which has a 
						 //Message member variable (property)
						var successRequestResultMessage = new
						{
								Message = "Saved student record"
						};

						//Create a HttpOkObjectResult class instance, httpOkResult.
						//When creating the object, provide the previous message object into it.
						HttpOkObjectResult httpOkResult =
																									new HttpOkObjectResult(successRequestResultMessage);
						//Send the HttpOkObjectResult class object back to the client.
						return httpOkResult;

				}

				//POST api/Students
				[HttpPost]
				public IActionResult Post([FromBody]string value)
				{
						string customMessage = "";
						Student oneNewStudent = new Student();
						var studentNewInput = JsonConvert.DeserializeObject<dynamic>(value);
						//To obtain the full name information, use studentNewInput.FullName.value
						//To obtain the email information, use studentNewInput.Email.value
						oneNewStudent.FullName = studentNewInput.FullName.Value;
						oneNewStudent.Email = studentNewInput.Email.Value;
						oneNewStudent.AdmissionId = studentNewInput.AdmissionId.Value;
						oneNewStudent.MobileContact = studentNewInput.MobileContact.Value;
						//DateTime datatype is not that straightforward.
						//Need to make a DateTime datatype conversion first because
						//the DateOfBirth of the foundOneStudent instance is DateTime datatype.
						oneNewStudent.DateOfBirth = 
								DateTime.ParseExact(studentNewInput.DateOfBirth.Value, 
								"d/M/yyyy", CultureInfo.InvariantCulture);
						oneNewStudent.CourseId = Convert.ToInt32(studentNewInput.CourseId.Value);

						try
						{
								Database.Students.Add(oneNewStudent);
								Database.SaveChanges();
						}
						catch (Exception ex)
						{
								if (ex.InnerException.Message
														.Contains("Student_AdmissionId_UniqueConstraint") == true)
								{
										customMessage = "Unable to save student record due " +
													"to another record having the same admin id : " +
										studentNewInput.AdmissionId.Value;
										//Create a fail message anonymous object that has one property, Message.
										//This anonymous object's Message property contains a simple string message
										object httpFailRequestResultMessage = new { Message = customMessage };
										//Return a bad http request message to the client
										return HttpBadRequest(httpFailRequestResultMessage);
								}
						}//End of try .. catch block on saving data
						 //Construct a custom message for the client
						 //Create a success message anonymous object which has a 
						 //Message member variable (property)
						var successRequestResultMessage = new
						{
								Message = "Saved student record"
						};

						//Create a HttpOkObjectResult class instance, httpOkResult.
						//When creating the object, provide the previous message object into it.
						HttpOkObjectResult httpOkResult =
									new HttpOkObjectResult(successRequestResultMessage);
						//Send the HttpOkObjectResult class object back to the client.
						return httpOkResult;
				}//End of Post() method


				// DELETE api/Students/5
				[HttpDelete("{id}")]
				public IActionResult Delete(int id)
				{
						string customMessage = "";

						try
						{
								var foundOneStudent = Database.Students
								.Single(eachStudent => eachStudent.StudentId == id);
								foundOneStudent.DeletedAt = DateTime.Now;

								//Update the database model
								Database.Update(foundOneStudent);
								//Tell the db model to commit/persist the changes to the database, 
								//I use the following command.
								Database.SaveChanges();
						}
						catch (Exception ex)
						{
								customMessage = "Unable to delete student record.";
								object httpFailRequestResultMessage = new { Message = customMessage };
								//Return a bad http request message to the client
								return HttpBadRequest(httpFailRequestResultMessage);
						}//End of try .. catch block on manage data

						//Build a custom message for the client
						//Create a success message anonymous object which has a 
						//Message member variable (property)
						var successRequestResultMessage = new
						{
								Message = "Deleted student record"
						};

						//Create a HttpOkObjectResult class instance, httpOkResult.
						//When creating the object, provide the previous message object into it.
						HttpOkObjectResult httpOkResult =
																		new HttpOkObjectResult(successRequestResultMessage);
						//Send the HttpOkObjectResult class object back to the client.
						return httpOkResult;
				}//end of Delete() Web API method

				// GET: api/Students/GetStudentListByCourse/5
				[HttpGet("GetStudentListByCourse/{inCourseId}")]
				public JsonResult GetStudentListByCourse(int inCourseId)
				{
						List<object> studentList = new List<object>();
						object dataSummary = new object();
						//The following command will create an object, foundOneCourse
						//which represents the Course entity which matches the given course id
						//provided by the input parameter, inCourseId.
						//This foundOneCourse object is needed because the Course Abbreviation name is required
						//in the custom JSON content resultset.
						var foundOneCourse = Database.Courses
									.Where(eachCourse => eachCourse.CourseId == inCourseId).AsNoTracking().Single();

						//The following command will create an object, studentsQueryResult
						//which will only have one internal list of Student entities.
						//These Student entities has CourseId property which meets the search
						//criteria given by the inCourseId input parameter.
						var studentsQueryResult = Database.Students
											.Where(eachStudent => (eachStudent.DeletedAt == null) && (eachStudent.CourseId == inCourseId))
											.Include(eachStudent => eachStudent.Course);
						//The following foreach loop aims to create a
						//into a single List of anonymous objects, studentList.        
						foreach (var eachStudent in studentsQueryResult)
						{
								//Create new anonymous object and add into the studentList
								//container.
								studentList.Add(new
								{
										StudentId = eachStudent.StudentId,
										FullName = eachStudent.FullName,
										AdmissionId = eachStudent.AdmissionId,
										MobileContact = eachStudent.MobileContact,
										CourseAbbreviation = eachStudent.Course.CourseAbbreviation
								});
						}//end of the foreach block
						if (studentsQueryResult.ToList().Count == 0)
						{
								dataSummary = new
								{
										CourseAbbreviation = foundOneCourse.CourseAbbreviation,
										StudentList = studentList,
										Message = "There are no students found for this course."
								};
						}
						else
						{
								dataSummary = new
								{
										CourseAbbreviation = foundOneCourse.CourseAbbreviation,
										StudentList = studentList,
										Message = string.Format("There are {0} students associated to {1} course.",
																			studentList.Count.ToString(), foundOneCourse.CourseAbbreviation)
								};
						}//end if
						return new JsonResult(dataSummary);
				}//End of GetStudentListByCourse()
		}//End of Students Web API controller class
}//End of namespace
