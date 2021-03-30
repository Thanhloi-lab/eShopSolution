using System;
using System.Threading.Tasks;
using eShopSolution.ApiIntergration;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration,
            IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            ViewBag.keyword = keyword;
            if(TempData["result"]!=null)
            {
                ViewBag.success = TempData["result"];
            }
            var data = await _userApiClient.GetUsersPaging(request);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("Token");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "login");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Đăng ký người dùng thành công";
                return RedirectToAction("Index");
            }
                

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result!=null)
            {
                var user = result;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Update(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Chỉnh sửa người dùng thành công";
                return RedirectToAction("Index");
            }    
                

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            //var currentUserClaims = User.Claims;

            //foreach (var item in currentUserClaims)
            //{
            //    if (item.Value.Equals("Admin"))
            //    {
            //        RedirectToAction("Index", "Home");
            //    }
            //}
            var result = await _userApiClient.GetById(id);
            return View(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(Guid id)
        {

            return View(new UserDeleteRequest()
            {
                Id = id
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Xóa người dùng thành công";
                return RedirectToAction("Index");
            }

            return View(request);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            return View(roleAssignRequest);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RoleAssign(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Gán quyền người dùng thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = GetRoleAssignRequest(request.Id);
            return View(roleAssignRequest);
        }
        [Authorize(Roles = "Admin")]
        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            var roleObj = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }
        public IActionResult Forbidden()
        {
            ModelState.AddModelError("Forbidden", "You are not allow to do that action");
            return RedirectToAction("Index", "Home");
        }
    }
}
