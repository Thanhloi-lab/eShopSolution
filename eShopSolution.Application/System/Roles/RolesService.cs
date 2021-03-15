using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RolesService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<List<RolesViewModel>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(x => new RolesViewModel()
            {
                Description = x.Description,
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return roles;
        }
    }
}
