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
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WEBA_ASSIGNMENT.Helper;

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
        
        //POST /Api/Products/UploadProductPhotosAndSaveProductData
        [HttpPost("UploadNewUpdatedProductPhotos/{id}")]
        public async Task<IActionResult> UploadProductPhotosAndSaveProductData(int id, IList<IFormFile> fileInput)
        {
            // boolean to force the first image to be the default image
            bool alreadyHasPrimary = false;

            // Let's get the current product linked to these images first
            var foundProduct = Database.Products
                .Where(Product => Product.ProdId == id)
                .Include(Product => Product.ProductPhotos)
                .Single();

            // Define values for the isPrimaryPhoto Array
            int fileDataIndex = 0;
            int innerSystem = 0;
            string fileDataValue = "";

            //Add the Product record first, so that the newProduct
            //object's ProdId property is updated with the new record's
            //id.

            foreach (var oneFile in fileInput)
            {
                fileDataValue = Request.Form["NEW_" + fileDataIndex.ToString()].ToString();
                ProductPhoto newProductPhoto;
                var fileName = ContentDispositionHeaderValue
                      .Parse(oneFile.ContentDisposition)
                      .FileName
                      .Trim('"');
                string contentType = oneFile.ContentType;

                newProductPhoto = await Cloudinary.CloudinaryAPIs.UploadProductImageToCloudinary(oneFile.OpenReadStream(), contentType, fileName, "Products");

                if (newProductPhoto.PublicCloudinaryId != "")
                {
                    //Copy over the new products id information to the ProductPhotos object,
                    newProductPhoto.Product = foundProduct; // Relationship Fix

                    //newProductPhoto.ProdId = newProduct.ProdId;
                    newProductPhoto.CreatedById = _userManager.GetUserId(User);
                    // Attempt to test the isPrimaryPhoto variable
                    if (fileDataValue == (innerSystem).ToString() && alreadyHasPrimary == false)
                    {
                        newProductPhoto.isPrimaryPhoto = 1;
                        alreadyHasPrimary = true;
                    }
                    else
                    {
                        newProductPhoto.isPrimaryPhoto = 0;
                    }
                    
                    Database.ProductPhotos.Add(newProductPhoto);
                }
                innerSystem = innerSystem + 2;
                fileDataIndex += 1;
            }

            Database.Products.Update(foundProduct);
            Database.SaveChanges();
            
            var successRequestResultMessage = new
            {
                Message = "Your amazing product image/s have been saved!"
            };

            OkObjectResult httpOkResult =
                new OkObjectResult(successRequestResultMessage);
            return httpOkResult;

        }//End of UploadProductPhotosAndSaveProductData()
              
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
