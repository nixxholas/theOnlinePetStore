using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExperimentOneToManyEntities.Controllers
{
    public class CompaniesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewCompaniesGroupByCompanyType()
        {
            return View();
        }
        public IActionResult ViewCompaniesWithCompanyType()
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



        
    }
}
