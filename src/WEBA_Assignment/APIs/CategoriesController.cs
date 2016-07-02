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

namespace WEBA_ASSIGNMENT.APIs
{
    [Authorize("RequireAdminRole")]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        //Every Web API controller class need to have an ApplicationDbContext
        //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }
        public CategoriesController()
        {
            Database = new ApplicationDbContext();
        }

        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            List<object> categoryList = new List<object>();
            var categories = Database.Categories
                 .Where(category => category.DeletedAt == null)
                 .Include(input => input.Visibility);
            foreach (var category in categories)
            {
                categoryList.Add(new
                {
                    CatId = category.CatId,
                    CatName = category.CatName,
                    VisibilityId = category.VisibilityId,
                    Visibility = category.Visibility.VisibilityName,
                    Brands = category.BrandCategory,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                    DeletedAt = category.DeletedAt
                });
            }//end of foreach loop which builds the categoryList .
            return new JsonResult(categoryList);
        }

        // GET BrandsUnderCategory
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var foundCategory = Database.Categories
                     .Where(item => item.CatId == id).Single();
                var response = new
                {
                    CatId = foundCategory.CatId,
                    CatName = foundCategory.CatName,
                    VisibilityId = foundCategory.VisibilityId,
                    StartDate = foundCategory.StartDate,
                    EndDate = foundCategory.EndDate,
                    CreatedAt = foundCategory.CreatedAt,
                    UpdatedAt = foundCategory.UpdatedAt
                };//end of creation of the response object
                return new JsonResult(response);
            }
            catch (Exception exceptionObject)
            {
                //Create a fail message anonymous object
                //This anonymous object only has one Message property 
                //which contains a simple string message
                object httpFailRequestResultMessage =
                                    new { Message = "Unable to obtain Category information." };
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
                if (newCategory.VisibilityId == 3)
                {
                    newCategory.StartDate = DateTime.ParseExact(categoryNewInput.StartDate.Value, format, System.Globalization.CultureInfo.InvariantCulture);
                    newCategory.EndDate = DateTime.ParseExact(categoryNewInput.EndDate.Value, format, System.Globalization.CultureInfo.InvariantCulture);
                } else
                {
                    newCategory.StartDate = null;
                    newCategory.EndDate = null;
                }

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
                if (foundOneCategory.VisibilityId == 3)
                {
                    foundOneCategory.StartDate = DateTime.ParseExact(categoryChangeInput.StartDate.Value, format, System.Globalization.CultureInfo.InvariantCulture);
                    foundOneCategory.EndDate = DateTime.ParseExact(categoryChangeInput.EndDate.Value, format, System.Globalization.CultureInfo.InvariantCulture);
                    if (DateTime.Compare(foundOneCategory.StartDate.Value, foundOneCategory.EndDate.Value) > 0)
                    {
                        customMessage = "Please enter your start date that is before your end date.";
                        object httpFailRequestResultMessage = new { Message = customMessage };
                        //Return a bad http request message to the client
                        return BadRequest(httpFailRequestResultMessage);
                    }
                } else
                {
                    foundOneCategory.StartDate = null;
                    foundOneCategory.EndDate = null;
                }

                // Turn the category name input to upper case
                foundOneCategory.CatName.ToUpper();

                // Not needed, we don't have to modify the id.
                // foundOneCategory.CatId = Int32.Parse(categoryChangeInput.CatId.Value);
                foundOneCategory.UpdatedAt = DateTime.Now;

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
                foundOneCategory.DeletedAt = null;
                foundOneCategory.UpdatedAt = DateTime.Now;
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
                var foundOneCategory = Database.Categories
                           .Single(item => item.CatId == id);
                foundOneCategory.DeletedAt = DateTime.Now;

                //Update the database model
                Database.Update(foundOneCategory);
                //Tell the db model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                customMessage = "Unable to delete category record.";
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of try .. catch block on manage data

            //Build a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Deleted category record"
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
