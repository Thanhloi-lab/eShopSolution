using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;
        public CategoryService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId
            }).ToListAsync();
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryByProductId(int productId, string languageId)
        {
            var query = from c in _context.Categories 
                        join ct in _context.CategoryTranslations on c.Id equals ct.Id
                        join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                        join p in _context.Products on pic.ProductId equals p.Id
                        where p.Id == productId && ct.LanguageId == languageId
                        select new {c, ct};
            var categories = await query.Select(x=>new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId
            }).ToListAsync();
            return categories;
        }

        public async Task<CategoryViewModel> GetById(string languageId, int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var categoryTranslation = await _context.CategoryTranslations
                    .FirstOrDefaultAsync(x => x.LanguageId == languageId && x.CategoryId == id);
            CategoryViewModel model = new CategoryViewModel()
            {
                Id = category.Id,
                Name = categoryTranslation.Name == null? "N/A": categoryTranslation.Name,
                ParentId = category.ParentId
            };
            return model;
        }
    }
}
