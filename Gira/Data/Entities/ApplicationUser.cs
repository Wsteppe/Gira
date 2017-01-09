using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Data.Entities
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string MobilePhone { get; set; }

        [InverseProperty("Creator")]
        public ICollection<Issue> IssuesCreated { get; set; }
        [InverseProperty("ResponsibleUser")]
        public ICollection<Issue> IssuesResponsible { get; set; }

        [InverseProperty("Manager")]
        public ICollection<Issue> IssuesManaged { get; set; }

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