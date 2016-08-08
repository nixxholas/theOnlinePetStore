using Microsoft.AspNetCore.Mvc;

namespace WEBA_ASSIGNMENT.Controllers
{
    public class CategoriesController : Controller
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
        public IActionResult Update()
        {
            return View();
        }

        public IActionResult Restore()
        {
            return View();
        }
        public IActionResult BrandsUnderCategory()
        {
            return View();
        }
        public IActionResult ProductsUnderCategory()
        {
            return View();
        }
    }
}
