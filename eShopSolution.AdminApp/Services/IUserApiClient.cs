﻿using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        public Task<string> Authenticate(LoginRequest login);
        public Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request);
        public Task<bool> RegisterUser(RegisterRequest registerRequest);
    }
}
