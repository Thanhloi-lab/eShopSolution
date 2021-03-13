using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShopSolution.AdminApp.Controllers
{
    
    public class BaseController : Controller
    {
        private readonly HttpContextAccessor _httpContextAccessor;
        //public BaseController(HttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            var sessions = context.HttpContext.Session.GetString("Token");
            if (sessions == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
            base.OnActionExecuting(context);
            //if(cookieValueFromContext == null)
            //{
            //    context.Result = new RedirectToActionResult("Index", "Login", null);
            //}
            //base.OnActionExecuting(cookieValueFromContext);
        }
    }
}