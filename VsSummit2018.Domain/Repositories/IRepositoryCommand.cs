using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VsSummit2018.Domain.Repositories
{
    public interface IRepositoryCommand<T> where T : IEntity
    {
        Task AddAsync(T entity);

        Task UpdateAsync(T entity);
        
        Task RemoveAsync(T entity);

        Task RemoveAsync(int id);
    }
}
