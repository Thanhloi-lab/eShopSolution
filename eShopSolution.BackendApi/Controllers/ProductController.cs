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

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IPuclicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductController(IPuclicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;

        }

        //Products
        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(request, languageId);

            return Ok(products);
        }
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _manageProductService.GetById(productId, languageId);
            if (product == null)
                return BadRequest($"Cannot find product: {productId}");
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productId = await _manageProductService.Create(request);
            if (productId <= 0)
                return BadRequest();
            var product = await _manageProductService.GetById(productId, request.LanguageId);
            return Created(nameof(GetById), product);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var effectedResult = await _manageProductService.Update(request);
            if (effectedResult <= 0)
                return BadRequest();

            return Ok();
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var effectedResult = await _manageProductService.Delete(productId);
            if (effectedResult <= 0)
                return BadRequest();

            return Ok();
        }
        [HttpPatch("/{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isSuccess = await _manageProductService.UpdatePrice(productId, newPrice);
            if (isSuccess == false)
                return BadRequest();

            return Ok();
        }

        //Images
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> Create(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageId = await _manageProductService.AddImages(productId, request);
            if (imageId <= 0)
                return BadRequest();

            var image = await _manageProductService.GetImageById(imageId);
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var product = await _manageProductService.GetImageById(imageId);
            if (product == null)
                return BadRequest($"Cannot find product: {imageId}");
            return Ok(product);
        }
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> Update(int productId, int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var effectedResult = await _manageProductService.UpdateImages(imageId, request);
            if (effectedResult <= 0)
                return BadRequest();

            return Ok();
        }
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> Delete(int productId, int imageId)
        {
            var effectedResult = await _manageProductService.RemoveImages(imageId);
            if (effectedResult <= 0)
                return BadRequest();

            return Ok();
        }
    }
}
