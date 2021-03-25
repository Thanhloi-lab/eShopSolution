using eShopSolution.ApiIntergration;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration
            , IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<string>> Authenticate(UserLoginRequest request)
        {
            var result = await AuthenticateAsync<string>("/api/Users/authenticate", request);
            CookieOptions option = new CookieOptions();
            return result;
        }
        public async Task<bool> Delete(Guid id)
        {
            var result = await DeleteAsync($"/api/users/{id}");
            return result;
        }
        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            var result = await GetAsync<UserViewModel>($"/api/users/{id}");
            return result;
        }
        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPaging(GetUserPagingRequest request)
        {
            var result = await GetAsync<PagedResult<UserViewModel>> ($"/api/users/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");

            return result;
        }
        public async Task<ApiResult<bool>> RegisterUser(UserRegisterRequest registerRequest)
        {
            var result = await PostAsync<bool>($"/api/users/", registerRequest);
            return result;
        }
        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var result = await PutAsync<bool>($"/api/users/{id}/roles", request);
            return result;
        }
        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            var result = await PutAsync<bool>($"/api/users/{id}", request);
            return result;
        }
    }
}
