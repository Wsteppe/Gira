using System.Threading.Tasks;
using Gira.Data.Entities;
using Gira.Data.Repositories.Interfaces;

namespace Gira.Data
{
    public interface IGiraUoW
    {
        Task SaveAsync();

        IBaseRepository<Issue> Issues { get; }
    }
}
