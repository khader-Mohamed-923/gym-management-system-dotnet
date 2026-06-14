using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeymInfrastructure.Repositories;

public class MembershipRepository : Repository<MemberShip>, IMembershipRepository
{
    private readonly GymDbContext _dbContext;

    public MembershipRepository(GymDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> HasActiveMembershipAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        return await _dbContext.Set<MemberShip>().AnyAsync(m => 
            m.MemberId == memberId && 
            !m.IsDeleted && 
            m.EndDate >= now, cancellationToken);
    }

    public async Task<MemberShip?> GetActiveMembershipAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        return await _dbContext.Set<MemberShip>().FirstOrDefaultAsync(m => 
            m.MemberId == memberId && 
            !m.IsDeleted && 
            m.EndDate >= now, cancellationToken);
    }
}
