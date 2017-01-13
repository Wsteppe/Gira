using System.Collections.Generic;
using System.Web.Mvc;
using Gira.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Models.User
{
    public class UserEditViewModel
    {
        public ApplicationUser User { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public IEnumerable<IdentityRole> UserRoles { get; set; }

        /// <summary>
        /// Issue list for when application user is manager or administrator
        /// </summary>
        public IEnumerable<SelectListItem> Managers { get; set; }
    }
}