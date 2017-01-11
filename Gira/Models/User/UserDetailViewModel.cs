using System.Collections.Generic;
using Gira.Data.Entities;

namespace Gira.Models.User
{
    public class UserDetailViewModel
    {
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Issue list for when application user is manager or administrator
        /// </summary>
        public IEnumerable<Issue> Issues { get; set; }
    }
}