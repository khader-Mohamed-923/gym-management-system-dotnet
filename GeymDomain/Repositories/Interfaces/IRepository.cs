using GymManagement.Domain.Entities;
using GymManagement.Domain.Specifications;
using System.Linq.Expressions;

namespace GymManagement.Domain.Repositories;

public interface IMemberRepository<TEntity> where TEntity : BaseEntity
{
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdIncludedDeletedAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistAsyc(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity);

    Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync();

    Task<IReadOnlyList<TEntity>> GetListWithSpecAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    Task<TEntity?> GetEntityWithSpecAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
}
