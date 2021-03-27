
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
        //Entity Product
        [Display(Name = "Mã sản phẩm")]
        public int Id { set; get; }
        [Display(Name = "Giá")]
        public decimal Price { set; get; }
        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { set; get; }
        [Display(Name = "Tồn kho")]
        public int Stock { set; get; }
        [Display(Name = "Lượt xem")]
        public int ViewCount { set; get; }
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }
        [Display(Name = "Xuất hiện trên trang chủ")]
        public bool? IsFeatured { get; set; }
        [Display(Name = "Hình ảnh đại diện")]
        public string ThumbnailImage { get; set; }

        // Entity ProductTranslation
        [Display(Name = "Tên")]
        public string Name { set; get; }
        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        [Display(Name = "Chi tiết")]
        public string Details { set; get; }
        [Display(Name = "Chi tiết-tìm kiếm")]
        public string SeoDescription { set; get; }
        [Display(Name = "Tiêu đề-tìm kiếm")]
        public string SeoTitle { set; get; }
        [Display(Name = "Từ khóa")]
        public string SeoAlias { get; set; }
        [Display(Name = "Mã ngôn ngữ")]
        public string LanguageId { set; get; }
        [Display(Name = "Những danh mục")]
        public List<string> Categories { get; set; } = new List<string>();
    }
}
