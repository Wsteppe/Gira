using System.Threading.Tasks;

namespace Gira.Data.Repositories.Interfaces
{
    public interface IIdentityRepository<T> : IRepository<T> where T : class
    {
        Task<T> GetAsync(string id);
    }
}
