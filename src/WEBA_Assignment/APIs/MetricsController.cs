using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WEBA_ASSIGNMENT.Data;
using WEBA_ASSIGNMENT.Models;
using WEBA_ASSIGNMENT.Services;
using Microsoft.Extensions.Logging;
using WEBA_ASSIGNMENT.Controllers;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WEBA_ASSIGNMENT.APIs
{
    [Authorize(Roles = "SUPER ADMIN,ADMIN")]
    [Route("api/[controller]")]
    public class MetricsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        //Every Web API controller class need to have an ApplicationDbContext
        //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }

        public MetricsController(UserManager<ApplicationUser> userManager,
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            List<Object> metricList = new List<Object>();

            try
            {
                var foundMetrics = Database.Metrics
                    .Where(input => input.ProdId == id)
                    .Where(input => input.DeletedAt == null)
                    .Include(input => input.Price)
                    .Include(input => input.Status)
                    .Include(input => input.PresetMetric).AsNoTracking();
                 
                foreach (var metric in foundMetrics)
                {
                    if (metric.PresetMetric == null)
                    {
                        metricList.Add(new
                        {
                            MetricId = metric.MetricId,
                            MetricType = metric.MetricType,
                            MetricAmount = metric.MetricAmount,
                            Quantity = metric.Quantity,
                            Status = metric.Status,
                            Price = metric.Price.Value,
                            RRP = metric.Price.RRP,
                            PresetMetric = metric.PresetMetric
                        });
                    } else
                    {
                        metricList.Add(new
                        {
                            MetricId = metric.MetricId,
                            MetricSubType = metric.PresetMetric.MetricSubType,
                            MetricAmount = metric.MetricAmount,
                            Quantity = metric.Quantity,
                            Status = metric.Status,
                            Price = metric.Price.Value,
                            RRP = metric.Price.RRP,
                            PresetMetric = metric.PresetMetric
                        });
                    }
                }

                return new JsonResult(metricList);
            }
            catch (Exception exceptionObject)
            {
                //Create a fail message anonymous object
                //This anonymous object only has one Message property 
                //which contains a simple string message
                object httpFailRequestResultMessage =
                                            new { Message = "Unable to obtain Metrics information for your product." };
                //Return a bad http response message to the client
                return BadRequest(httpFailRequestResultMessage);
            }
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
