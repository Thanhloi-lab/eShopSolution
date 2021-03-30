using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public interface IUserApiClient
    {
        public Task<ApiResult<string>> Authenticate(UserLoginRequest login);
        public Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request);
        public Task<ApiResult<bool>> RegisterUser(UserRegisterRequest registerRequest);
        Task<UserViewModel> GetById(Guid id);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);
        Task<bool> Delete(Guid id);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}
