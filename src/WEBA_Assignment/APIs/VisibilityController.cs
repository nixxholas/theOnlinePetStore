//Need InternshipManagementSystemWithSecurity_V1.Data so that the .NET can find
//the ApplicationDbContext class.
using WEBA_ASSIGNMENT.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WEBA_ASSIGNMENT.APIs
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
            }//end of foreach loop which builds the visibilityList .
            return new JsonResult(visibilityList);
        }
    }
}
