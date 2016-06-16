using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace WEBACA.Controllers
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
