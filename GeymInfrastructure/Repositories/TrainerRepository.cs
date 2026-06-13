using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeymInfrastructure.Repositories;

public class TrainerRepository : Repository<Trainer>, ITrainerRepository
{
    private readonly GymDbContext _dbContext;

    public TrainerRepository(GymDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default)
        => await _dbContext.Set<User>().AnyAsync(u => u.Email == email && (!excludeId.HasValue || u.Id != excludeId.Value), cancellationToken);

    public async Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken cancellationToken = default)
        => await _dbContext.Set<User>().AnyAsync(u => u.Phone == phone && (!excludeId.HasValue || u.Id != excludeId.Value), cancellationToken);
}
