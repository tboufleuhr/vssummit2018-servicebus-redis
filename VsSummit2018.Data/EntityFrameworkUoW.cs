using VsSummit2018.Domain;
using VsSummit2018.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace VsSummit2018.Data
{
    public class EntityFrameworkUoW : IUnitOfWork
    {
        private readonly VsSummit2018Context context;
        private readonly IServiceProvider serviceProvider;

        public EntityFrameworkUoW(VsSummit2018Context context, IServiceProvider serviceProvider)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
        }

        public void Complete()
        {
            var rows = context.SaveChanges();
        }

        public Task CompleteAsync()
        {
            return context.SaveChangesAsync();
        }
        
        public IRepository<T> GetRepository<T>() where T : IEntity
        {
            return serviceProvider.GetService<IRepository<T>>();
        }
    }
}
