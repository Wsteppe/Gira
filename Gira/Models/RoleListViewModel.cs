using System.Collections.Generic;
using Gira.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Models
{
    public class RoleListViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}