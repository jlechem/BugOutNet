﻿using BugOutNet.CustomActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        [UserActionFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}