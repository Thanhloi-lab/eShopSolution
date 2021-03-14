using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class PagedResult<t> : PagedResultBase
    {
        public List<t> Items { get; set; }
        
    }
}
