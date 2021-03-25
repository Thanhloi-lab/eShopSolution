using eShopSolution.ApiIntergration;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public class CategoryApiClient :BaseApiClient, ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            var result = await GetListAsync<CategoryViewModel>("/api/categories?languageId=" + languageId);
            return result;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryByProductId(string languageId, int productId)
        {
            var result = await GetListAsync<CategoryViewModel>($"/api/categories/productcategories/{languageId}/{productId}");
            return result;
        }

        public async Task<CategoryViewModel> GetById(string languageId, int id)
        {
            var result = await GetListPageAsync<CategoryViewModel>($"/api/categories/{languageId}/{id}");
            return result;
        }
    }
}
