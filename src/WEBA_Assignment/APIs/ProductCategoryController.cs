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
    [Authorize(Roles = "SUPER ADMIN,ADMIN")]
    [Route("api/[controller]")]
    public class ProductCategoryController : Controller
    {
        public ApplicationDbContext Database { get; }

        public ProductCategoryController()
        {
            Database = new ApplicationDbContext();
        }

        [HttpGet]
        public JsonResult Get()
        {
            List<object> productCategoryList = new List<object>();
            var productCategories = Database.ProductCategory
            .Include(eachproductCategoryEntity => eachproductCategoryEntity.Product)
            .Include(eachproductCategoryEntity => eachproductCategoryEntity.Category);

            //After obtaining all the productCategory entity rows (records) from the database,
            //the productCategory variable will become an container holding these 
            //productCategory class entity rows.
            //I need to loop through each productCategory instance inside BrandCategories
            //to construct a List container of anonymous objects.
            //Then use the new JsonResult(productCategoryList) technique to generate the
            //JSON formatted string data which can be sent back to the web browser client.
            foreach (var oneProductCategory in productCategories)
            {
                productCategoryList.Add(new
                {
                    ProdId = oneProductCategory.ProdId,
                    ProdName = oneProductCategory.Product.ProdName,
                    CatId = oneProductCategory.CatId,
                    CatName = oneProductCategory.Category.CatName
                });
            }//end of foreach loop which builds the productCategoryList .
            return new JsonResult(productCategoryList);
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
                List<object> productCategoryList = new List<object>();
                var foundProductCategories = Database.ProductCategory
                     .Where(eachproductCategory => eachproductCategory.ProdId == id)
                     .Include(eachproductCategory => eachproductCategory.Category)
                     .Include(eachproductCategory => eachproductCategory.Product);

                foreach (var productCategory in foundProductCategories)
                {
                    productCategoryList.Add(new
                    {
                        ProdId = productCategory.ProdId,
                        CatId = productCategory.CatId,
                        CatName = productCategory.Category.CatName,
                        ProdName = productCategory.Product.ProdName
                    });
                }//end of foreach loop which builds the categoryList .
                return new JsonResult(productCategoryList);
            }
            catch (Exception exceptionObject)
            {
                //Create a fail message anonymous object
                //This anonymous object only has one Message property 
                //which contains a simple string message
                object httpFailRequestResultMessage =
                                    new { Message = "Unable to obtain Brand Category information." };
                //Return a bad http response message to the client
                return BadRequest(httpFailRequestResultMessage);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
