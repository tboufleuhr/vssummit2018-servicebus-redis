using VsSummit2018.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VsSummit2018.Domain
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : IEntity;

        void Complete();

        Task CompleteAsync();
    }
}
