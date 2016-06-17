using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBACA.Models;

namespace WEBACA.APIs
{
    [Route("api/[controller]")]
    public class VisibilityController : Controller
    {
        //Every Web API controller class need to have an ApplicationDbContext
        //type property. I have declared one here, called Database.
        public ApplicationDbContext Database { get; }
        public VisibilityController()
        {
            Database = new ApplicationDbContext();
        }
        // GET: api/values
        [HttpGet("GetAllVisibilities")]
        public JsonResult Get()
        {
            List<object> visibilityList = new List<object>();
            var visibilities = Database.Visibilities;

            foreach (var visibility in visibilities)
            {
                visibilityList.Add(new
                {
                    VisibilityId = visibility.VisibilityId,
                    VisibilityName = visibility.VisibilityName
                });
            }//end of foreach loop which builds the categoryList .
            return new JsonResult(visibilityList);
        }
    }
}
