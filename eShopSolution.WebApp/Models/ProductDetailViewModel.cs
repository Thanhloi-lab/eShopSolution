using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Models
{
    public class ProductDetailViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public ProductViewModel Product { get; set; }

    }
}
