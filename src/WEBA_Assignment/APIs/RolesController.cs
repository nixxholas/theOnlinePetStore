﻿using WEBA_ASSIGNMENT.Models;
using WEBA_ASSIGNMENT.Data;
using WEBA_ASSIGNMENT.Services;
using WEBA_ASSIGNMENT.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
namespace WEBA_ASSIGNMENT.APIs
{
    [Authorize(Roles = "SUPER ADMIN,ADMIN")]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        
        public ApplicationDbContext Database { get; }


        //Create a Constructor, so that the .NET engine can pass in the ApplicationDbContext object
        //which represents the database session.
        public RolesController()
        {
            Database = new ApplicationDbContext();
        }



        // GET: api/Company
        [HttpGet]
        public IActionResult Get()
        {
            //The include() feature requires Microsoft Data Entity for now.
            //The visual studio might suggest you to install some packages for the include()
            //to be recognized. Don't select them.
            var roles = Database.Roles.Select(roleItem => new { RoleId = roleItem.Id, RoleName = roleItem.Name }  );
            return new JsonResult(roles);
        }//end of Get()

    }
}
