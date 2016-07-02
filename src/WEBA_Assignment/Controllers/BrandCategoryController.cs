using Microsoft.AspNetCore.Mvc;

namespace WEBA_ASSIGNMENT.Controllers
{
    public class BrandCategoryController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewBrandsGroupByCategories()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
