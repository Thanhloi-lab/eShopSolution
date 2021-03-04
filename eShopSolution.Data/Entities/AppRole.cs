using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        string description;

        public string Description { get => description; set => description = value; }
    }
}
