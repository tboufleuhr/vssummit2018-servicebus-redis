using System;
using System.Collections.Generic;
using System.Text;

namespace VsSummit2018.Domain.Repositories
{
    public interface IRepository<T> : IRepositoryCommand<T>, IRepositoryQueryable<T>, IDisposable
        where T : IEntity
    {
    }
}
