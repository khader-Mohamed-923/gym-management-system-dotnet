using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeymInfrastructure.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    private readonly GymDbContext _dbContext;

    public MemberRepository(GymDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> HasUpcomingBookingsAsync(int id, DateTime dateTime, CancellationToken cancellationToken)
    {
        return await _dbContext.Bokings 
            .AnyAsync(b => b.MemberId == id && b.Session != null && b.Session.EndDate >= dateTime, cancellationToken);
    }

    public async Task<bool> IsEmailTakenAsync(string normalizedEmail, int? excludeId = null, CancellationToken cancellationToken = default)
        => await _dbContext.Set<Member>().AnyAsync(m => m.Email == normalizedEmail && (!excludeId.HasValue || m.Id != excludeId.Value), cancellationToken);

    public async Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken cancellationToken = default)
        => await _dbContext.Set<Member>().AnyAsync(m => m.Phone == phone && (!excludeId.HasValue || m.Id != excludeId.Value), cancellationToken);
}
