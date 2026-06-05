using GeymManagement.DbContexts;
using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagement.Infrastructure.Repositories;

public class Repository<TEntity>(GymDbContext dbContext) : IMemberRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
      => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<TEntity?> GetByIdIncludedDeletedAsync(int id, CancellationToken cancellationToken = default)
        => await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public Task<bool> ExistAsyc(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default)
        => _dbSet.AnyAsync(predicate, cancellation);

    public Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync()
        => dbContext.SaveChangesAsync();

    public async Task<IReadOnlyList<TEntity>> GetListWithSpecAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        => await ApplySpecification(spec).AsNoTracking().ToListAsync(cancellationToken);

    public async Task<TEntity?> GetEntityWithSpecAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        => await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        => SpecificationEvaluator<TEntity>.GetQuery(_dbSet.AsQueryable(), spec);
}