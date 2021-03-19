using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using eShopSolution.ViewModels.Catalog.Categories;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;

        }

        //Products
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _productService.GetAllPaging(request);
            return Ok(products);
        }
        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId, [FromQuery] GetProductPagingRequest request)
        {
            var products = await _productService.GetAllByCategoryId(request, languageId);

            return Ok(products);
        }
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _productService.GetById(productId, languageId);
            if (product == null)
                return BadRequest($"Cannot find product: {productId}");
            return Ok(product);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productId = await _productService.Create(request);
            if (productId <= 0)
                return BadRequest();
            var product = await _productService.GetById(productId, request.LanguageId);
            return Created(nameof(GetById), product);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var effectedResult = await _productService.Update(request);
            if (effectedResult <= 0)
                return BadRequest();

            return Ok();
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var effectedResult = await _productService.Delete(productId);
            if (effectedResult <= 0)
                return BadRequest();

            return Ok();
        }
        [HttpPatch("/{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isSuccess = await _productService.UpdatePrice(productId, newPrice);
            if (isSuccess == false)
                return BadRequest();

            return Ok();
        }
        [HttpPut("{productId}/categories")]
        public async Task<IActionResult> RoleAssign(int productId, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.CategoryAssign(productId, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //Images
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> Create(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageId = await _productService.AddImages(productId, request);
            if (imageId <= 0)
                return BadRequest();

            var image = await _productService.GetImageById(imageId);
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var product = await _productService.GetImageById(imageId);
            if (product == null)
                return BadRequest($"Cannot find product: {imageId}");
            return Ok(product);
        }
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> Update(int productId, int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var effectedResult = await _productService.UpdateImages(imageId, request);
            if (effectedResult <= 0)
                return BadRequest();

            return Ok();
        }
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> Delete(int productId, int imageId)
        {
            var effectedResult = await _productService.RemoveImages(imageId);
            if (effectedResult <= 0)
                return BadRequest();

            return Ok();
        }
    }
}
