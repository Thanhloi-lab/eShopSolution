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
        Task<string> Authenticate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);

        Task<PagedResult<UserViewModel>> GetUserPaging(GetUserPagingRequest request);
        //Task<bool> Update(Guid id, UpdateRequest request);
        //Task<bool> RoleAssign(Guid id, RoleAssignRequest request);

        //Task<UserViewModel> GetById(Guid id);

        //Task<bool> Delete(Guid Id);
    }
}
