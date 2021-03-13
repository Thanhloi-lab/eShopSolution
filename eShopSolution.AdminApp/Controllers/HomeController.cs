using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eShopSolution.AdminApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace eShopSolution.AdminApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly HttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger)//, HttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            //_httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            //string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["key"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
