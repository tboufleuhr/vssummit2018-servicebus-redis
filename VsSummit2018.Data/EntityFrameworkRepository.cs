using VsSummit2018.Domain;
using VsSummit2018.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace VsSummit2018.Data
{
    public class EntityFrameworkRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly VsSummit2018Context context;
        private readonly DbSet<T> collection;

        public EntityFrameworkRepository(VsSummit2018Context context)
        {
            this.context = context;
            collection = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await collection.AddAsync(entity);
        }

        public IQueryable<T> GetAll()
        {
            return collection;
        }

        public Task<T> GetByIdAsync(int id)
        {
            return collection.FindAsync(id);
        }

        public async Task RemoveAsync(T entity)
        {
            await Task.Run(() => collection.Remove(entity));
        }

        public async Task RemoveAsync(int id)
        {
            await RemoveAsync(await GetByIdAsync(id));
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() => collection.Update(entity));
        }

        public void Dispose()
        {
            //context.Dispose();
            //GC.SuppressFinalize(this);
        }
    }
}
