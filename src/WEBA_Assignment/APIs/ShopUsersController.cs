using WEBA_ASSIGNMENT.Models;
//Need InternshipManagementSystemWithSecurity_V1.Data so that the .NET can find
//the ApplicationDbContext class.
using WEBA_ASSIGNMENT.Data;
using WEBA_ASSIGNMENT.Services;
using WEBA_ASSIGNMENT.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace WEBA_ASSIGNMENT.APIs
{
    [Authorize]
    [Route("api/[controller]")]
    public class ShopUsersController : Controller
    {
        //Create a property Database so that the code in the Web API controller methods
        //can use this property to communicate with the database. Note that, this Database
        //property is required in every Web API controller. The property is initiatialized in the Controller's
        //Constructor. (In this case, the public UsersController() constructor has been created
        //so that the Database property is instantiated as an ApplicationDbContext instance.

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public ApplicationDbContext Database { get; }


				//Create a Constructor, so that the .NET engine can pass in the ApplicationDbContext object
				//which represents the database session.
		public ShopUsersController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory)
        {
            Database = new ApplicationDbContext();
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }



        // GET: api/Users
        [HttpGet]
        public JsonResult Get()
        {
            List<object> shopUserList = new List<object>();
						var shopUsersQueryResult = Database.ShopUsers
								 .Where(eachUser => eachUser.DeletedAt == null)
								 .Include(eachUser => eachUser.CreatedBy)
								 .Include(eachUser => eachUser.UpdatedBy);
            //After obtaining all the user records from the database,
            //the users object will become a container holding these User entities.
            //I need to loop through each  User instance inside users
            //so that I can build a userList which contains anonymous objects.
            foreach (var oneUser in shopUsersQueryResult)
            {
                shopUserList.Add(new
                {
                    UserId = oneUser.UserId,
                    FullName = oneUser.FullName,
                    IdentityCode = oneUser.IdentityCode,
                    Email = oneUser.Email,
                    MobileContact = oneUser.MobileContact,
                    DateOfBirth = oneUser.DateOfBirth,
										CreatedBy = oneUser.CreatedBy.FullName,
										UpdatedBy = oneUser.UpdatedBy.FullName,
                    });
            }//end of foreach loop which builds the userList List container .
												//Use the JsonResult class to create a new JsonResult object by using the userList.
												//The ASP.NET framework will do the rest to translate it into a string JSON structured content
												//which can travel through the Internet wire to the client browser.
            return new JsonResult(shopUserList);
        }//end of Get()

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            List<object> shopUserList = new List<object>();
            //The include() feature requires Microsoft Data Entity for now.
            var foundOneUser = Database.ShopUsers
                 .Where(eachUser => eachUser.UserId == id).Single();

            //Create an anonymous object, response.
            var response = new
            {
                UserId = foundOneUser.UserId,
                FullName = foundOneUser.FullName,
                Email = foundOneUser.Email,
                IdentityCode = foundOneUser.IdentityCode,
                MobileContact = foundOneUser.MobileContact,
                DateOfBirth = foundOneUser.DateOfBirth,
                };//end of creation of the response object
            return new JsonResult(response);
        }

        // PUT api/Users/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
		    string customMessage = "";
            var userChangeInput = JsonConvert.DeserializeObject<dynamic>(value);
            //To obtain the full name information, use userChangeInput.FullName.value
            //To obtain the email information, use userChangeInput.Email.value
            var foundOneUser = Database.ShopUsers
                .Where(eachUser => eachUser.UserId == id).FirstOrDefault();

            foundOneUser.FullName = userChangeInput.FullName.Value;
            foundOneUser.Email = userChangeInput.Email.Value;
            foundOneUser.IdentityCode = userChangeInput.IdentityCode.Value;
            foundOneUser.MobileContact = userChangeInput.MobileContact.Value;
            //DateTime datatype is not that straightforward.
            //Need to make a DateTime datatype conversion first because
            //the DateOfBirth of the foundOneUser instance is DateTime datatype.
            foundOneUser.DateOfBirth =DateTime.ParseExact(userChangeInput.DateOfBirth.Value, 
                "d/M/yyyy", CultureInfo.InvariantCulture);
            foundOneUser.CourseId = Convert.ToInt32(userChangeInput.CourseId.Value);
            foundOneUser.UpdatedAt = DateTime.Now;
			      foundOneUser.UpdatedById = _userManager.GetUserId(User); 
			try { 
            Database.Update(foundOneUser);
            Database.SaveChanges();
			} catch (Exception ex){
                if (ex.InnerException.Message
					                  .Contains("User_IdentityCode_UniqueConstraint") == true)
                {
                    customMessage = "Unable to save user record due " +
						              "to another record having the same admin id : " +
                    userChangeInput.IdentityCode.Value;
                    //Create a fail message anonymous object that has one property, Message.
                    //This anonymous object's Message property contains a simple string message
                    object httpFailRequestResultMessage = new { Message = customMessage };
                    //Return a bad http request message to the client
                    return BadRequest(httpFailRequestResultMessage);
                }
            }//End of try .. catch block on saving data
            //Construct a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Saved user record"
            };

            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult = 
				                                          new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;

        }
				//POST api/Users
				[HttpPost]
				public IActionResult Post([FromBody]string value)
				{
						string customMessage = "";
						ShopUser oneNewUser = new ShopUser();
						var userNewInput = JsonConvert.DeserializeObject<dynamic>(value);
						//To obtain the full name information, use userNewInput.FullName.value
						//To obtain the email information, use userNewInput.Email.value
						oneNewUser.FullName = userNewInput.FullName.Value;
						oneNewUser.Email = userNewInput.Email.Value;
						oneNewUser.IdentityCode = userNewInput.IdentityCode.Value;
						oneNewUser.MobileContact = userNewInput.MobileContact.Value;
						//DateTime datatype is not that straightforward.
						//Need to make a DateTime datatype conversion first because
						//the DateOfBirth of the foundOneUser instance is DateTime datatype.
						oneNewUser.DateOfBirth =
								DateTime.ParseExact(userNewInput.DateOfBirth.Value,
								"d/M/yyyy", CultureInfo.InvariantCulture);
						oneNewUser.CourseId = Convert.ToInt32(userNewInput.CourseId.Value);
						//---------------- Security and Authorization code ----(Start)------------------------------
						//When the record is created, the CreatedById and the UpdatedById property
						//has the same value.
						oneNewUser.CreatedById = _userManager.GetUserId(User);
						oneNewUser.UpdatedById = _userManager.GetUserId(User);
						//Don't need oneNewUser.CreatedAt = DateTime.Now; because there is 
						//a default value using GETDATE() setup in the database.
						//---------------- Security and Authorization code ------(End)------------------------------
						try
						{
								Database.ShopUsers.Add(oneNewUser);
								Database.SaveChanges();
						}
						catch (Exception ex)
						{
								if (ex.InnerException.Message
														.Contains("User_IdentityCode_UniqueConstraint") == true)
								{
										customMessage = "Unable to save user record due " +
													"to another record having the same admin id : " +
										userNewInput.IdentityCode.Value;
										//Create a fail message anonymous object that has one property, Message.
										//This anonymous object's Message property contains a simple string message
										object httpFailRequestResultMessage = new { Message = customMessage };
										//Return a bad http request message to the client
										return BadRequest(httpFailRequestResultMessage);
								}
						}//End of try .. catch block on saving data
						 //Construct a custom message for the client
						 //Create a success message anonymous object which has a 
						 //Message member variable (property)
						var successRequestResultMessage = new
						{
								Message = "Saved user record"
						};

						//Create a OkObjectResult class instance, httpOkResult.
						//When creating the object, provide the previous message object into it.
						OkObjectResult httpOkResult =
									new OkObjectResult(successRequestResultMessage);
						//Send the OkObjectResult class object back to the client.
						return httpOkResult;
				}//End of Post() method

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string customMessage = "";

            try
            {
                var foundOneUser = Database.ShopUsers
                .Single(eachUser => eachUser.UserId == id);
                foundOneUser.DeletedAt = DateTime.Now;
                foundOneUser.DeletedById = _userManager.GetUserId(User);

                //Update the database model
                Database.Update(foundOneUser);
                //Tell the db model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                customMessage = "Unable to delete user record.";
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of try .. catch block on manage data

            //Build a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Deleted user record"
            };

            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                                                            new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;

        }
        
 }
}
