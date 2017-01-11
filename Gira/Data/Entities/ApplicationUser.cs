using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Data.Entities
{
    //lazy loading ftw
    public class ApplicationUser : IdentityUser
    {
        public string Surname { get; set; }
        [DisplayName(@"Given Name")]
        public string GivenName { get; set; }
        [DisplayName(@"Mobile")]
        public string MobilePhone { get; set; }

        [InverseProperty("Creator")]
        public virtual ICollection<Issue> IssuesCreated { get; set; }
        [InverseProperty("ResponsibleUser")]
        public virtual ICollection<Issue> IssuesResponsible { get; set; }

        public virtual ApplicationUser Manager { get; set; }
        public string ManagerId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim(ClaimTypes.Role,"ADMINISTRATOR"));
            return userIdentity;
        }
    }
}