using System.Linq.Expressions;

using UpTulse.Core.Common;

namespace UpTulse.DataAccess.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entity);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> AnyAsync();

        Task<int> CountAsync();

        Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> DeleteAsync(TEntity entity);

        Task<List<TEntity>> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities);
    }
}