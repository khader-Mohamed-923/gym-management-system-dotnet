using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<bool> HasBookingAsync(int memberId, int sessionId, CancellationToken cancellationToken = default);
    Task<int> GetBookingCountAsync(int sessionId, CancellationToken cancellationToken = default);
    Task<Booking?> GetBookingWithDetailsAsync(int bookingId, CancellationToken cancellationToken = default);
    Task<Booking?> GetMemberBookingAsync(int memberId, int sessionId, CancellationToken cancellationToken = default);
    Task<Booking?> GetBookingIncludingDeletedAsync(int memberId, int sessionId, CancellationToken cancellationToken = default);
}
