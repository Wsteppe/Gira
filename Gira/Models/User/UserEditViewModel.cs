using System.Collections.Generic;
using System.Web.Mvc;
using Gira.Data.Entities;

namespace Gira.Models.User
{
    public class UserEditViewModel
    {
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Issue list for when application user is manager or administrator
        /// </summary>
        public IEnumerable<SelectListItem> Managers { get; set; }
    }
}