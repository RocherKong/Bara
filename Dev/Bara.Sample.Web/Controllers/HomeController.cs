using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bara.Sample.Web.Models;
using Bara.Abstract.Core;

namespace Bara.Sample.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaraMapper _baraMapper;
        public HomeController(IBaraMapper baraMapper)
        {
            this._baraMapper = baraMapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
