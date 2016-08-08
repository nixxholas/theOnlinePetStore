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
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WEBA_ASSIGNMENT.Helper;
using Microsoft.AspNetCore.Identity;
using WEBA_ASSIGNMENT.Services;
using Microsoft.Extensions.Logging;
using WEBA_ASSIGNMENT.Controllers;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WEBA_ASSIGNMENT.APIs
{
    [Authorize(Roles = "SUPER ADMIN,ADMIN")]
    [Route("api/[controller]")]
    public class BrandsController : Controller

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        //Every Web API controller class need to have an ApplicationDbContext
        //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }

        public BrandsController(UserManager<ApplicationUser> userManager,
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
            List<object> brandList = new List<object>();
            var brands = Database.Brands
            //.Where(eachBrandEntity => eachBrandEntity.DeletedAt == null)
            .Include(eachBrandEntity => eachBrandEntity.BrandCategory)
            // Prepare for Products counting
            .Include(eachBrandEntity => eachBrandEntity.Products)
            .Include(eachBrandEntity => eachBrandEntity.BrandPhoto)
            .OrderBy(eachBrandEntity => eachBrandEntity.BrandName).AsNoTracking();

            //After obtaining all the Brand entity rows (records) from the database,
            //the brandss variable will become an container holding these 
            //Brand class entity rows.
            //I need to loop through each  Brand instance inside brandss
            //to construct a List container of anonymous objects (which has 6 properties).
            //Then use the new JsonResult(brandsList) technique to generate the
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
            }//end of foreach loop which builds the brandsList .
            return new JsonResult(brandList);
        }


        // GET BrandsUnderCategory
        // Takes in an integer as a Category Id
        [HttpGet("GetBrandsUnderCategory/{id}")]
        public JsonResult BrandsUnderCategory(int id)
        {
            List<object> brandList = new List<object>();

            var brands = Database.Brands
            .Include(eachBrandEntity => eachBrandEntity.BrandCategory)
            .Include(eachBrandEntity => eachBrandEntity.BrandPhoto).AsNoTracking();

            foreach (var oneBrand in brands)
            {
                foreach (var brandCat in oneBrand.BrandCategory)
                {
                    if (brandCat.CatId == id)
                    {
                        brandList.Add(new
                        {
                            BrandId = oneBrand.BrandId,
                            BrandName = oneBrand.BrandName,
                            // Save this for later
                            // PhotoUrl = oneBrand.BrandPhoto.Url,
                            NoOfProducts = oneBrand.NoOfProducts,
                            CreatedAt = oneBrand.CreatedAt,
                            UpdatedAt = oneBrand.UpdatedAt
                        });
                    }
                }
            }//end of foreach loop which builds the brandsList .
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
                                    new { Message = "Unable to obtain Category information." };
                //Return a bad http response message to the client
                return BadRequest(httpFailRequestResultMessage);
            }
        }
        
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //You are making a soft delete (not hard delete).
            //Therefore, the Brand binary photo image at Cloudinary will remain.
            string customMessage = "";
            try
            {
                var foundOneBrand = Database.Brands
                                     .Include(item => item.CreatedBy)
                                     .Single(item => item.BrandId == id);
                foundOneBrand.DeletedAt = DateTime.Now;
                foundOneBrand.DeletedById = _userManager.GetUserId(User);
                //Update the database model
                Database.Update(foundOneBrand);
                //Tell the db model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                customMessage = "Unable to delete brands record.";
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of try .. catch block on manage data

            //Build a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Deleted Brand record"
            };

            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                                                            new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;
        }//end of Delete(id) Web API method with /API/Brands/digit URL pattern route

        // POST /api/Brands/SaveNewBrandInformationInSession
        [HttpPost("SaveNewBrandInformationInSession")]
        public IActionResult SaveNewBrandInformationInSession([FromBody]string value)
        {
            string customMessage = "";
            // Issue: Should I add a "'" into a the Brand name String, the received from the
            // Client results in an unended json object..
            //Reconstruct a useful object from the input string value. 
            var brandNewInput = JsonConvert.DeserializeObject<dynamic>(value);

            Brands newBrand = new Brands();
            try
            {
                //Copy out all the brands data into the new Brand instance,
                //newBrand.
                newBrand.BrandName = brandNewInput.BrandName.Value;
                newBrand.BrandCategory = new List<BrandCategory>();
                newBrand.CreatedById = _userManager.GetUserId(User);
                newBrand.UpdatedById = _userManager.GetUserId(User);

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

                //I cannot save the brands information into database yet. The 
                //UploadBrandPhotoAndSaveBrandData has logic to upload the binary file to
                //Cloudinary first, then it will save brands data into the database.
                //Therefore, I need to save the brands data as a Session variable first.
                //The command below will save the brand data inside a
                //Session variable, Brand (I can use other names...it is just a name)
                HttpContext.Session.SetObjectAsJson("Brands", newBrand);
            }
            catch (Exception exceptionObject)
            {
                customMessage = "Unable to save brand into session.";
                //Create a fail message anonymous object that has one property, Message.
                //This anonymous object's Message property contains a simple string message
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of Try..Catch block
             //If there is no runtime error in the try catch block, the code execution
             //should reach here. Sending success message back to the client.

            //******************************************************
            //Construct a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Saved brand into session"
            };

            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                                        new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;

        }//End of SaveNewBrandInformationInSession() method

        // POST /api/Brands/SaveNewBrandInformationInSession
        [HttpPost("SaveBrandData")]
        public IActionResult SaveBrandDataInDatabase([FromBody]string value)
        {
            string customMessage = "";
            // Issue: Should I add a "'" into a the Brand name String, the received from the
            // Client results in an unended json object..
            //Reconstruct a useful object from the input string value. 
            Brands newBrand = HttpContext.Session.GetObjectFromJson<Brands>("Brands");

            try
            {
                newBrand.BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 120,
                    ImageSize = 1692,
                    PublicCloudinaryId = "Brands/u0ofsgsn9b1q6tlwrkxg",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466794773/Brands/u0ofsgsn9b1q6tlwrkxg.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466794773/Brands/u0ofsgsn9b1q6tlwrkxg.jpg",
                    Version = 1466794773,
                    Width = 171
                };
                //Add the brand record first, so that the newBrand
                //object's BrandId property is updated with the new record's
                //id.
                Database.Brands.Add(newBrand);
                Database.SaveChanges();


                //******************************************************
                //Construct a custom message for the client
                //Create a success message anonymous object which has a 
                //Message member variable (property)
                var successRequestResultMessage = new
                {
                    Message = "Your amazing brand has been saved!"
                };

                //Create a OkObjectResult class instance, httpOkResult.
                //When creating the object, provide the previous message object into it.
                OkObjectResult httpOkResult =
                                            new OkObjectResult(successRequestResultMessage);
                //Send the OkObjectResult class object back to the client.
                return httpOkResult;
            }
            catch (Exception exceptionObject)
            {
                customMessage = "Unable to save brand into session :(";
                //Create a fail message anonymous object that has one property, Message.
                //This anonymous object's Message property contains a simple string message
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of Try..Catch block
        }//End of SaveNewBrandInformationInSession() method

        // PUT /api/Brands/SaveBrandUpdateInformationIntoSession
        [HttpPut("SaveBrandUpdateInformationIntoSession/{id}")]
        public IActionResult SaveBrandUpdateInformationIntoSession(int id, [FromBody]string value)
        {

            string customMessage = "";
            //Reconstruct a useful object from the input string value. 
            var brandChangeInput = JsonConvert.DeserializeObject<dynamic>(value);

            Brands brandToBeUpdated = new Brands();
            try
            {
                //Copy out all the brands data into the new Brand instance,
                //newBrand.
                brandToBeUpdated.BrandId = id;
                brandToBeUpdated.BrandName = brandChangeInput.BrandName.Value;

                if (brandChangeInput.BrandCategories.Value != null)
                {
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
                return BadRequest(httpFailRequestResultMessage);
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
            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                            new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;
        }//End of SaveBrandUpdateInformationIntoSession() method

        // PUT /api/Brands/SaveBrandUpdateInformationIntoDatabase
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

                // Check if there are any categories i
                if (categories != null)
                {
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

                        // Yet to apply the .NET Core method
                        //DataRow[] foundRows;
                        //foundRows = dataSet1.Tables["Customers"].Select("CompanyName Like 'A%'");
                    }
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
                foundOneBrand.UpdatedById = _userManager.GetUserId(User);
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
                return BadRequest(httpFailRequestResultMessage);
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
            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                                                                    new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;
        }//End of SaveBrandUpdateInformationIntoDatabase() method

        //POST /Api/Brands/UploadBrandPhotoAndUpdateBrandData
        [HttpPost("UploadBrandPhotoAndUpdateBrandData")]
        public async Task<IActionResult> UploadBrandPhotoAndUpdateBrandData(IList<IFormFile> fileInput)
        {
            //Retrieve the brands data which is stashed inside the Session, "Brand".
            //http://benjii.me/2015/07/using-sessions-and-httpcontext-in-aspnet5-and-mvc6/
            Brands brandToBeUpdated = HttpContext.Session.GetObjectFromJson<Brands>("Brands");
            //Get the current brands data from the database.
            //Also get the current brands Photo information.
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

            string CreatedById = _userManager.GetUserId(User);

            var oneFile = fileInput[0];
            var fileName = ContentDispositionHeaderValue
                        .Parse(oneFile.ContentDisposition)
                        .FileName
                        .Trim('"');
            string contentType = oneFile.ContentType;
            //Upload the binary file first
            var currentBrandPhoto = await Cloudinary.CloudinaryAPIs.UploadBrandImageToCloudinary(oneFile.OpenReadStream(), contentType, fileName, "Brands", CreatedById);
            //Delete the existing binary file
            //Obtain the Cloudinary public id value from the foundOneBrand's BrandPhoto navigation property
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
                oneBrand.BrandPhoto.CreatedById = _userManager.GetUserId(User);
            }

            Database.Brands.Update(oneBrand);
            Database.SaveChanges();
            var successRequestResultMessage = new
            {
                Message = "Saved brand.",
                NewImageUrl = oneBrand.BrandPhoto.Url
            };
            OkObjectResult httpOkResult =
                                        new OkObjectResult(successRequestResultMessage);
            return httpOkResult;
        }//End of UploadBrandPhotoAndUpdateBrandData()


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
            
            newBrandPhoto = await Cloudinary.CloudinaryAPIs.UploadBrandImageToCloudinary(oneFile.OpenReadStream(), contentType, fileName, "Brands", _userManager.GetUserId(User));

            //Retrieve the new brands data which is stashed inside the Session, "Brand".
            Brands newBrand = HttpContext.Session.GetObjectFromJson<Brands>("Brands");
            if (newBrandPhoto.PublicCloudinaryId != "")
            {
                //Add the Brand record first, so that the newBrand
                //object's BrandId property is updated with the new record's
                //id.
                newBrand.CreatedById = _userManager.GetUserId(User);
                newBrand.UpdatedById = _userManager.GetUserId(User);
                Database.Brands.Add(newBrand);
                //Copy over the new brands id information to the BrandPhoto object,
                //newBrandPhoto. 
                newBrandPhoto.BrandId = newBrand.BrandId;
                newBrandPhoto.CreatedById = _userManager.GetUserId(User);
                Database.BrandPhotos.Add(newBrandPhoto);
                Database.SaveChanges();
            }
            var successRequestResultMessage = new
            {
                Message = "Saved Brand"
            };

            OkObjectResult httpOkResult =
                new OkObjectResult(successRequestResultMessage);
            return httpOkResult;

        }//End of UploadBrandPhotoAndSaveBrandData()

    }
}
