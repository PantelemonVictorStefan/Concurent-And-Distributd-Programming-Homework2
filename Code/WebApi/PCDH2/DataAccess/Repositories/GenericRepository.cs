using Microsoft.EntityFrameworkCore;
using PCDH2.Core.Contracts.Repositories;
using PCDH2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where
        TEntity : Entity, new()
    {
        private readonly FeedContext ctx;

        public GenericRepository(FeedContext dbContext)
        {
            ctx = dbContext;
        }
        public async Task Add(TEntity entity)
        {
            await ctx.AddAsync(entity);
            await ctx.SaveChangesAsync();
        }


        public IQueryable<TEntity> GetAll()
        {
            return ctx.Set<TEntity>().AsNoTracking();
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await ctx.Set<TEntity>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Create(TEntity entity)
        {
            await ctx.Set<TEntity>().AddAsync(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task Update(Guid id, TEntity entity)
        {
            ctx.Set<TEntity>().Update(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetById(id);
            ctx.Set<TEntity>().Remove(entity);
            await ctx.SaveChangesAsync();
        }

    }
}