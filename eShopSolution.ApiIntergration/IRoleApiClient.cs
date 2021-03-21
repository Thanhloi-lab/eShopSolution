using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public interface IRoleApiClient
    {
        Task<List<RolesViewModel>> GetAll();
    }
}
