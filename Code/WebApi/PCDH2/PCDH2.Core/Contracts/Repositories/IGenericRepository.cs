using PCDH2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCDH2.Core.Contracts.Repositories
{
    public interface IGenericRepository<TEntity>
        where TEntity : Entity, new()
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById(Guid id);

        Task Add(TEntity entity);

        Task Update(Guid id, TEntity entity);

        Task Delete(Guid id);
    }
}
