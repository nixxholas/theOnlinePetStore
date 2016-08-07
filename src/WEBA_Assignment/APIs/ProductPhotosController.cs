using WEBA_ASSIGNMENT.Models;
//Need InternshipManagementSystemWithSecurity_V1.Data so that the .NET can find
//the ApplicationDbContext class.
using WEBA_ASSIGNMENT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WEBA_ASSIGNMENT.Services;
using Microsoft.Extensions.Logging;
using WEBA_ASSIGNMENT.Controllers;

namespace WEBA_ASSIGNMENT.APIs
{
    [Authorize(Roles = "SUPER ADMIN,ADMIN")]
    [Route("api/[controller]")]
    public class ProductPhotosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        //Every Web API controller class need to have an ApplicationDbContext
        //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }
        public ProductPhotosController(UserManager<ApplicationUser> userManager,
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

        // GET: api/values 
        [HttpGet]
        public JsonResult Get()
        {
            List<object> productPhotosList = new List<object>();
            var productPhotos = Database.ProductPhotos
                .Include(input => input.CreatedBy);
                //.Include(input => input.DeletedBy);

            foreach (var productphoto in productPhotos)
            {
                productPhotosList.Add(new
                {
                    ProductPhotoId = productphoto.ProductPhotoId,
                    ProdId = productphoto.ProdId,
                    PublicCloudinaryId = productphoto.PublicCloudinaryId,
                    SecureUrl = productphoto.SecureUrl,
                    Url = productphoto.Url,
                    isPrimaryPhoto = productphoto.isPrimaryPhoto,
                    CreatedAt = productphoto.CreatedAt,
                    CreatedBy = productphoto.CreatedBy.FullName,
                    //DeletedAt = productphoto.DeletedAt,
                    //DeletedBy = productphoto.DeletedBy.FullName
                });
            }//end of foreach loop which builds the categoryList . 
            return new JsonResult(productPhotosList);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            List<Object> productPhotoList = new List<Object>();

            try
            {
                var foundProductPhotos = Database.ProductPhotos
                    .Where(input => input.ProdId == id)
                    .Where(input => input.DeletedAt == null)
                    .Include(input => input.CreatedBy);

                foreach (var productPhoto in foundProductPhotos)
                {
                    productPhotoList.Add(new
                    {
                        ProductPhotoId = productPhoto.ProductPhotoId,
                        ProdId = productPhoto.ProdId,
                        PublicCloudinaryId = productPhoto.PublicCloudinaryId,
                        SecureUrl = productPhoto.SecureUrl,
                        Url = productPhoto.Url,
                        isPrimaryPhoto = productPhoto.isPrimaryPhoto,
                        CreatedAt = productPhoto.CreatedAt,
                        CreatedBy = productPhoto.CreatedBy.FullName
                    });
                }

                return new JsonResult(productPhotoList);
            }
            catch (Exception exceptionObject)
            {
                //Create a fail message anonymous object
                //This anonymous object only has one Message property 
                //which contains a simple string message
                object httpFailRequestResultMessage =
                                            new { Message = "Unable to obtain images for your product." };
                //Return a bad http response message to the client
                return BadRequest(httpFailRequestResultMessage);
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            string customMessage = "";
            string format = "dd/MM/yyyy";
            //Reconstruct a useful object from the input string value. 
            dynamic categoryNewInput = JsonConvert.DeserializeObject<dynamic>(value);
            Category newCategory = new Category();
            try
            {
                //Copy out all the category data into the new category instance,
                //newcategory.
                newCategory.CatName = categoryNewInput.CatName.Value;
                newCategory.VisibilityId = Int32.Parse(categoryNewInput.VisibilityId.Value);
                if (newCategory.VisibilityId == 2)
                {
                    newCategory.StartDate = DateTime.ParseExact(categoryNewInput.StartDate.Value, format, System.Globalization.CultureInfo.InvariantCulture);
                    newCategory.EndDate = DateTime.ParseExact(categoryNewInput.EndDate.Value, format, System.Globalization.CultureInfo.InvariantCulture);
                    // The one big reason why I implemented server-sided validation.
                    // http://net-informations.com/faq/asp/validation.htm
                    //
                    // Let's compare the two dates to make sure the StartDate is later than EndDate
                    if (DateTime.Compare(newCategory.StartDate.Value, newCategory.EndDate.Value) > 0)
                    {
                        // But if it is later, we'll toss them an error for SweetAlert to throw out.
                        customMessage = "Please enter your start date that is before your end date.";
                        object httpFailRequestResultMessage = new { Message = customMessage };
                        // Return a bad http request message to the client
                        // Good job to the user who's trolling with me, good try
                        return BadRequest(httpFailRequestResultMessage);
                    }
                }
                else
                {
                    newCategory.StartDate = null;
                    newCategory.EndDate = null;
                }

                newCategory.CreatedById = _userManager.GetUserId(User);
                newCategory.UpdatedById = _userManager.GetUserId(User);

                // Change the string of Category Name to uppercase
                newCategory.CatName.ToUpper();

                Database.Categories.Add(newCategory);
                Database.SaveChanges();//Telling the database model to save the changes
            }
            catch (Exception exceptionObject)
            {
                if (exceptionObject.InnerException.Message
                          .Contains("Category_CatName_UniqueConstraint") == true)
                {
                    customMessage = "Unable to save Category record due " +
                                  "to another record having the same name as : " +
                                  categoryNewInput.CatName.Value;
                    //Create a fail message anonymous object that has one property, Message.
                    //This anonymous object's Message property contains a simple string message
                    object httpFailRequestResultMessage = new { Message = customMessage };
                    //Return a bad http request message to the client
                    return BadRequest(httpFailRequestResultMessage);
                }
            }//End of Try..Catch block

            //If there is no runtime error in the try catch block, the code execution
            //should reach here. Sending success message back to the client.

            //******************************************************
            //Construct a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Saved Category record"
            };

            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                        new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            string customMessage = "";
            string format = "dd/MM/yyyy";
            var categoryChangeInput = JsonConvert.DeserializeObject<dynamic>(value);
            //To obtain the category name information:
            //use categoryChangeInput.categoryName.value
            //To obtain the address information:
            //use categoryChangeInput.Address.value
            try
            {
                //Find the category Entity through the Categories Entity Set
                //by calling the Single() method.
                //I learnt Single() method from this online reference:
                //http://geekswithblogs.net/BlackRabbitCoder/archive/2011/04/14/c.net-little-wonders-first-and-single---similar-yet-different.aspx
                var foundOneCategory = Database.Categories
                                    .Single(item => item.CatId == id);
                foundOneCategory.CatName = categoryChangeInput.CatName;
                foundOneCategory.VisibilityId = categoryChangeInput.VisibilityId;
                if (foundOneCategory.VisibilityId == 2)
                {
                    foundOneCategory.StartDate = DateTime.ParseExact(categoryChangeInput.StartDate.Value, format, System.Globalization.CultureInfo.InvariantCulture);
                    foundOneCategory.EndDate = DateTime.ParseExact(categoryChangeInput.EndDate.Value, format, System.Globalization.CultureInfo.InvariantCulture);
                    // The one big reason why I implemented server-sided validation.
                    // http://net-informations.com/faq/asp/validation.htm
                    //
                    // Let's compare the two dates to make sure the StartDate is later than EndDate
                    if (DateTime.Compare(foundOneCategory.StartDate.Value, foundOneCategory.EndDate.Value) > 0)
                    {
                        // But if it is later, we'll toss them an error for SweetAlert to throw out.
                        customMessage = "Please enter your start date that is before your end date.";
                        object httpFailRequestResultMessage = new { Message = customMessage };
                        // Return a bad http request message to the client
                        // Good job to the user who's trolling with me, good try
                        return BadRequest(httpFailRequestResultMessage);
                    }
                }
                else
                {
                    foundOneCategory.StartDate = null;
                    foundOneCategory.EndDate = null;
                }


                // Turn the category name input to upper case
                foundOneCategory.CatName.ToUpper();

                // Not needed, we don't have to modify the id.
                // foundOneCategory.CatId = Int32.Parse(categoryChangeInput.CatId.Value);
                foundOneCategory.UpdatedAt = DateTime.Now;
                foundOneCategory.UpdatedById = _userManager.GetUserId(User);

                //Tell the database model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message
                     .Contains("Category_CatName_UniqueConstraint") == true)
                {
                    customMessage = "Unable to save Category record due " +
                                      "to another record having the same name as : " +
                    categoryChangeInput.CatName.Value;
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
                Message = "The amazing category record has been saved!"
            };

            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                   new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;
        }

        // PUT api/values/5
        [HttpPut("Restore/{id}")]
        public IActionResult Restore(int id, [FromBody]string value)
        {
            string customMessage = "";
            var categoryChangeInput = JsonConvert.DeserializeObject<dynamic>(value);
            try
            {
                //Find the category Entity through the Categories Entity Set
                //by calling the Single() method.
                //I learnt Single() method from this online reference:
                //http://geekswithblogs.net/BlackRabbitCoder/archive/2011/04/14/c.net-little-wonders-first-and-single---similar-yet-different.aspx
                var foundOneCategory = Database.Categories
                                    .Single(item => item.CatId == id);
                // No validation checks required, no user provided data.
                foundOneCategory.DeletedAt = null;
                foundOneCategory.DeletedById = null;
                foundOneCategory.UpdatedAt = DateTime.Now;
                foundOneCategory.UpdatedById = _userManager.GetUserId(User);
                //Tell the database model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message
                     .Contains("Category_CatName_UniqueConstraint") == true)
                {
                    customMessage = "Unable to Restore Category record due " +
                                      "to another record having the same name as : " +
                    categoryChangeInput.CatName.Value;
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
                Message = "The category has been restored to glory!"
            };

            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                   new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string customMessage = "";

            //The following command should not be used. Although can work too.
            // var existingOneCategory = Database.Categories
            //    .Where(item => item.CategoryId == id).FirstOrDefault();
            //--------------------------------------------------------------------------------------------
            try
            {
                var foundOneProductPhoto = Database.ProductPhotos
                           .Single(item => item.ProductPhotoId == id);
                foundOneProductPhoto.DeletedAt = DateTime.Now;
                foundOneProductPhoto.DeletedById = _userManager.GetUserId(User);

                //Update the database model
                Database.Update(foundOneProductPhoto);
                //Tell the db model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                customMessage = "Unable to delete Product Photo record.";
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of try .. catch block on manage data

            //Build a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Your Product Image has been deleted!"
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
