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
    public class PresetMetricsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        //Every Web API controller class need to have an ApplicationDbContext
        //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }
        public PresetMetricsController(UserManager<ApplicationUser> userManager,
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
            List<object> presetMetricsList = new List<object>();
            var presetmetrics = Database.PresetMetrics;

            foreach (var presetmetric in presetmetrics)
            {
                presetMetricsList.Add(new
                {
                    PMetricId = presetmetric.PMetricId,
                    MetricType = presetmetric.MetricType,
                    MetricSubType = presetmetric.MetricSubType,
                    MetricCharacter = presetmetric.MetricCharacter
                });
            }//end of foreach loop which builds the categoryList .
            return new JsonResult(presetMetricsList);
        }

        // GET: api/values
        //[HttpGet]
        //[HttpGet("{id}")]
        //public JsonResult Get(int id)
        //{
        //    try
        //    {
        //        var foundPMetric = Database.PresetMetrics
        //            .Include(input => input.Metrics)

        //        // Not yet implemented
        //        if (foundProduct.isConsumable != 0)
        //        {
        //            // var foundConsumable = Database.
        //        }

        //        int quantity = 0;

        //        foreach (Metrics metric in foundProduct.Metrics)
        //        {
        //            quantity += metric.Quantity;
        //        }

        //        var response = new
        //        {
        //            ProdId = foundProduct.ProdId,
        //            ProdName = foundProduct.ProdName,
        //            Description = foundProduct.Description,
        //            Brand = foundProduct.Brand,
        //            ThresholdInventoryQuantity = foundProduct.ThresholdInvertoryQuantity,
        //            Quantity = quantity,
        //            Metrics = foundProduct.Metrics,
        //            ProductPhotos = foundProduct.ProductPhotos,
        //            isConsumable = foundProduct.isConsumable,
        //            Specials = foundProduct.Special,
        //            Published = foundProduct.Published,
        //        };//end of creation of the response object

        //        return new JsonResult(response);
        //    }
        //    catch (Exception exceptionObject)
        //    {
        //        //Create a fail message anonymous object
        //        //This anonymous object only has one Message property 
        //        //which contains a simple string message
        //        object httpFailRequestResultMessage =
        //                                    new { Message = "Unable to obtain Category information." };
        //        //Return a bad http response message to the client
        //        return BadRequest(httpFailRequestResultMessage);
        //    }
        //}
    }
}
