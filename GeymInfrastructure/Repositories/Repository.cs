using GeymManagement.DbContexts;
using GymManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace GymManagement.Infrastructure.Repositories;

public class Repository<TEntity>(GymDbContext dbContext) : IRepository<TEntity> where TEntity : BaseEntity
{


    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();


    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
      => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)

      => await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<TEntity?> GetByIdIncludedDeletedAsync(int id, CancellationToken cancellationToken = default)

        => await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);


    public Task<bool> ExistAsyc(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default)
        => _dbSet.AnyAsync(predicate, cancellation);

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.Where(predicate).ToListAsync(cancellationToken);



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


}
