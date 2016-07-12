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
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WEBA_ASSIGNMENT.APIs
{
    [Authorize(Roles = "SUPER ADMIN,ADMIN")]
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        //Every Web API controller class need to have an ApplicationDbContext
        //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }
        public StatusController(UserManager<ApplicationUser> userManager,
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
            List<object> statusList = new List<object>();
            var statuses = Database.Statuses;

            foreach (var status in statuses)
            {
                statusList.Add(new
                {
                    StatusId = status.StatusId,
                    StatusName = status.StatusName
                });
            }//end of foreach loop which builds the categoryList .
            return new JsonResult(statusList);
        }

        // Don't need all these junk

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
