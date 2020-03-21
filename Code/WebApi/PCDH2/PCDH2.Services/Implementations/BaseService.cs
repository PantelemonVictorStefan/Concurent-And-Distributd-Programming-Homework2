using PCDH2.Core.Contracts.Repositories;
using PCDH2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCDH2.Services.Implementations
{
    public abstract class BaseService<T>
        where T:Entity,new()
    {
        protected IGenericRepository<T> repository;
        public BaseService(IGenericRepository<T> repository)
        {
            this.repository = repository;
        }
    }
}
