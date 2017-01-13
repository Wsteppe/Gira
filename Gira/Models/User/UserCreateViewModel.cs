using System.Collections.Generic;
using System.Web.Mvc;
using Gira.Data.Entities;

namespace Gira.Models.User
{
    public class UserCreateViewModel
    {
        public ApplicationUser User { get; set; }

        public MultiSelectList Roles { get; set; }

        /// <summary>
        /// Issue list for when application user is manager or administrator
        /// </summary>
        public IEnumerable<SelectListItem> Managers { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}