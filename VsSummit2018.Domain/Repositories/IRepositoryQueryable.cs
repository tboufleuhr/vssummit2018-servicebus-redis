using System.Linq;
using System.Threading.Tasks;

namespace VsSummit2018.Domain.Repositories
{
    public interface IRepositoryQueryable<T> where T : IEntity
    {
        IQueryable<T> GetAll();

        Task<T> GetByIdAsync(int id);
    }
}
