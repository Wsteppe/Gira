using System.Threading.Tasks;

namespace Gira.Data.Repositories.Interfaces
{
    public interface IEntityRepository<T> : IRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
    }
}
