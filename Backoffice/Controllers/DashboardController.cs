using Backoffice.Helpers;
using Backoffice.Models;
using Backoffice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Backoffice.Areas.Dashboard.Controllers
{
    [BackofficeAuthorize]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
