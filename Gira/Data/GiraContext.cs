using System.Data.Entity;

namespace Gira.Data
{
    public class GiraContext : DbContext
    {
        private GiraContext() : base("Gira")
        {
        }
    }
}