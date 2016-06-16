using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBACA.Models;

namespace WEBACA.APIs
{
    [Route("api/[controller]")]
    public class BrandCategoryController : Controller
    {
        public ApplicationDbContext Database { get; }

        public BrandCategoryController()
        {
            Database = new ApplicationDbContext();
        }

        [HttpGet]
        public JsonResult Get()
        {
            List<object> brandCategoryList = new List<object>();
            var brandCategories = Database.BrandCategory
                .Where(eachBrandCategoryEntity => eachBrandCategoryEntity.DeletedAt != null)
            .Include(eachBrandCategoryEntity => eachBrandCategoryEntity.Brand)
            .Include(eachBrandCategoryEntity => eachBrandCategoryEntity.Category);

            //After obtaining all the Employee entity rows (records) from the database,
            //the employees variable will become an container holding these 
            //Employee class entity rows.
            //I need to loop through each  Employee instance inside employees
            //to construct a List container of anonymous objects (which has 6 properties).
            //Then use the new JsonResult(employeeList) technique to generate the
            //JSON formatted string data which can be sent back to the web browser client.
            foreach (var oneCategoryBrand in brandCategories)
            {
                brandCategoryList.Add(new
                {
                    BrandId = oneCategoryBrand.BrandId,
                    BrandName = oneCategoryBrand.Brand.BrandId,
                    CatId = oneCategoryBrand.CatId,
                    CatName = oneCategoryBrand.Category.CatName
                });
            }//end of foreach loop which builds the employeeList .
            return new JsonResult(brandCategoryList);
        }

        // GET: api/values
        [HttpGet("GetCatIds/{id}")]
        public IActionResult GetCatIds(int id)
        {
            try
            {
                var categoryIds = "";
                var foundBrandCategories = Database.BrandCategory
                     .Where(eachBrandCategory => eachBrandCategory.BrandId == id)
                     .Include(eachBrandCategory => eachBrandCategory.Category)
                     .Include(eachBrandCategory => eachBrandCategory.Brand);
                foreach (var brandCategory in foundBrandCategories)
                {
                    categoryIds += brandCategory.Category.CatName;
                    categoryIds += ", ";
                    //need to trim the end 
                }//end of foreach loop which builds the categoryList .
                return new JsonResult(categoryIds);
            }
            catch (Exception exceptionObject)
            {
                //Create a fail message anonymous object
                //This anonymous object only has one Message property 
                //which contains a simple string message
                object httpFailRequestResultMessage =
                                    new { Message = "Unable to obtain Brand Category information." };
                //Return a bad http response message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }
        }

        // We're parsing in the integer for the BrandId, gotta
        // find something that can detect either if its a BrandId
        // or CatId..
        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                List<object> brandCategoryList = new List<object>();
                var foundBrandCategories = Database.BrandCategory
                     .Where(eachBrandCategory => eachBrandCategory.BrandId == id)
                     .Include(eachBrandCategory => eachBrandCategory.Category)
                     .Include(eachBrandCategory => eachBrandCategory.Brand);
                foreach (var brandCategory in foundBrandCategories)
                {
                    brandCategoryList.Add(new
                    {
                        BrandId = brandCategory.BrandId,
                        CatId = brandCategory.CatId,
                        CatName = brandCategory.Category.CatName,
                        BrandName = brandCategory.Brand.BrandName
                    });
                }//end of foreach loop which builds the categoryList .
                return new JsonResult(brandCategoryList);
            }
            catch (Exception exceptionObject)
            {
                //Create a fail message anonymous object
                //This anonymous object only has one Message property 
                //which contains a simple string message
                object httpFailRequestResultMessage =
                                    new { Message = "Unable to obtain Brand Category information." };
                //Return a bad http response message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }
        }

        /**
         * This POST method takes in the web form data
         * and parses in the Brand Object and the Category Object together.
         */
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            string customMessage = "";
            string format = "dd/MM/yyyy";
            //Reconstruct a useful object from the input string value. 
            dynamic brandCategoryNewInput = JsonConvert.DeserializeObject<dynamic>(value);


            BrandCategory newBrandCategory = new BrandCategory();
            try
            {
                //Copy out all the category data into the new category instance,
                //newcategory.
                newBrandCategory.BrandId = Int32.Parse(brandCategoryNewInput.Brand.BrandId.Value);
                newBrandCategory.CatId = Int32.Parse(brandCategoryNewInput.Category.CatId.Value);
                Database.BrandCategory.Add(newBrandCategory);
                Database.SaveChanges();//Telling the database model to save the changes
            }
            catch (Exception exceptionObject)
            {
                if (exceptionObject.InnerException.Message
                          .Contains("BrandsOfCategories_CompositeKey") == true)
                {
                    customMessage = "Unable to save Brand of Category record due " +
                                  "to another record having the same name as : " +
                                  brandCategoryNewInput.Category.CatName.Value;
                    //Create a fail message anonymous object that has one property, Message.
                    //This anonymous object's Message property contains a simple string message
                    object httpFailRequestResultMessage = new { Message = customMessage };
                    //Return a bad http request message to the client
                    return HttpBadRequest(httpFailRequestResultMessage);
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

            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            HttpOkObjectResult httpOkResult =
                        new HttpOkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;
        }

        // Takes in an ID of the brand and updates it's records accordingly.
        // PUT api/values/5
        [HttpPut("SaveBrandCategoryUpdateInformationIntoDatabase/{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            string customMessage = "";
            string format = "dd/MM/yyyy";
            var brandCategoryChangeInput = JsonConvert.DeserializeObject<dynamic>(value);

            BrandCategory brandCategoryToBeUpdated = new BrandCategory();
            try
            {
                brandCategoryToBeUpdated.BrandId = id;
                // Reference code from MSDN
                //// Query for all blogs with names starting with B 
                //var blogs = from b in context.Blogs
                //            where b.Name.StartsWith("B")
                //            select b;

                //// Query for the Blog named ADO.NET Blog 
                //var blog = context.Blogs
                //                .Where(b => b.Name == "ADO.NET Blog")
                //                .FirstOrDefault();

                //Find the category Entity through the Categories Entity Set
                //by calling the Single() method.
                //I learnt Single() method from this online reference:
                //http://geekswithblogs.net/BlackRabbitCoder/archive/2011/04/14/c.net-little-wonders-first-and-single---similar-yet-different.aspx
                var foundBrandCategories = Database.BrandCategory
                                    .Where(item => item.BrandId == id);

                Console.WriteLine(foundBrandCategories);

                // We need to make a method where the brandcategories that were pulled
                // will be able to run checks with the user inputs for the categories.

                // We need to delete it if it doesn't exist anymore

                ///**
                // * For each brand category in foundBrandCategories
                // * 
                // * if brandCategoryChangeInput does not have it, delete it
                // * 
                // * for each brand category brandCategoryChangeInput
                // * 
                // * if foundBrandCategories has it, add it
                // * 
                // * /

                // We also need to add it if something doesn't exist now exists


                //Tell the database model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message
                     .Contains("BrandsOfCategories_CompositeKey") == true)
                {
                    customMessage = "Unable to save Brand Category record due " +
                                      "to another record having the same name as : " +
                    brandCategoryChangeInput.CatName.Value;
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
                Message = "Saved Category record"
            };

            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            HttpOkObjectResult httpOkResult =
               new HttpOkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
