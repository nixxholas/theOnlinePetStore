﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientSide_CaseStudy_2_Practise.Controllers
{
    public class ExperimentJavaScriptController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
				public IActionResult ExperimentDOMManipulation() {
						return View();
				}
		}
}
