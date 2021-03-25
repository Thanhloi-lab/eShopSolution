using eShopSolution.ApiIntergration;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        public readonly IProductApiClient _productApiClient;
        public readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Detail(int id, string languageId)
        {
            var product = await _productApiClient.GetById(id, languageId);
            var categories = await _categoryApiClient.GetAllCategoryByProductId(languageId, id);
            if(categories == null)
            {
                categories = new List<CategoryViewModel>();
                var category = new CategoryViewModel()
                {
                    Id = -1,
                    Name = "N/A",
                };
                categories.Add(category);
            }
            var products = new List<ProductViewModel>();
            products.Add(product.ResultObject);
            products = await GetProductImages(products);
            product.ResultObject = products.ElementAt(0);
            return View(new ProductDetailViewModel() 
            { 
                Categories = categories,
                Product = product.ResultObject
            });
        }

        public async Task<IActionResult> Category(string languageId, int id, int page = 1, int pageSize = 10)
        {
            var products = await _productApiClient.GetPagings(new GetManageProductPagingRequest()
            {
                CategoryId = id,
                LanguageId = languageId,
                PageIndex = page,
                PageSize = pageSize
            });
            products.Items = await GetProductImages(products.Items);
            return View(new ProductCategoryViewModel()
            {
                Category = await _categoryApiClient.GetById(languageId, id),
                Products = products
            }) ;
        }
        public async Task<List<ProductViewModel>> GetProductImages(List<ProductViewModel> products)
        {
            foreach (var item in products)
            {
                var images = await _productApiClient.GetListImages(item.Id);
                if (images != null)
                {
                    if (images.Count > 0)
                    {
                        foreach (var image in images)
                        {
                            if (image.IsDefault == true)
                                item.ThumbnailImage = image.ImagePath;
                        }
                        if (item.ThumbnailImage == null)
                        {
                            item.ThumbnailImage = images.ElementAt(0).ImagePath;
                        }
                    }
                }
            }
            return products;
        }
    }
}
