using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using eShopSolution.ApiIntergration;
using eShopSolution.Utilities.Constant;
using System.Collections.Generic;
using eShopSolution.ViewModels.Catalog.Products;
using System.Linq;

namespace eShopSolution.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;

        public HomeController(ILogger<HomeController> logger,
            ISlideApiClient slideApiClient,
            IProductApiClient productApiClient)
        {
            _logger = logger;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index(string languageId, int id, int pageIndex = 1, int pageSize = SystemConstant.ProductSettings.NumberOfLastestProducts)
        {
            GetManageProductPagingRequest request = new GetManageProductPagingRequest()
            {
                LanguageId = "vi-VN",
                PageSize = pageSize,
                PageIndex = pageIndex,
            };
            var viewModel = new HomeViewModel
            {
                Slides = await _slideApiClient.GetAll(),
                FeaturedProducts = await _productApiClient.GetFeaturedProducts(request),
                LastestProducts = await _productApiClient.GetLastestProducts("vi-VN", SystemConstant.ProductSettings.NumberOfLastestProducts)
            };
            viewModel.FeaturedProducts.Items = await GetProductImages(viewModel.FeaturedProducts.Items);
            viewModel.LastestProducts = await GetProductImages(viewModel.LastestProducts);
            return View(viewModel);
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}