

using GymManagement.Infrastructure.Models;
using System.Linq.Expressions;

namespace GymManagement.Infrastructure.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
     Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task <TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdIncludedDeletedAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> FindAsync(Expression <Func<TEntity,bool>> predicate, CancellationToken cancellationToken = default);


    Task<bool> ExistAsyc(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity);
    Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync();

}
