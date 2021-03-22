using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ApiIntergration;
using eShopSolution.Utilities.Constant;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace eShopSolution.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IUserApiClient userApiClient,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            
            if (token != null)
            {
                var userPrincipal = this.ValidateToken(token);
                if(userPrincipal == null)
                {
                    Response.Cookies.Delete("Token");
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("index", "login");
                }  
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };
                HttpContext.Session.SetString(SystemConstant.AppSettings.DefaultLanguageId, _configuration["DefaultLanguageId"]);
                HttpContext.Session.SetString(SystemConstant.AppSettings.Token, token);
                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
                Response.Cookies.Append("Token", token, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(30)});
                return RedirectToAction("Index", "Home");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserLoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            var result = await _userApiClient.Authenticate(request);
            if (result.ResultObject == null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
                
            var userPrincipal = this.ValidateToken(result.ResultObject);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(30)
            };
            HttpContext.Session.SetString(SystemConstant.AppSettings.DefaultLanguageId, _configuration["DefaultLanguageId"]);
            HttpContext.Session.SetString(SystemConstant.AppSettings.Token, result.ResultObject);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            Response.Cookies.Append("Token", result.ResultObject, new CookieOptions(){Expires=DateTimeOffset.Now.AddDays(30)});
            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = false;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            ClaimsPrincipal principal= null;
            try
            {
                principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            }
            catch (Exception e)
            {
                return null;
            }
            return principal;
        }
    }
}