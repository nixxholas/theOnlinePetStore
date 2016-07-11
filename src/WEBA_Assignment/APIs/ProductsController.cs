﻿using WEBA_ASSIGNMENT.Models;
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
    public class ProductsController : Controller

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        //Every Web API controller class need to have an ApplicationDbContext
        //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }

        public ProductsController(UserManager<ApplicationUser> userManager,
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
            List<object> productList = new List<object>();
            var products = Database.Products
            .Where(eachProductEntity => eachProductEntity.DeletedAt == null)
            .Include(eachProductEntity => eachProductEntity.Brand)
            .Include(eachProductEntity => eachProductEntity.Metrics)
            .Include(eachProductEntity => eachProductEntity.ProductPhotos).AsNoTracking();
            
            //After obtaining all the Product entity rows (records) from the database,
            //the productss variable will become an container holding these 
            //Product class entity rows.
            //I need to loop through each  Product instance inside productss
            //to construct a List container of anonymous objects (which has 6 properties).
            //Then use the new JsonResult(productsList) technique to generate the
            //JSON formatted string data which can be sent back to the web browser client.
            foreach (var oneProduct in products)
            {
                productList.Add(new
                {
                    ProdId = oneProduct.ProdId,
                    ProdName = oneProduct.ProdName,
                    Brand = oneProduct.Brand,
                    BrandName = oneProduct.Brand.BrandName,
                    // Save this for later
                    // PhotoUrl = oneProduct.ProductPhotos.Url,
                    TiS = oneProduct.ThresholdInvertoryQuantity,
                    Quantity = getTotalQuantity(oneProduct.Metrics),
                    Published = oneProduct.Published,
                    CreatedAt = oneProduct.CreatedAt,
                    CreatedBy = oneProduct.CreatedBy.FullName,
                    UpdatedAt = oneProduct.UpdatedAt,
                    UpdatedBy = oneProduct.UpdatedBy.FullName
                });
            }//end of foreach loop which builds the productsList .
            return new JsonResult(productList);
        }

        // Method to compute the total amount of quantity of a product.
        public int getTotalQuantity(List<Metrics> metricList)
        {
            int TotalQuantity = 0;
            foreach (Metrics metric in metricList)
            {
                TotalQuantity += metric.Quantity;
            }
            return TotalQuantity;
        }

        // GET ProductsUnderBrand
        // Takes in an integer as a Category Id
        [HttpGet("GetProductsUnderBrand/{id}")]
        public JsonResult ProductsUnderCategory(int id)
        {
            List<object> productList = new List<object>();

            var products = Database.Products
                .Where(eachProductEntity => eachProductEntity.Brand.BrandId == id)
                .Include(eachProductEntity => eachProductEntity.Brand)
                .Include(eachProductEntity => eachProductEntity.Metrics)
                .Include(eachProductEntity => eachProductEntity.ProductPhotos).AsNoTracking();

            foreach (var oneProduct in products)
            {
                productList.Add(new
                {
                    ProdId = oneProduct.ProdId,
                    ProdName = oneProduct.ProdName,
                    Brand = oneProduct.Brand,
                    BrandName = oneProduct.Brand.BrandName,
                    // Save this for later
                    // PhotoUrl = oneProduct.ProductPhotos.Url,
                    TiS = oneProduct.ThresholdInvertoryQuantity,
                    Quantity = getTotalQuantity(oneProduct.Metrics),
                    Published = oneProduct.Published,
                    CreatedAt = oneProduct.CreatedAt,
                    CreatedBy = oneProduct.CreatedBy.FullName,
                    UpdatedAt = oneProduct.UpdatedAt,
                    UpdatedBy = oneProduct.UpdatedBy.FullName
                });
            }//end of foreach loop which builds the productsList .
            return new JsonResult(productList);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var foundProduct = Database.Products
                     .Where(eachProduct => eachProduct.ProdId == id)
                     .Include(eachProduct => eachProduct.ProductPhotos).Single();
                var response = new
                {
                    ProdId = foundProduct.ProdId,
                    ProdName = foundProduct.ProdName,
                    ProductPhotos = foundProduct.ProductPhotos
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

        /**
         * A method to enable the server to
         * compute the amount of products within a brand
         * 
         * */
        public void computeProductsPerBrand()
        {
            // Get all the brands first
            var allBrands = Database.Brands
                .Where(eachBrand => eachBrand.DeletedAt == null)
                .Include(eachBrand => eachBrand.Products);

            // We then implement a loop to configure the NoOfProducts Column
            foreach (var Brand in allBrands)
            {
                // Let us reset the column first
                Brand.NoOfProducts = 0;
                // All set, time to count
                foreach (var Product in Brand.Products)
                {
                    Brand.NoOfProducts += 1;
                }
            }

            // Update the table and save it with the Database
            Database.Update(allBrands);
            Database.SaveChanges();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //You are making a soft delete (not hard delete).
            //Therefore, the Product binary photo image at Cloudinary will remain.
            string customMessage = "";
            try
            {
                var foundOneProduct = Database.Products
                                     .Include(item => item.CreatedBy)
                                     .Single(item => item.ProdId == id);
                foundOneProduct.DeletedAt = DateTime.Now;
                foundOneProduct.DeletedById = _userManager.GetUserId(User);
                //Update the database model
                Database.Update(foundOneProduct);
                //Tell the db model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
                //Let's update the Brands table as well 
                computeProductsPerBrand();
            }
            catch (Exception ex)
            {
                customMessage = "Unable to delete products record.";
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of try .. catch block on manage data

            //Build a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Deleted Product record"
            };

            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                                                            new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;
        }//end of Delete(id) Web API method with /API/Products/digit URL pattern route

        // POST /api/Products/SaveNewProductInformationInSession
        [HttpPost("SaveNewProductInformationInSession")]
        public IActionResult SaveNewProductInformationInSession([FromBody]string value)
        {
            string customMessage = "";
            // Issue: Should I add a "'" into a the Product name String, the received from the
            // Client results in an unended json object..
            //Reconstruct a useful object from the input string value. 
            var productNewInput = JsonConvert.DeserializeObject<dynamic>(value);

            Product newProduct = new Product();
            try
            {
                //Copy out all the products data into the new Product instance,
                //newProduct.
                newProduct.ProdName = productNewInput.ProdName.Value;
                if (productNewInput.Description.Value != null)
                {
                    newProduct.Description = productNewInput.Description.Value;
                } else
                {
                    newProduct.Description = null;
                }
                newProduct.BrandId = Int32.Parse(productNewInput.BrandId.Value);
                
                // Have to implement a foreach loop to stash multiple Metrics
                // into the table
                
                //I cannot save the products information into database yet. The 
                //UploadProductPhotosAndSaveProductData has logic to upload the binary file to
                //Cloudinary first, then it will save products data into the database.
                //Therefore, I need to save the products data as a Session variable first.
                //The command below will save the product data inside a
                //Session variable, Product (I can use other names...it is just a name)
                HttpContext.Session.SetObjectAsJson("Product", newProduct);
            }
            catch (Exception exceptionObject)
            {
                customMessage = "Unable to save product into session.";
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
                Message = "Saved product into session"
            };

            //Create a OkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult =
                                        new OkObjectResult(successRequestResultMessage);
            //Send the OkObjectResult class object back to the client.
            return httpOkResult;

        }//End of SaveNewProductInformationInSession() method

        // POST /api/Products/SaveNewProductInformationInSession
        [HttpPost("SaveProductData")]
        public IActionResult SaveProductDataInDatabase([FromBody]string value)
        {
            string customMessage = "";
            // Issue: Should I add a "'" into a the Product name String, the received from the
            // Client results in an unended json object..
            //Reconstruct a useful object from the input string value. 
            Product newProduct = HttpContext.Session.GetObjectFromJson<Product>("Product");

            try
            {


                // Parse in auditing data
                newProduct.CreatedById = _userManager.GetUserId(User);
                newProduct.UpdatedById = _userManager.GetUserId(User);

                // I didn't even start testing the ProductPhotos yet
                // So I'll leave the default Product image alone first

                //newProduct.ProductPhotos.Add(new ProductPhoto()
                //{
                //    Format = "jpg",
                //    Height = 120,
                //    ImageSize = 1692,
                //    PublicCloudinaryId = "Products/u0ofsgsn9b1q6tlwrkxg",
                //    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466794773/Products/u0ofsgsn9b1q6tlwrkxg.jpg",
                //    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466794773/Products/u0ofsgsn9b1q6tlwrkxg.jpg",
                //    Version = 1466794773,
                //    Width = 171
                //});
                //Add the product record first, so that the newProduct
                //object's ProdId property is updated with the new record's
                //id.
                Database.Products.Add(newProduct);
                Database.SaveChanges();
                // We'll then update the brands table
                computeProductsPerBrand();

                //******************************************************
                //Construct a custom message for the client
                //Create a success message anonymous object which has a 
                //Message member variable (property)
                var successRequestResultMessage = new
                {
                    Message = "Your amazing product has been saved!"
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
                customMessage = "Unable to save product into session :(";
                //Create a fail message anonymous object that has one property, Message.
                //This anonymous object's Message property contains a simple string message
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of Try..Catch block
        }//End of SaveNewProductInformationInSession() method

        // PUT /api/Products/SaveProductUpdateInformationIntoSession
        [HttpPut("SaveProductUpdateInformationIntoSession/{id}")]
        public IActionResult SaveProductUpdateInformationIntoSession(int id, [FromBody]string value)
        {

            string customMessage = "";
            //Reconstruct a useful object from the input string value. 
            var productChangeInput = JsonConvert.DeserializeObject<dynamic>(value);

            Product productToBeUpdated = new Product();
            try
            {
                //Copy out all the products data into the new Product instance,
                //newProduct.
                productToBeUpdated.ProdId = id;
                productToBeUpdated.ProdName = productChangeInput.ProdName.Value;
                               

                //Saved it into a Session variable
                HttpContext.Session.SetObjectAsJson("Products", productToBeUpdated);
                customMessage = "Saved product into session";
            }
            catch (Exception exceptionObject)
            {
                customMessage = "Unable to save product into database.";
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
        }//End of SaveProductUpdateInformationIntoSession() method

        // PUT /api/Products/SaveProductUpdateInformationIntoDatabase
        [HttpPut("SaveProductUpdateInformationIntoDatabase/{id}")]
        public IActionResult SaveProductUpdateInformationIntoDatabase(int id, [FromBody]string value)
        {
            string customMessage = "";
            //Reconstruct a useful object from the input string value. 
            var productChangeInput = JsonConvert.DeserializeObject<dynamic>(value);

            Product productToBeUpdated = new Product();
            try
            {
                productToBeUpdated.ProdName = productChangeInput.ProdName.Value;
                
                var foundOneProduct = Database.Products
                        .Where(eachProduct => eachProduct.ProdId == id)
                        .Include(eachProduct => eachProduct.Brand)
                        .Single();
                
                foundOneProduct.ProdName = productToBeUpdated.ProdName;
                foundOneProduct.UpdatedAt = DateTime.Now;
                Database.Products.Update(foundOneProduct);
                Database.SaveChanges();//Without this command, the changes are not committed.
                computeProductsPerBrand();
                customMessage = "Saved product into database.";
            }
            catch (Exception exceptionObject)
            {
                customMessage = "Unable to save product into database.";
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
        }//End of SaveProductUpdateInformationIntoDatabase() method

        //POST /Api/Products/UploadProductPhotosAndUpdateProductData
        [HttpPost("UploadProductPhotosAndUpdateProductData")]
        public async Task<IActionResult> UploadProductPhotosAndUpdateProductData(IList<IFormFile> fileInput)
        {
            //Retrieve the products data which is stashed inside the Session, "Product".
            //http://benjii.me/2015/07/using-sessions-and-httpcontext-in-aspnet5-and-mvc6/
            Product productToBeUpdated = HttpContext.Session.GetObjectFromJson<Product>("Product");
            //Get the current products data from the database.
            //Also get the current products Photo information.
            var oneProduct = Database.Products
                .Where(Product => Product.ProdId == productToBeUpdated.ProdId)
                .Include(Product => Product.Brand)
                .Single(); // Take in only that product.

            oneProduct.ProdName = productToBeUpdated.ProdName;
            
            foreach (var oneFile in fileInput)
            {
                foreach (ProductPhoto productPhoto in oneProduct.ProductPhotos)
                {
                    //var oneFile = fileInput[0];
                    var fileName = ContentDispositionHeaderValue
                                .Parse(oneFile.ContentDisposition)
                                .FileName
                                .Trim('"');

                    string contentType = oneFile.ContentType;
                    //Upload the binary file first
                    var currentProductPhotos = await Cloudinary.CloudinaryAPIs.UploadProductImageToCloudinary(oneFile.OpenReadStream(), contentType, fileName, "Products");

                    //Delete the existing binary file
                    //Obtain the Cloudinary public id value from the foundOneProduct's ProductPhotos navigation property
                    string originalCloudinaryPublicId = "";
                    originalCloudinaryPublicId = productPhoto.PublicCloudinaryId;


                    //Use the Cloudinary public id value as an input argument for the DeleteImageInCloudinary to delete the binary
                    //file resource.
                    Boolean result = await Cloudinary.CloudinaryAPIs.DeleteImageInCloudinary(originalCloudinaryPublicId);

                    if (currentProductPhotos.PublicCloudinaryId != "")
                    {
                        productPhoto.ImageSize = currentProductPhotos.ImageSize;
                        productPhoto.Version = currentProductPhotos.Version;
                        productPhoto.Height = currentProductPhotos.Height;
                        productPhoto.Width = currentProductPhotos.Width;
                        productPhoto.PublicCloudinaryId = currentProductPhotos.PublicCloudinaryId;
                        productPhoto.Url = currentProductPhotos.Url;
                        productPhoto.SecureUrl = currentProductPhotos.SecureUrl;
                    }
                }
            }


            Database.Products.Update(oneProduct);
            Database.SaveChanges();
            computeProductsPerBrand();
            var successRequestResultMessage = new
            {
                Message = "Saved product."
            };
            OkObjectResult httpOkResult =
                                        new OkObjectResult(successRequestResultMessage);
            return httpOkResult;
        }//End of UploadProductPhotosAndUpdateProductData()


        //POST /Api/Products/UploadProductPhotosAndSaveProductData
        [HttpPost("UploadProductPhotosAndSaveProductData")]
        public async Task<IActionResult> UploadProductPhotosAndSaveProductData(IList<IFormFile> fileInput)
        {
            var oneFile = fileInput[0];
            ProductPhoto newProductPhotos;
            var fileName = ContentDispositionHeaderValue
                  .Parse(oneFile.ContentDisposition)
                  .FileName
                  .Trim('"');
            string contentType = oneFile.ContentType;
            // Not complete
            newProductPhotos = await Cloudinary.CloudinaryAPIs.UploadProductImageToCloudinary(oneFile.OpenReadStream(), contentType, fileName, "Products");

            //Retrieve the new products data which is stashed inside the Session, "Product".
            Product newProduct = HttpContext.Session.GetObjectFromJson<Product>("Products");
            if (newProductPhotos.PublicCloudinaryId != "")
            {
                //Add the Product record first, so that the newProduct
                //object's ProdId property is updated with the new record's
                //id.
                Database.Products.Add(newProduct);
                //Copy over the new products id information to the ProductPhotos object,
                //newProductPhotos. 
                newProductPhotos.ProdId = newProduct.ProdId;
                Database.ProductPhotos.Add(newProductPhotos);
                Database.Products.Add(newProduct);
                Database.SaveChanges();
                computeProductsPerBrand();
            }
            var successRequestResultMessage = new
            {
                Message = "Saved Product"
            };

            OkObjectResult httpOkResult =
                new OkObjectResult(successRequestResultMessage);
            return httpOkResult;

        }//End of UploadProductPhotosAndSaveProductData()

    }
}
