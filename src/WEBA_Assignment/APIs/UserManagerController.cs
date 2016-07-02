using WEBA_ASSIGNMENT.Models;
using WEBA_ASSIGNMENT.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.APIs
{
    [Authorize("RequireAdminRole")]
    [Route("api/[controller]")]
    public class UserManagerController : Controller
    {
        //Create a property DbContext so that the code in the Action Methods
        //can use this property to communicate with the database. Note that, this DbContext
        //property is required in every controller. The property is initiatialized in the Controller's
        //Constructor. (In this case, the public UserManagerController() constructor has been created
        //so that the DbContext property is instantiated as an ApplicationDbContext instance.
        public ApplicationDbContext Database { get; }


        //Create a Constructor, so that the .NET engine can pass in the dbContext object
        //which represents the database session.
        public UserManagerController()
        {
            Database = new ApplicationDbContext();
        }
        // GET: api/UserManager
        [HttpGet]
        public  IActionResult Get()
        {
            List<object> applicationUserList = new List<object>();
            //The include() feature requires Microsoft Data Entity for now.
            //The visual studio might suggest you to install some packages for the include()
            //to be recognized. Don't select them.

            //Reference: http://stackoverflow.com/questions/26078271/getting-a-list-of-users-with-their-assigned-role-in-identity-2
            //I refered to the online article to get the folowing LINQ working. Notice that I do not need to apply foreach to create the JSON structure
            //object. I can create one by just using LINQ.
            var users = from user in Database.Users
                select new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
					FullName = user.FullName,
                    Roles = Database.Roles
                        .Where(input => user.Roles.Select(r => r.RoleId)
                        .Contains(input.Id))
                        .Select(r => r.Name)
                };

            return new JsonResult(users);
        }//end of Get()

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            //Notice that the input parameter id is type String. This is due to the GUID
            //of the user which is type string too.
            //The include() feature requires Microsoft Data Entity for now.
            var oneUser = Database.Users
                 .Where(userItem => userItem.Id == id)
                 .Include(p => p.Roles).Single();


            var response = new
            {
                Id = oneUser.Id,
                UserName = oneUser.UserName,
                FullName = oneUser.FullName,
                Email = oneUser.Email,
                RoleId = oneUser.Roles.FirstOrDefault().RoleId,
            };//end of creation of the response object


            return new JsonResult(response);
        }



        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]string value)
        {
            string databaseInnerExceptionMessage = "";
            var userChangeInput = JsonConvert.DeserializeObject<dynamic>(value);
            List<object> messages = new List<object>();
            bool status = true; //This variable is used to track the overall success of all the database operations
            object response;
            //http://stackoverflow.com/questions/20444022/updating-user-data-asp-net-identity
            //Database is our database context set in this controller.
            //I used the following 2 lines of command to create a userStore which represents AspNetUser table in the DB.
            var userStore = new UserStore<ApplicationUser>(Database);
            //Then, I created a userManager instance that operates on the userStore.
            var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
           
            //To obtain the full name information, use student.FullName.value
            //To obtain the email information, use student.Email.value
            var oneUser = Database.Users
                .Where(item => item.Id == id)
                .Include(p=>p.Roles).FirstOrDefault();
                        
            //The following code to obtain the role name are asynchronous and not reliable.
            //Doing a bypass by leveraging on the client-side JavaScript.
            // var newRoleNameToAdd =  identityRoleManager.FindByIdAsync(user.RoleId.Value).Result;
            // var existingRoleNameToRemove =   identityRoleManager.FindByIdAsync(oneUser.Roles.First().RoleId).Result;
            
            oneUser.UserName = userChangeInput.UserName;
            oneUser.FullName = userChangeInput.FullName.Value;
            oneUser.Email = userChangeInput.Email.Value;

            try {

                //Although the command below correct but violated foreign key constraint
                //Must let the UserManager instance, userManager do the job.
                //oneUser.Roles.FirstOrDefault().RoleId = user.RoleId.Value;

                //All the following 3 method calls are asynchronous. Notice that I delcared the Put method as Asynchronous method too.
                //http://stackoverflow.com/questions/20444022/updating-user-data-asp-net-identity has some hints how to define the return
                //type of this Put method here. Also, notice that I applied a technique to make these 3 asynchronous calls to become synchronous.

                //Remove existing role from the user
                var removeRoleFromUserResult = userManager.RemoveFromRoleAsync(oneUser, userChangeInput.OriginalRoleName.Value).Result;
                //Add the new role to the user.
                var addRoleToUserResult = userManager.AddToRoleAsync(oneUser, userChangeInput.SelectedRoleName.Value).Result;
                //Update the 
                var updateUserResult = userManager.UpdateAsync(oneUser).Result;

                try
                {
                    //Changes are not persisted in the database until
                    //I use the following command.
                    userStore.Context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    databaseInnerExceptionMessage = ex.InnerException.Message;
                    status = false;
                    messages.Add(databaseInnerExceptionMessage);
                }



                if (removeRoleFromUserResult.Succeeded != true)
                {
                    status = false;
                    messages.Add(removeRoleFromUserResult);
                }
                if (addRoleToUserResult.Succeeded != true)
                {
                    status = false;
                    messages.Add(addRoleToUserResult);
                }
                if (updateUserResult.Succeeded != true)
                {
                    status = false;
                    messages.Add(updateUserResult);
                }
                if (status == true)
                {
                    response = new { Status = "success", Message = "Saved user record." };
                }
                else
                {
                    response = new { Status = "fail", Message = messages };
                }
            }catch(Exception outerException)
            {
                response = new { status = "fail", Message = outerException.InnerException.Message };
            }
            return new JsonResult(response);
        }//End of Put()
				 // POST api/Post/
				[HttpPost]
				public async Task<IActionResult> Post([FromBody]string value)
				{
						string databaseInnerExceptionMessage = "";
						var userNewInput = JsonConvert.DeserializeObject<dynamic>(value);
						List<object> messages = new List<object>();
						bool status = true; //This variable is used to track the overall success of all the database operations
						object response = null;
						//http://stackoverflow.com/questions/20444022/updating-user-data-asp-net-identity
						//Database is our database context set in this controller.
						//I used the following 2 lines of command to create a userStore which represents AspNetUser table in the DB.
						var userStore = new UserStore<ApplicationUser>(Database);
						//Then, I created a userManager instance that operates on the userStore.
						var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
						string selectedRoleName = "";
						//To obtain the full name information, use student.FullName.value
						//To obtain the email information, use student.Email.value
						var newUser = new ApplicationUser();

						newUser.UserName = userNewInput.UserName;
						newUser.FullName = userNewInput.FullName.Value;
						newUser.Email = userNewInput.Email.Value;
						PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
						newUser.PasswordHash = passwordHasher.HashPassword(newUser, "P@ssw0rd"); //More complex password
						newUser.SecurityStamp = Guid.NewGuid().ToString();
						newUser.NormalizedEmail = newUser.Email;
						newUser.NormalizedUserName = newUser.UserName;
						//I had problems with the command:
						//await userStore.AddToRoleAsync(newUser, userNewInput.SelectedRoleName.Value);
						//inside the try section. As a result, I need to read out the role name information.
						selectedRoleName = userNewInput.SelectedRoleName.Value;
						try
						{
								dynamic addUserResult = null;
								try
								{
								    //Cannot use the normal programming techniques (Chap 2 to Chap8)
										// to save new user information.
										addUserResult = await userStore.CreateAsync(newUser);
									  await userStore.AddToRoleAsync(newUser, selectedRoleName);
										userStore.Context.SaveChanges();
										response = new { Status = "success", Message = "Saved new user record." };
								}
								catch (DbUpdateException ex)
								{
										databaseInnerExceptionMessage = ex.InnerException.Message;
										status = false;
										messages.Add(databaseInnerExceptionMessage);
								}
								
						}
						catch (Exception outerException)
						{
								response = new { status = "fail", Message = outerException.InnerException.Message };
						}
						return new JsonResult(response);
				}//End of Post()



		}//End of UserManagerController class


}
