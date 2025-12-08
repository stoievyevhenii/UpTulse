using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using UpTulse.Core.Common;
using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.Persistence;

namespace UpTulse.DataAccess.Repositories.Impl
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DatabaseContext Context;
        protected readonly DbSet<TEntity> DbSet;

        protected BaseRepository(DatabaseContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var addedEntity = (await DbSet.AddAsync(entity)).Entity;
            await Context.SaveChangesAsync();

            return addedEntity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entity)
        {
            await DbSet.AddRangeAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet
                .Where(predicate)
                .AsNoTracking()
                .AnyAsync();
        }

        public async Task<bool> AnyAsync()
        {
            return await DbSet.AnyAsync();
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            var removedEntity = DbSet.Remove(entity).Entity;
            await Context.SaveChangesAsync();

            return removedEntity;
        }

        public async Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await DbSet
                .Where(predicate)
                .AsNoTracking()
                .FirstOrDefaultAsync() ?? throw new DbRecordNotFoundException(typeof(TEntity));

            var removedEntity = DbSet.Remove(entity).Entity;

            await Context.SaveChangesAsync();
            return removedEntity;
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await DbSet
                .Where(predicate)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return entity == null ? throw new DbRecordNotFoundException(typeof(TEntity)) : await DbSet.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet
                .Where(predicate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            await Context.SaveChangesAsync();

            return entities;
        }
    }
}