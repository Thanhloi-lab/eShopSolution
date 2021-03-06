using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using eShopSolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Catalog.Products.Dtos.Public;

namespace eShopSolution.Application.Catalog.Products.Dtos
{
    public class PublicProductService : IPuclicProductService
    {
        private EShopDbContext _context;

        public PublicProductService(EShopDbContext context)
        {
            this._context = context;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request)
        {
            /*
            query = select *
                from products as p, producttranslations as pt, productincategories as pic, categories as c
                where p.id = pt.productID AND p.id = pic.productID AND c.id = pic.categoryId 
            */
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            //get category in request with the same id in table pic
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            /* PAGING*/
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    Description = x.pt.Description,
                    DateCreated = x.p.DateCreated,
                    Details = x.pt.Details,
                    SeoDescription = x.pt.SeoDescription,
                    LanguageId = x.pt.LanguageId,
                    SeoAlias = x.pt.SeoAlias,
                    SeoTitle = x.pt.SeoTitle
                }).ToListAsync();

            //4.select and projection
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

    }
}
