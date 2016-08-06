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
/**
 * 
 * Potential Worker
 * http://stackoverflow.com/questions/4661760/c-sharp-equivalent-for-java-executorservice-newsinglethreadexecutor-or-how-t
 * 
 * **/


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
            //.Include(eachProductEntity => eachProductEntity.Metrics)
            .Include(eachProductEntity => eachProductEntity.ProductPhotos)
                                 .Include(eachUser => eachUser.CreatedBy)
                                 .Include(eachUser => eachUser.UpdatedBy)
            .AsNoTracking();

            //After obtaining all the Product entity rows(records) from the database,
            //the products variable will become an container holding these
            //Product class entity rows.
            //I need to loop through each  Product instance inside products
            //to construct a List container of anonymous objects(which has 6 properties).
            //Then use the new JsonResult(productsList) technique to generate the
            //JSON formatted string data which can be sent back to the web browser client.

            foreach (var oneProduct in products)
            {
                productList.Add(new
                {
                    ProdId = oneProduct.ProdId,
                    ProdName = oneProduct.ProdName,
                    BrandName = oneProduct.Brand.BrandName,
                    Description = oneProduct.Description,
                    TiQ = oneProduct.ThresholdInvertoryQuantity,
                    Quantity = oneProduct.Quantity,
                    Published = oneProduct.Published,
                    CreatedAt = oneProduct.CreatedAt,
                    CreatedBy = oneProduct.CreatedBy.FullName,
                    UpdatedAt = oneProduct.UpdatedAt,
                    UpdatedBy = oneProduct.UpdatedBy.FullName
                });
            }//end of foreach loop which builds the productsList .
            return new JsonResult(productList);
        }

        // We need to find a better way to quantify the quantity column
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

        // GET ViewProductPhotos
        // Takes in an integer as a Product Id
        [HttpGet("ViewProductPhotos/{id}")]
        public JsonResult ProductsUnderCategory(int id)
        {
            List<object> productPhotosList = new List<object>();

            var productPhotos = Database.ProductPhotos
                .Where(eachProductEntity => eachProductEntity.ProdId == id);

            foreach (var oneProductPhoto in productPhotos)
            {

                productPhotosList.Add(new
                {
                    ProductPhotoId = oneProductPhoto.ProductPhotoId,
                    Format = oneProductPhoto.Format,
                    Height = oneProductPhoto.Height,
                    PublicCloudinaryId = oneProductPhoto.PublicCloudinaryId,
                    SecureUrl = oneProductPhoto.SecureUrl,
                    Url = oneProductPhoto.Url,
                    Version = oneProductPhoto.Version,
                    Width = oneProductPhoto.Width,
                    IsPrimaryPhoto = oneProductPhoto.isPrimaryPhoto
                });
            }//end of foreach loop which builds the productsList .
            return new JsonResult(productPhotosList);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var foundProduct = Database.Products
                     .Where(eachProduct => eachProduct.ProdId == id)
                     .Include(eachProduct => eachProduct.Brand)
                     .Include(eachProduct => eachProduct.Consumable)
                     .Include(eachProduct => eachProduct.Specials)
                     .Include(eachProduct => eachProduct.ProductPhotos)
                     .Single();
                
                var response = new
                {
                    ProdId = foundProduct.ProdId,
                    ProdName = foundProduct.ProdName,
                    Description = foundProduct.Description,
                    SavingsOverview = foundProduct.SavingsOverview,
                    ThresholdInventoryQuantity = foundProduct.ThresholdInvertoryQuantity,
                    Quantity = foundProduct.Quantity,
                    ProductPhotos = foundProduct.ProductPhotos,
                    isConsumable = foundProduct.isConsumable,
                    Consumable = foundProduct.Consumable,
                    Specials = foundProduct.Specials,
                    Published = foundProduct.Published,
                    Brand = foundProduct.Brand
                };//end of creation of the response object

                return new JsonResult(response);
            }
            catch (Exception exceptionObject)
            {
                //Create a fail message anonymous object
                //This anonymous object only has one Message property 
                //which contains a simple string message
                object httpFailRequestResultMessage =
                                            new { Message = "Unable to obtain Product information." };
                //Return a bad http response message to the client
                return BadRequest(httpFailRequestResultMessage);
            }
        }

        /**
         * 
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
                // Time to update
                Database.Brands.Update(Brand);
            }

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
                // If there aren't any metrics, we'll toss it back to the user
                if (productNewInput.Metrics.Count == 0)
                {
                    customMessage = "Your product does not contain a metric.";
                    //Create a fail message anonymous object that has one property, Message.
                    //This anonymous object's Message property contains a simple string message
                    object httpFailRequestResultMessage = new { Message = customMessage };
                    //Return a bad http request message to the client
                    return BadRequest(httpFailRequestResultMessage);
                }

                //Copy out all the products data into the new Product instance,
                //newProduct.
                newProduct.ProdName = productNewInput.ProdName.Value;
                newProduct.BrandId = Int32.Parse(productNewInput.BrandId.Value);
                newProduct.Published = Int32.Parse(productNewInput.Published.Value);
                newProduct.ThresholdInvertoryQuantity = Int32.Parse(productNewInput.ThresholdInventoryQuantity.Value);
                newProduct.CreatedById = _userManager.GetUserId(User);
                newProduct.UpdatedById = _userManager.GetUserId(User);
                newProduct.Metrics = new List<Metrics>();

                // Consumable Weak Entity
                if (productNewInput.Consumable != null)
                {
                    newProduct.isConsumable = 1; // It is a consumable
                    Consumable newConsumable = new Consumable();
                    newConsumable.TypicalAnalysis = productNewInput.Consumable.TypicalAnalysis.Value;
                    newConsumable.GuranteedAnalysis = productNewInput.Consumable.GuranteedAnalysis.Value;
                    newConsumable.Ingredients = productNewInput.Consumable.Ingredients.Value;
                    newConsumable.ActiveIngredients = productNewInput.Consumable.ActiveIngredients.Value;
                    newConsumable.InActiveIngredients = productNewInput.Consumable.InActiveIngredients.Value;

                    // Push the consumable object into the product object
                    newProduct.Consumable = newConsumable;
                }
                else
                {
                    newProduct.isConsumable = 0; // It is NOT a consumable
                }

                // Savings Overview
                if (productNewInput.SavingsOverview.Value != null)
                {
                    newProduct.SavingsOverview = productNewInput.SavingsOverview.Value;
                }
                else
                {
                    newProduct.SavingsOverview = null;
                }

                // Description
                if (productNewInput.Description.Value != null)
                {
                    newProduct.Description = productNewInput.Description.Value;
                }
                else
                {
                    newProduct.Description = null;
                }

                // Brand Id Relation
                newProduct.BrandId = Int32.Parse(productNewInput.BrandId.Value);

                // ProductCategory Relation
                var categories = productNewInput.Categories.Value;
                categories = categories.TrimEnd(']');
                categories = categories.TrimStart('[');

                newProduct.ProductCategory = new List<ProductCategory>();
                foreach (string catId in categories.Split(','))
                {
                    int CatId = Int32.Parse(catId);
                    
                    // Create the necessary object to store the composites
                    ProductCategory newProductCategory = new ProductCategory();                                    

                    newProductCategory.ProdId = newProduct.ProdId; // NullReferenceException
                    newProductCategory.CatId = CatId;
                    newProduct.ProductCategory.Add(newProductCategory);
                }

                // Initialize the ProductPhotos list first
                newProduct.ProductPhotos = new List<ProductPhoto>();

                // Iterate through the metric list
                foreach (var Metric in productNewInput.Metrics)
                {
                    // We need have an if statement to identify a custom metric row
                    // or preset metric row as well

                    // If the metric is preset
                    if (Metric.isPreset == "1")
                    {
                        String MetricType = Metric.MetricType.Value;
                        var presetMetricUsed = Database.PresetMetrics
                            .Where(input => input.MetricSubType == MetricType).Single();
                        Metrics newMetric = new Metrics();
                        //newMetric.ProdId = newProduct.ProdId;
                        newMetric.MetricAmount = Int32.Parse(Metric.MetricAmount.Value);
                        newMetric.MetricType = presetMetricUsed.MetricType; // Taken from PresetMetrics Table
                        newMetric.PMetricId = presetMetricUsed.PMetricId; // Only for Preset Metrics
                        newMetric.Quantity = Int32.Parse(Metric.Quantity.Value);
                        String StatusName = Metric.Status.Value;
                        // Status is parsed in a string format
                        var selectedStatus = Database.Statuses
                            .Where(input => input.StatusName == StatusName).Single();
                        newMetric.StatusId = selectedStatus.StatusId;
                        newMetric.CreatedById = _userManager.GetUserId(User);
                        
                        Price price = new Price();
                        // Have not converted to decimal yet
                        price.RRP = Convert.ToDecimal(Metric.RRP.Value);
                        price.Value = Convert.ToDecimal(Metric.Price.Value);
                        price.CreatedById = _userManager.GetUserId(User);

                        // Push the Metric and price into the product object
                        newMetric.Price = price;
                        newProduct.Metrics.Add(newMetric);
                    }
                    else
                    // Else, it'll be a custom preset metric
                    {
                        // Time to construct a custom metric
                        Metrics newMetric = new Metrics();
                        //newMetric.ProdId = newProduct.ProdId;
                        newMetric.MetricAmount = Int32.Parse(Metric.MetricAmount.Value);
                        newMetric.MetricType = Metric.MetricType.Value;
                        newMetric.Quantity = Int32.Parse(Metric.Quantity.Value);
                        String StatusName = Metric.Status.Value;
                        // Status is parsed in a string format
                        var selectedStatus = Database.Statuses
                            .Where(input => input.StatusName == StatusName).Single();
                        newMetric.StatusId = selectedStatus.StatusId;
                        newMetric.CreatedById = _userManager.GetUserId(User);
                        
                        Price price = new Price();
                        // Have not converted to decimal yet
                        price.RRP = Convert.ToDecimal(Metric.RRP.Value);
                        price.Value = Convert.ToDecimal(Metric.Price.Value);
                        price.CreatedById = _userManager.GetUserId(User);

                        // Push the Metric and price into the product object
                        newMetric.Price = price;
                        newProduct.Metrics.Add(newMetric);
                    }
                }

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

                // Implementing a foreach loop such that the quantity 
                // is tabulated amongst many metrics of that particular
                // product should there be more than once metric that is
                // binded.
                int quantity = 0;

                // Let's save the Consumable object if it exists
                if (newProduct.Consumable != null)
                {
                    Database.Consumables.Add(newProduct.Consumable);
                }

                // Let's save the metrics and price
                foreach (var metric in newProduct.Metrics)
                {
                    quantity += metric.Quantity;
                    Database.Metrics.Add(metric);
                    Database.Prices.Add(metric.Price);
                }

                // Set the ProductCategories
                foreach (var ProdCat in newProduct.ProductCategory)
                {
                    Database.ProductCategory.Add(ProdCat);
                }

                // Set the product's total quantity here
                newProduct.Quantity = quantity;

                // Default Product image adapted from
                // http://gemkolabwell.com/Admin/images/product/075319default_product.jpg

                newProduct.ProductPhotos.Add(new ProductPhoto()
                {
                    Format = "jpg",
                    Height = 400,
                    ImageSize = 13167,
                    PublicCloudinaryId = "Products/fkx2d5uduu1ja36zpu03",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1468923470/Products/fkx2d5uduu1ja36zpu03.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1468923470/Products/fkx2d5uduu1ja36zpu03.jpg",
                    Version = 1468923470,
                    Width = 400,
                    isPrimaryPhoto = 1,
                    CreatedById = _userManager.GetUserId(User),
                });

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
                    Message = "Your amazing product has been saved with a default image!"
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
                productToBeUpdated.BrandId = Int32.Parse(productChangeInput.BrandId.Value);
                productToBeUpdated.Published = Int32.Parse(productChangeInput.Published.Value);
                productToBeUpdated.ThresholdInvertoryQuantity = Int32.Parse(productChangeInput.ThresholdInventoryQuantity.Value);
                productToBeUpdated.CreatedById = _userManager.GetUserId(User);
                productToBeUpdated.UpdatedById = _userManager.GetUserId(User);
                productToBeUpdated.Metrics = new List<Metrics>();

                // Consumable Weak Entity
                if (productChangeInput.Consumable != null)
                {
                    productToBeUpdated.isConsumable = 1; // It is a consumable
                    Consumable newConsumable = new Consumable();
                    newConsumable.TypicalAnalysis = productChangeInput.Consumable.TypicalAnalysis.Value;
                    newConsumable.GuranteedAnalysis = productChangeInput.Consumable.GuranteedAnalysis.Value;
                    newConsumable.Ingredients = productChangeInput.Consumable.Ingredients.Value;
                    newConsumable.ActiveIngredients = productChangeInput.Consumable.ActiveIngredients.Value;
                    newConsumable.InActiveIngredients = productChangeInput.Consumable.InActiveIngredients.Value;

                    // Push the consumable object into the product object
                    productToBeUpdated.Consumable = newConsumable;
                }
                else
                {
                    productToBeUpdated.isConsumable = 0; // It is NOT a consumable
                }

                // Savings Overview
                if (productChangeInput.SavingsOverview.Value != null)
                {
                    productToBeUpdated.SavingsOverview = productChangeInput.SavingsOverview.Value;
                }
                else
                {
                    productToBeUpdated.SavingsOverview = null;
                }

                // Description
                if (productChangeInput.Description.Value != null)
                {
                    productToBeUpdated.Description = productChangeInput.Description.Value;
                }
                else
                {
                    productToBeUpdated.Description = null;
                }
                
                // ProductCategory Relation
                var categories = productChangeInput.Categories.Value;
                categories = categories.TrimEnd(']');
                categories = categories.TrimStart('[');

                productToBeUpdated.ProductCategory = new List<ProductCategory>();
                foreach (string catId in categories.Split(','))
                {
                    int CatId = Int32.Parse(catId);

                    // Create the necessary object to store the composites
                    ProductCategory newProductCategory = new ProductCategory();

                    newProductCategory.ProdId = productToBeUpdated.ProdId; // NullReferenceException
                    newProductCategory.CatId = CatId;
                    productToBeUpdated.ProductCategory.Add(newProductCategory);
                }
                
                // Iterate through the metric list
                foreach (var Metric in productChangeInput.Metrics)
                {
                    // We need have an if statement to identify a custom metric row
                    // or preset metric row as well

                    // If the metric is preset
                    if (Metric.isPreset == "1")
                    {
                        String MetricType = Metric.MetricType.Value;
                        var presetMetricUsed = Database.PresetMetrics
                            .Where(input => input.MetricSubType == MetricType).Single();
                        Metrics newMetric = new Metrics();
                        //newMetric.ProdId = newProduct.ProdId;
                        newMetric.MetricAmount = Int32.Parse(Metric.MetricAmount.Value);
                        newMetric.MetricType = presetMetricUsed.MetricType; // Taken from PresetMetrics Table
                        newMetric.PMetricId = presetMetricUsed.PMetricId; // Only for Preset Metrics
                        newMetric.Quantity = Int32.Parse(Metric.Quantity.Value);
                        String StatusName = Metric.Status.Value;
                        // Status is parsed in a string format
                        var selectedStatus = Database.Statuses
                            .Where(input => input.StatusName == StatusName).Single();
                        newMetric.StatusId = selectedStatus.StatusId;
                        newMetric.CreatedById = _userManager.GetUserId(User);

                        Price price = new Price();
                        // Have not converted to decimal yet
                        price.RRP = Convert.ToDecimal(Metric.RRP.Value);
                        price.Value = Convert.ToDecimal(Metric.Price.Value);
                        price.CreatedById = _userManager.GetUserId(User);

                        // Push the Metric and price into the product object
                        newMetric.Price = price;
                        productToBeUpdated.Metrics.Add(newMetric);
                    }
                    else
                    // Else, it'll be a custom preset metric
                    {
                        // Time to construct a custom metric
                        Metrics newMetric = new Metrics();
                        //newMetric.ProdId = newProduct.ProdId;
                        newMetric.MetricAmount = Int32.Parse(Metric.MetricAmount.Value);
                        newMetric.MetricType = Metric.MetricType.Value;
                        newMetric.Quantity = Int32.Parse(Metric.Quantity.Value);
                        String StatusName = Metric.Status.Value;
                        // Status is parsed in a string format
                        var selectedStatus = Database.Statuses
                            .Where(input => input.StatusName == StatusName).Single();
                        newMetric.StatusId = selectedStatus.StatusId;
                        newMetric.CreatedById = _userManager.GetUserId(User);

                        Price price = new Price();
                        // Have not converted to decimal yet
                        price.RRP = Convert.ToDecimal(Metric.RRP.Value);
                        price.Value = Convert.ToDecimal(Metric.Price.Value);
                        price.CreatedById = _userManager.GetUserId(User);

                        // Push the Metric and price into the product object
                        newMetric.Price = price;
                        productToBeUpdated.Metrics.Add(newMetric);
                    }
                }

                var foundOneProduct = Database.Products
                        .Where(eachProduct => eachProduct.ProdId == id)
                        .Include(eachProduct => eachProduct.Brand)
                        .Include(eachProduct => eachProduct.Metrics)
                        .Include(eachProduct => eachProduct.Consumable)
                        .Single();

                // Let's update the general stuff first           
                foundOneProduct.ProdName = productToBeUpdated.ProdName;
                foundOneProduct.BrandId = productToBeUpdated.BrandId;
                foundOneProduct.Published = productToBeUpdated.Published;
                foundOneProduct.ThresholdInvertoryQuantity = productToBeUpdated.ThresholdInvertoryQuantity;

                // Update Metrics
                /**
                 * We need to check whether it exists first, then update it
                 * if it doesn't exist, add it
                 * if an old metric no longer exists, delete it 
                 **/

                // We'll perform updating first
                // MetricAmount and Type must be the same if the user wants it to be updating.
                // Changing the Amount is equivalent to creating a new Metric
                foreach (var incomingMetric in productToBeUpdated.Metrics)
                {
                    foreach (var metricFromDB in foundOneProduct.Metrics)
                    {
                        // If we can find it, update it
                        if (incomingMetric.MetricAmount == metricFromDB.MetricAmount && incomingMetric.MetricType == metricFromDB.MetricType)
                        {
                            // If it's a preset metric
                            if (incomingMetric.PresetMetric != null)
                            {
                                metricFromDB.PresetMetric = null;
                                metricFromDB.PMetricId = null;
                                metricFromDB.Status = incomingMetric.Status;
                                metricFromDB.StatusId = incomingMetric.StatusId;
                                metricFromDB.Quantity = incomingMetric.Quantity;

                                // Update the price as well
                                // Remove the old pricing
                                metricFromDB.Price.DeletedAt = DateTime.Now;
                                metricFromDB.Price.DeletedById = _userManager.GetUserId(User);
                                // Add the new price
                                metricFromDB.Price = incomingMetric.Price;
                                
                            } else
                            {

                            }
                        }  
                    }
                }

                foundOneProduct.UpdatedAt = DateTime.Now;
                foundOneProduct.UpdatedById = _userManager.GetUserId(User);

                Database.Products.Update(foundOneProduct);
                Database.SaveChanges();//Without this command, the changes are not committed.
                computeProductsPerBrand();
                customMessage = "Saved product into database.";
            }
            catch (Exception exceptionObject)
            {
                customMessage = "Unable to save product into database. Error: ";
                //Create a fail message anonymous object that has one property, Message.
                //This anonymous object's Message property contains a simple string message
                object httpFailRequestResultMessage = new { Message = customMessage + exceptionObject };
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
            oneProduct.ProductPhotos = new List<ProductPhoto>();

            // Load ProductPhotos in this way
            var productPhotos = Database.ProductPhotos
                .Where(input => input.Product == oneProduct);

            foreach (var oneFile in fileInput)
            {
                foreach (ProductPhoto productPhoto in productPhotos)
                {
                    //var oneFile = fileInput[0];
                    var fileName = ContentDispositionHeaderValue
                                .Parse(oneFile.ContentDisposition)
                                .FileName
                                .Trim('"');

                    string CreatedById = _userManager.GetUserId(User);

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

                    oneProduct.ProductPhotos.Add(productPhoto);
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
            // boolean to force the first image to be the default image
            bool alreadyHasPrimary = false;
            //Retrieve the new products data which is stashed inside the Session, "Product".
            Product newProduct = HttpContext.Session.GetObjectFromJson<Product>("Product");

            // Implementing a foreach loop such that the quantity 
            // is tabulated amongst many metrics of that particular
            // product should there be more than once metric that is
            // binded.
            int quantity = 0;

            // Let's save the Consumable object if it exists
            if (newProduct.Consumable != null)
            {
                Database.Consumables.Add(newProduct.Consumable);
            }

            // Let's save the metrics and price
            foreach (var metric in newProduct.Metrics)
            {
                quantity += metric.Quantity;
                Database.Metrics.Add(metric);
                Database.Prices.Add(metric.Price); // Can work?
            }

            // Set the product's total quantity here
            newProduct.Quantity = quantity;

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

                    newProductPhoto.Product = newProduct; // Relationship Fix

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

                    //if (firstImage == true)
                    //{
                    //    firstImage = false;
                    //    newProductPhoto.isPrimaryPhoto = 1;
                    //}
                    Database.ProductPhotos.Add(newProductPhoto);
                }
                innerSystem = innerSystem + 2;
                fileDataIndex += 1;
            }

            Database.Products.Add(newProduct);
            Database.SaveChanges();

            computeProductsPerBrand();

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
