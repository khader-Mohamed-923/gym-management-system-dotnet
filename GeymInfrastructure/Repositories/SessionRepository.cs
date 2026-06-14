using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeymInfrastructure.Repositories;

public class SessionRepository : Repository<Session>, ISessionRepository
{
    private readonly GymDbContext _dbContext;

    public SessionRepository(GymDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> GetBookedCountAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bokings
            .CountAsync(b => b.SessionId == sessionId, cancellationToken);
    }

    public async Task<bool> HasActiveBookingsAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bokings
            .AnyAsync(b => b.SessionId == sessionId, cancellationToken);
    }
}
