using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WEBA_ASSIGNMENT.Models;
using WEBA_ASSIGNMENT.Services;
using Microsoft.Extensions.Logging;
using WEBA_ASSIGNMENT.Data;
using WEBA_ASSIGNMENT.Controllers;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WEBA_ASSIGNMENT.APIs
{
    [Authorize(Roles = "SUPER ADMIN,ADMIN")]
    [Route("api/[controller]")]
    public class SpecialsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        //Every Web API controller class need to have an ApplicationDbContext
        //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }
        public SpecialsController(UserManager<ApplicationUser> userManager,
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
            List<object> entityList = new List<object>();
            var entitiesForCat = Database.Categories
                .Include(input => input.Visibility)
                .Include(input => input.BrandCategory);

            foreach (var category in entitiesForCat)
            {
                entityList.Add(new
                {
                    CatId = category.CatId,
                    CatName = category.CatName,
                    VisibilityId = category.VisibilityId,
                    Visibility = category.Visibility.VisibilityName,
                    Brands = category.BrandCategory,
                    //CreatedAt = category.CreatedAt,
                    //CreatedBy = category.CreatedBy.FullName,
                    //UpdatedAt = category.UpdatedAt,
                    //UpdatedBy = category.UpdatedBy.FullName,
                    //DeletedAt = category.DeletedAt
                });
            }//end of foreach loop which builds the categoryList .

            var entitiesForBrand = Database.Brands
                .Include(input => input.BrandPhoto);

            foreach (var oneBrand in entitiesForBrand)
            {
                entityList.Add(new
                {
                    BrandId = oneBrand.BrandId,
                    BrandName = oneBrand.BrandName,
                    PhotoUrl = oneBrand.BrandPhoto.Url,
                    NoOfProducts = oneBrand.NoOfProducts,
                    //CreatedAt = oneBrand.CreatedAt,
                    //UpdatedAt = oneBrand.UpdatedAt
                });
            }

            var entitiesForProduct = Database.Products
                .Include(input => input.Brand)
                .Include(input => input.Consumable);

            foreach (var oneProduct in entitiesForProduct)
            {
                entityList.Add(new
                {
                    ProdId = oneProduct.ProdId,
                    ProdName = oneProduct.ProdName,
                    BrandName = oneProduct.Brand.BrandName,
                    Description = oneProduct.Description,
                    Consumable = oneProduct.Consumable,
                    TiQ = oneProduct.ThresholdInvertoryQuantity,
                    Quantity = oneProduct.Quantity,
                    Published = oneProduct.Published,
                    //CreatedAt = oneProduct.CreatedAt,
                    //CreatedBy = oneProduct.CreatedBy.FullName,
                    //UpdatedAt = oneProduct.UpdatedAt,
                    //UpdatedBy = oneProduct.UpdatedBy.FullName
                });
            }

            return new JsonResult(entityList);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
