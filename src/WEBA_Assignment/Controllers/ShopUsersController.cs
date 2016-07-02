using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WEBA_ASSIGNMENT.Controllers
{
    public class ShopUsersController : Controller
    {

       // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
  public IActionResult Create()
  {
   return View();
  }
 }//End of Students Action Controller class
}//End of namespace

