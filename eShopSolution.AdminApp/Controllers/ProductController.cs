
using System.Linq;
using System.Threading.Tasks;

using eShopSolution.ApiIntergration;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 3)
        {
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = "vi-VN",
                CategoryId = categoryId
            };

            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }

            var products = await _productApiClient.GetPagings(request);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll("vi-VN");
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Tạo mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Tạo mới sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productApiClient.GetById(id, "vi-VN");
            var productViewModel = new ProductUpdateRequest()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Details = product.Details,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                ThumbnailImage = null
            };
            return View(productViewModel);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.UpdateProduct(request);
            if (result)
            {
                TempData["result"] = "Chỉnh sửa sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Chỉnh sửa sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id, string languageId)
        {
            var result = await _productApiClient.GetById(id, languageId);
            return View(result);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ProductDeleteRequest()
            {
                Id = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.DeleteProduct(request);
            if (result)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int Id)
        {
            var roleAssignRequest = await GetCategoryAssignRequest(Id, "vi-VN");
            return View(roleAssignRequest);
        }
        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.CategoryAssign(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Gán danh mục sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = GetCategoryAssignRequest(request.Id, "vi-VN");
            return View(roleAssignRequest);
        }
        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int productId, string languageId)
        {
            var productObj = await _productApiClient.GetById(productId, languageId);
            var categoryObj = await _categoryApiClient.GetAll(languageId);
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var category in categoryObj)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = productObj.Categories.Contains(category.Name)
                });
            }

            return categoryAssignRequest;
        }
    }
}
