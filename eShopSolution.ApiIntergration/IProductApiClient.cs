using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.ApiIntergration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetPagings(GetManageProductPagingRequest request);
        Task<bool> CreateProduct(ProductCreateRequest request);
        Task<bool> UpdateProduct(ProductUpdateRequest request);
        Task<bool> DeleteProduct(ProductDeleteRequest request);
        Task<ApiResult<bool>> CategoryAssign(int productId, CategoryAssignRequest request);
        Task<ProductViewModel> GetById(int productId, string languageId);
        Task<PagedResult<ProductViewModel>> GetFeaturedProducts(GetManageProductPagingRequest request);
        Task<List<ProductViewModel>> GetLastestProducts(string languageId, int take);
        Task<List<ProductImageViewModel>> GetListImages(int productId);
    }
}
