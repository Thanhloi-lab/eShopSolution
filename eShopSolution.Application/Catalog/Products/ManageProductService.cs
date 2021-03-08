using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        public EShopDbContext _context;
        public IStorageService _storageService;
        public ManageProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        //product
        public async Task AddViewcount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }
        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice =request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoAlias = request.SeoAlias,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
            };
            //save file

            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail Image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        SortOrder = 1
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        public async Task<int> Delete(int productId)
        {
            //find product by productID
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Cannot fine a product: { productId}");
            //find all imanges and delete imanges file before delete product
            var images =  _context.ProductImages.Where(x => x.ProductId == productId);
            foreach (var item in images)
            {
               await _storageService.DeleteFileAsync(item.ImagePath);
            }
            //delete product
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }
        public async Task<ProductViewModel> GetById(int id, string languageId)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) throw new eShopException($"Cannot fine a product: {id}");
            var productTranslation = await _context.ProductTranslations
                .FirstOrDefaultAsync(x => x.ProductId == id && x.LanguageId == x.LanguageId);

            ProductViewModel view = new ProductViewModel()
            {
                Stock = product.Stock,
                DateCreated = product.DateCreated,
                ViewCount = product.ViewCount,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice
            };
            if(productTranslation !=null)
            {
                view.Name = productTranslation.Name;
                view.SeoAlias = productTranslation.SeoAlias;
                view.SeoDescription = productTranslation.SeoDescription;
                view.SeoTitle = productTranslation.SeoTitle;
                view.Name = productTranslation.Name;
                view.Description = productTranslation.Description;
                view.LanguageId = languageId;
                view.Details = productTranslation.Details;
            }
            return view;
        }
        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
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
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, pic };
            /*
             * @request.Keyword varchar(...)
             * query = query
             *       + where pt.Name Like @request.Keyword
             */
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            /* 
             * request(categoryIDs) is a table that have a list CategoryIDs
             * query = select *from query(--previous table--) as q, request as r
             *         where r.categoryids = pic.categoryid
             */
            if (request.CategorieIds.Count > 0)
            {
                query = query.Where(x => request.CategorieIds.Contains(x.pic.CategoryId));
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
        public async Task<int> Update(ProductUpdateRequest request)
        {
            //get product with the same ID of request
            var product = await _context.Products.FindAsync(request.Id);
            //get productTranslation with the same productID and languageID of request
            var productTranslation = await _context.ProductTranslations
                .FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            if (product == null) throw new eShopException($"Cannot fine a product: {request.Id}");
            //old = new
            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            //save image

            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.IsDefault == true);
                
                if(thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                }
            }

            return await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            //get product with the same ID of request
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Cannot fine a product: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync()>0;
        }
        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            //get product with the same ID of request
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Cannot fine a product: {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }


        //Product image
        public async Task<int> AddImages(int productId, ProductImageCreateRequest request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Cannot find a product: { productId}");

            var productImage = new ProductImage()
            {
                DateCreated = DateTime.Now,
                Caption = "Non-caption",
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder,
            };

            if (request.imageFile!=null)
            {
                productImage.ImagePath = await this.SaveFile(request.imageFile);
                productImage.FileSize = request.imageFile.Length;
            }

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }
        public async Task<int> RemoveImages(int imageId)
        {

            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) throw new eShopException($"Cannot find a image: {imageId}");

            var product = await _context.Products.FindAsync(productImage.ProductId);
            if (product == null) throw new eShopException($"Cannot find a product: { productImage.ProductId}");

            if (productImage.IsDefault == true && product.ProductImages.Count != 1)
            {
                var ortherImage = _context.ProductImages.FirstOrDefault(x => x.ProductId == productImage.ProductId && x.Id != imageId);
                ortherImage.IsDefault = true;
                _context.ProductImages.Update(ortherImage);
            }
            await _storageService.DeleteFileAsync(productImage.ImagePath);
            product.ProductImages.Remove(productImage);
            
            
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateImages(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) throw new eShopException($"Cannot find a image: {imageId}");
            
            //var product = await _context.Products.FindAsync(productImage.ProductId);
            //if (product == null) throw new eShopException($"Cannot find a product: { productImage.ProductId}");

            productImage.Caption = request.Caption;
            productImage.SortOrder = request.SortOrder;

            //ProductImage ortherImage = new ProductImage();
            //if(request.IsDefault != productImage.IsDefault)
            //{
            //    if(request.IsDefault==true)
            //    {
            //        ortherImage = _context.ProductImages.FirstOrDefault(x => x.ProductId == product.Id && x.IsDefault == true);
            //        ortherImage.IsDefault = false;
            //        productImage.IsDefault = true;
            //    }
            //    else
            //    {
            //        ortherImage = _context.ProductImages.FirstOrDefault(x => x.ProductId == product.Id && x.IsDefault == false);
            //        ortherImage.IsDefault = true;
            //        productImage.IsDefault = false;
            //    }
            //}
            if (request.imageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.imageFile);
                productImage.FileSize = request.imageFile.Length;
            }

            //_context.ProductImages.Update(ortherImage);
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }
        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Cannot find a product: { productId}");
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                    .Select(i => new ProductImageViewModel()
                    {
                       Caption = i.Caption,
                       ImagePath = i.ImagePath,
                       FileSize = i.FileSize,
                       DateCreated = i.DateCreated,
                       IsDefault = i.IsDefault,
                       ProductId = i.ProductId,
                       Id = i.Id,
                       SortOrder = i.SortOrder
                    }).ToListAsync();
        }
        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null) throw new eShopException($"Cannot find a image: {imageId}");

            var productViewModel =   new ProductImageViewModel()
            {
                Caption =   image.Caption,
                ImagePath = image.ImagePath,
                FileSize =  image.FileSize,
                DateCreated = image.DateCreated,
                IsDefault =   image.IsDefault,
                ProductId =   image.ProductId,
                Id = image.Id,
                SortOrder = image.SortOrder
            };
            return productViewModel;
        }
        //File
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        
    }
}
