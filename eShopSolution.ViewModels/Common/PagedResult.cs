using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class PagedResult<t>
    {
        public List<t> Items { get; set; }
        public int TotalRecord { get; set; }
    }
}
