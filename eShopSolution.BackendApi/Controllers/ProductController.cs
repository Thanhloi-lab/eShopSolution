using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPuclicProductService _publicProductService;
        public ProductController(IPuclicProductService publicProductService)
        {
            _publicProductService = publicProductService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _publicProductService.GetAll();
            return Ok(data);
        }
    }
}
