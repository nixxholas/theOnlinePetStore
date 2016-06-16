using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using WEBACA.Models;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using WEBACA.Helper;
using Newtonsoft.Json;
using Microsoft.Data.Entity;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WEBACA.APIs
{
    [Route("api/[controller]")]
    public class BrandsController : Controller

    {//Every Web API controller class need to have an ApplicationDbContext
     //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }

        public BrandsController()
        {
            Database = new ApplicationDbContext();
        }

        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            List<object> brandList = new List<object>();
            var brands = Database.Brands
            .Where(eachBrandEntity => eachBrandEntity.DeletedAt == null)
            .Include(eachBrandEntity => eachBrandEntity.BrandCategory)
            .Include(eachBrandEntity => eachBrandEntity.BrandPhoto).AsNoTracking();

            //After obtaining all the Employee entity rows (records) from the database,
            //the employees variable will become an container holding these 
            //Employee class entity rows.
            //I need to loop through each  Employee instance inside employees
            //to construct a List container of anonymous objects (which has 6 properties).
            //Then use the new JsonResult(employeeList) technique to generate the
            //JSON formatted string data which can be sent back to the web browser client.
            foreach (var oneBrand in brands)
            {
                brandList.Add(new
                {
                    BrandId = oneBrand.BrandId,
                    BrandName = oneBrand.BrandName,
                    PhotoUrl = oneBrand.BrandPhoto.Url,
                    NoOfProducts = oneBrand.NoOfProducts,
                    CreatedAt = oneBrand.CreatedAt,
                    UpdatedAt = oneBrand.UpdatedAt
                });
            }//end of foreach loop which builds the employeeList .
            return new JsonResult(brandList);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var foundBrand = Database.Brands
                     .Where(eachBrand => eachBrand.BrandId == id)
                     .Include(eachBrand => eachBrand.BrandPhoto).Single();
                var response = new
                {
                    BrandId = foundBrand.BrandId,
                    BrandName = foundBrand.BrandName,
                    NoOfProducts = foundBrand.NoOfProducts,
                    PhotoUrl = foundBrand.BrandPhoto.Url,
                    PhotoPublicId = foundBrand.BrandPhoto.PublicCloudinaryId
                };//end of creation of the response object
                return new JsonResult(response);
            }
            catch (Exception exceptionObject)
            {
                //Create a fail message anonymous object
                //This anonymous object only has one Message property 
                //which contains a simple string message
                object httpFailRequestResultMessage =
                                    new { Message = "Unable to obtain employee information." };
                //Return a bad http response message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //You are making a soft delete (not hard delete).
            //Therefore, the Employee binary photo image at Cloudinary will remain.
            string customMessage = "";
            try
            {
                var foundOneBrand = Database.Brands
                                     .Single(item => item.BrandId == id);
                foundOneBrand.DeletedAt = DateTime.Now;
                //Update the database model
                Database.Update(foundOneBrand);
                //Tell the db model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                customMessage = "Unable to delete employee record.";
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }//End of try .. catch block on manage data

            //Build a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Deleted Brand record"
            };

            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            HttpOkObjectResult httpOkResult =
                                                            new HttpOkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;
        }//end of Delete(id) Web API method with /API/Employees/digit URL pattern route

        // POST /api/Employees/SaveNewEmployeeInformationInSession
        [HttpPost("SaveNewBrandInformationInSession")]
        public IActionResult SaveNewBrandInformationInSession([FromBody]string value)
        {
            string customMessage = "";
            //Reconstruct a useful object from the input string value. 
            var brandNewInput = JsonConvert.DeserializeObject<dynamic>(value);

            Brands newBrand = new Brands();
            try
            {
                //Copy out all the employee data into the new Brand instance,
                //newBrand.
                newBrand.BrandName = brandNewInput.BrandName.Value;
                newBrand.BrandCategory = new List<BrandCategory>();

                var categories = brandNewInput.BrandCategories.Value;
                categories = categories.TrimEnd(']');
                categories = categories.TrimStart('[');

                foreach (string catId in categories.Split(','))
                {
                    int CatId = Int32.Parse(catId);

                    // Create the necessary object to store the composites
                    BrandCategory newBrandCategory = new BrandCategory();
                    newBrandCategory.BrandId = newBrand.BrandId;
                    newBrandCategory.CatId = CatId;
                    newBrand.BrandCategory.Add(newBrandCategory);
                }



                //I cannot save the employee information into database yet. The 
                //UploadBrandPhotoAndSaveEmployeeData has logic to upload the binary file to
                //Cloudinary first, then it will save employee data into the database.
                //Therefore, I need to save the employee data as a Session variable first.
                //The command below will save the employee data inside a
                //Session variable, Employee (I can use other names...it is just a name)
                HttpContext.Session.SetObjectAsJson("Brands", newBrand);
            }
            catch (Exception exceptionObject)
            {
                customMessage = "Unable to save employee into session.";
                //Create a fail message anonymous object that has one property, Message.
                //This anonymous object's Message property contains a simple string message
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }//End of Try..Catch block
             //If there is no runtime error in the try catch block, the code execution
             //should reach here. Sending success message back to the client.

            //******************************************************
            //Construct a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Saved employee into session"
            };

            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            HttpOkObjectResult httpOkResult =
                                        new HttpOkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;

        }//End of SaveNewEmployeeInformationInSession() method

        // PUT /api/Employees/SaveEmployeeUpdateInformationIntoDatabase
        [HttpPut("SaveBrandUpdateInformationIntoDatabase/{id}")]
        public IActionResult SaveBrandUpdateInformationIntoDatabase(int id, [FromBody]string value)
        {
            string customMessage = "";
            //Reconstruct a useful object from the input string value. 
            var brandChangeInput = JsonConvert.DeserializeObject<dynamic>(value);

            Brands brandToBeUpdated = new Brands();
            try
            {
                brandToBeUpdated.BrandName = brandChangeInput.BrandName.Value;
                brandToBeUpdated.BrandCategory = new List<BrandCategory>();

                var categories = brandChangeInput.BrandCategories.Value;
                categories = categories.TrimEnd(']');
                categories = categories.TrimStart('[');

                foreach (string catId in categories.Split(','))
                {
                    int CatId = Int32.Parse(catId);

                    // Create the necessary object to store the composites
                    BrandCategory newBrandCategory = new BrandCategory();
                    newBrandCategory.BrandId = id;
                    newBrandCategory.CatId = CatId;
                    brandToBeUpdated.BrandCategory.Add(newBrandCategory);
                }

                var foundOneBrand = Database.Brands
                        .Where(eachBrand => eachBrand.BrandId == id)
                        .Include(eachBrand => eachBrand.BrandCategory)
                        .Single();

                // For loop to delete deleted entries
                foreach (BrandCategory bc in foundOneBrand.BrandCategory)
                {
                    bool exists = false;
                    foreach (BrandCategory UpdatedBC in brandToBeUpdated.BrandCategory)
                    {
                        // Let BTBU be brandToBeUpdated
                        // if the brandcategory is found in BTBU.BrandCategory
                        if (bc.CatId == UpdatedBC.CatId && bc.BrandId == id)
                        {
                            // Do no shit
                            exists = true;
                            break;
                        }
                    }
                    if (exists == false && bc.DeletedAt == null)
                    {
                        bc.DeletedAt = DateTime.Now;
                    }
                }

                // For loop to add entries
                // Basically this creates new rows
                // If the new brandcategory does not exist, create a new row
                // if it already exists, set its deletedAt property to null
                foreach (BrandCategory UpdatedBC in brandToBeUpdated.BrandCategory)
                {
                    bool exists = false;
                    foreach (BrandCategory bc in foundOneBrand.BrandCategory)
                    {
                        // If a brandcategory is found in updated bc, we make sure its
                        // DeletedAt is null to make sure it is not marked as deleted.
                        if (bc.CatId == UpdatedBC.CatId && bc.BrandId == id)
                        {
                            bc.DeletedAt = null;
                            exists = true;
                            break;
                        } 
                    }
                    // If an UpdatedBC does not exist in foundOneBrand,
                    // We need to add it in
                    if (exists == false && UpdatedBC.BrandId == foundOneBrand.BrandId)
                    {
                        foundOneBrand.BrandCategory.Add(UpdatedBC);
                    }
                }

                foundOneBrand.BrandName = brandToBeUpdated.BrandName;
                foundOneBrand.UpdatedAt = DateTime.Now;
                Database.Brands.Update(foundOneBrand);
                Database.SaveChanges();//Without this command, the changes are not committed.
                customMessage = "Saved brand into database.";
            }
            catch (Exception exceptionObject)
            {
                customMessage = "Unable to save brand into database.";
                //Create a fail message anonymous object that has one property, Message.
                //This anonymous object's Message property contains a simple string message
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }//End of Try..Catch block

            //If there is no runtime error in the try catch block, the code execution
            //should reach here. Sending success message back to the client.
            //******************************************************
            //Construct a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = customMessage
            };
            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            HttpOkObjectResult httpOkResult =
                                                                    new HttpOkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;
        }//End of SaveEmployeeUpdateInformationIntoDatabase() method

        // PUT /api/Employees/SaveEmployeeUpdateInformationIntoSession
        [HttpPut("SaveBrandUpdateInformationIntoSession/{id}")]
        public IActionResult SaveBrandUpdateInformationIntoSession(int id, [FromBody]string value)
        {

            string customMessage = "";
            //Reconstruct a useful object from the input string value. 
            var brandChangeInput = JsonConvert.DeserializeObject<dynamic>(value);

            Brands brandToBeUpdated = new Brands();
            try
            {
                //Copy out all the employee data into the new Brand instance,
                //newBrand.
                brandToBeUpdated.BrandId = id;
                brandToBeUpdated.BrandName = brandChangeInput.BrandName.Value;
                brandToBeUpdated.BrandCategory = new List<BrandCategory>();

                var categories = brandChangeInput.BrandCategories.Value;
                categories = categories.TrimEnd(']');
                categories = categories.TrimStart('[');

                foreach (string catId in categories.Split(','))
                {
                    int CatId = Int32.Parse(catId);

                    // Create the necessary object to store the composites
                    BrandCategory newBrandCategory = new BrandCategory();
                    newBrandCategory.BrandId = id;
                    newBrandCategory.CatId = CatId;
                    brandToBeUpdated.BrandCategory.Add(newBrandCategory);
                }

                //Saved it into a Session variable
                HttpContext.Session.SetObjectAsJson("Brands", brandToBeUpdated);
                customMessage = "Saved brand into session";
            }
            catch (Exception exceptionObject)
            {
                customMessage = "Unable to save brand into database.";
                //Create a fail message anonymous object that has one property, Message.
                //This anonymous object's Message property contains a simple string message
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }//End of Try..Catch block

            //If there is no runtime error in the try catch block, the code execution
            //should reach here. Sending success message back to the client.
            //******************************************************
            //Construct a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = customMessage
            };
            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            HttpOkObjectResult httpOkResult =
                            new HttpOkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;
        }//End of SaveEmployeeUpdateInformationIntoSession() method

        //POST /Api/Employees/UploadEmployeePhotoAndUpdateEmployeeData
        [HttpPost("UploadBrandPhotoAndUpdateBrandData")]
        public async Task<IActionResult> UploadBrandPhotoAndUpdateBrandData(IList<IFormFile> fileInput)
        {
            //Retrieve the employee data which is stashed inside the Session, "Employee".
            //http://benjii.me/2015/07/using-sessions-and-httpcontext-in-aspnet5-and-mvc6/
            Brands brandToBeUpdated = HttpContext.Session.GetObjectFromJson<Brands>("Brands");
            //Get the current employee data from the database.
            //Also get the current employee Photo information.
            var oneBrand = Database.Brands
                .Where(Brand => Brand.BrandId == brandToBeUpdated.BrandId)
                .Include(Brand => Brand.BrandCategory)
                .Include(Brand => Brand.BrandPhoto).Single();
            oneBrand.BrandName = brandToBeUpdated.BrandName;

            // For loop to delete deleted entries
            foreach (BrandCategory bc in oneBrand.BrandCategory)
            {
                bool exists = false;
                foreach (BrandCategory UpdatedBC in brandToBeUpdated.BrandCategory)
                {
                    // Let BTBU be brandToBeUpdated
                    // if the brandcategory is found in BTBU.BrandCategory
                    if (bc.CatId == UpdatedBC.CatId && bc.BrandId == brandToBeUpdated.BrandId)
                    {
                        // Do no shit
                        exists = true;
                        break;
                    }
                }
                if (exists == false && bc.DeletedAt == null)
                {
                    bc.DeletedAt = DateTime.Now;
                }
            }

            // For loop to add entries
            // Basically this creates new rows
            // If the new brandcategory does not exist, create a new row
            // if it already exists, set its deletedAt property to null
            foreach (BrandCategory UpdatedBC in brandToBeUpdated.BrandCategory)
            {
                bool exists = false;
                foreach (BrandCategory bc in oneBrand.BrandCategory)
                {
                    // If a brandcategory is found in updated bc, we make sure its
                    // DeletedAt is null to make sure it is not marked as deleted.
                    if (bc.CatId == UpdatedBC.CatId && bc.BrandId == brandToBeUpdated.BrandId)
                    {
                        bc.DeletedAt = null;
                        exists = true;
                        break;
                    }
                }
                // If an UpdatedBC does not exist in foundOneBrand,
                // We need to add it in
                if (exists == false && UpdatedBC.BrandId == oneBrand.BrandId)
                {
                    oneBrand.BrandCategory.Add(UpdatedBC);
                }
            }

            var oneFile = fileInput[0];
            var fileName = ContentDispositionHeaderValue
                        .Parse(oneFile.ContentDisposition)
                        .FileName
                        .Trim('"');
            string contentType = oneFile.ContentType;
            //Upload the binary file first
            var currentBrandPhoto = await Cloudinary.CloudinaryAPIs.UploadBrandImageToCloudinary(oneFile.OpenReadStream(), contentType, fileName, "Employees");
            //Delete the existing binary file
            //Obtain the Cloudinary public id value from the foundOneEmployee's EmployeePhoto navigation property
            string originalCloudinaryPublicId = oneBrand.BrandPhoto.PublicCloudinaryId;
            //Use the Cloudinary public id value as an input argument for the DeleteImageInCloudinary to delete the binary
            //file resource.
            Boolean result = await Cloudinary.CloudinaryAPIs.DeleteImageInCloudinary(originalCloudinaryPublicId);

            if (currentBrandPhoto.PublicCloudinaryId != "")
            {
                oneBrand.BrandPhoto.ImageSize = currentBrandPhoto.ImageSize;
                oneBrand.BrandPhoto.Version = currentBrandPhoto.Version;
                oneBrand.BrandPhoto.Height = currentBrandPhoto.Height;
                oneBrand.BrandPhoto.Width = currentBrandPhoto.Width;
                oneBrand.BrandPhoto.PublicCloudinaryId = currentBrandPhoto.PublicCloudinaryId;
                oneBrand.BrandPhoto.Url = currentBrandPhoto.Url;
                oneBrand.BrandPhoto.SecureUrl = currentBrandPhoto.SecureUrl;
            }

            Database.Brands.Update(oneBrand);
            Database.SaveChanges();
            var successRequestResultMessage = new
            {
                Message = "Saved brand.",
                NewImageUrl = oneBrand.BrandPhoto.Url
            };
            HttpOkObjectResult httpOkResult =
                                        new HttpOkObjectResult(successRequestResultMessage);
            return httpOkResult;
        }//End of UploadEmployeePhotoAndUpdateEmployeeData()


        //POST /Api/Brands/UploadBrandPhotoAndSaveBrandData
        [HttpPost("UploadBrandPhotoAndSaveBrandData")]
        public async Task<IActionResult> UploadBrandPhotoAndSaveBrandData(IList<IFormFile> fileInput)
        {
            var oneFile = fileInput[0];
            BrandPhoto newBrandPhoto;
            var fileName = ContentDispositionHeaderValue
                  .Parse(oneFile.ContentDisposition)
                  .FileName
                  .Trim('"');
            string contentType = oneFile.ContentType;
            newBrandPhoto = await Cloudinary.CloudinaryAPIs.UploadBrandImageToCloudinary(oneFile.OpenReadStream(), contentType, fileName, "Brands");

            //Retrieve the new employee data which is stashed inside the Session, "Brand".
            Brands newBrand = HttpContext.Session.GetObjectFromJson<Brands>("Brands");
            if (newBrandPhoto.PublicCloudinaryId != "")
            {
                //Add the employ record first, so that the newBrand
                //object's BrandId property is updated with the new record's
                //id.
                Database.Brands.Add(newBrand);
                //Copy over the new employee id information to the BrandPhoto object,
                //newBrandPhoto. 
                newBrandPhoto.BrandId = newBrand.BrandId;
                Database.BrandPhotos.Add(newBrandPhoto);
                Database.Brands.Add(newBrand);
                Database.SaveChanges();
            }
            var successRequestResultMessage = new
            {
                Message = "Saved Brand"
            };

            HttpOkObjectResult httpOkResult =
                new HttpOkObjectResult(successRequestResultMessage);
            return httpOkResult;

        }//End of UploadBrandPhotoAndSaveBrandData()

    }
}
