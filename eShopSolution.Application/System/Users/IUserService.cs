using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(UserLoginRequest request);

        Task<ApiResult<bool>> Register(UserRegisterRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);
        //Task<bool> RoleAssign(Guid id, RoleAssignRequest request);

        Task<ApiResult<UserViewModel>> GetById(Guid id);

        //Task<bool> Delete(Guid Id);
    }
}
