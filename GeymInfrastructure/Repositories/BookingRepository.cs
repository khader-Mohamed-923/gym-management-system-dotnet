using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeymInfrastructure.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    private readonly GymDbContext _dbContext;

    public BookingRepository(GymDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> HasBookingAsync(int memberId, int sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .AnyAsync(b => b.MemberId == memberId && b.SessionId == sessionId && !b.IsDeleted, cancellationToken);
    }

    public async Task<int> GetBookingCountAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .CountAsync(b => b.SessionId == sessionId && !b.IsDeleted, cancellationToken);
    }

    public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .Include(b => b.Session)
                .ThenInclude(s => s!.Category)
            .Include(b => b.Session)
                .ThenInclude(s => s!.Trainer)
            .Include(b => b.Member)
            .FirstOrDefaultAsync(b => b.Id == bookingId && !b.IsDeleted, cancellationToken);
    }

    public async Task<Booking?> GetMemberBookingAsync(int memberId, int sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId && !b.IsDeleted, cancellationToken);
    }

    public async Task<Booking?> GetBookingIncludingDeletedAsync(int memberId, int sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, cancellationToken);
    }
}
