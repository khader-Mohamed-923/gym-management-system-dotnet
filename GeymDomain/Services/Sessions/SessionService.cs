using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Sessions.Requests;
using GymManagement.Domain.DTOs.Sessions.Responses;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.Specifications;

namespace GymManagement.Domain.Services.Sessions;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IMemberRepository<Category> _categoryRepository;
    private readonly ITrainerRepository _trainerRepository;

    public SessionService(
        ISessionRepository sessionRepository,
        IMemberRepository<Category> categoryRepository,
        ITrainerRepository trainerRepository)
    {
        _sessionRepository = sessionRepository;
        _categoryRepository = categoryRepository;
        _trainerRepository = trainerRepository;
    }

    public async Task<Result<IEnumerable<SessionResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var spec = new SessionWithDetailsSpecification();
        var sessions = await _sessionRepository.GetListWithSpecAsync(spec, cancellationToken);

        var responses = new List<SessionResponse>();
        foreach (var session in sessions)
        {
            var bookedCount = await _sessionRepository.GetBookedCountAsync(session.Id, cancellationToken);
            responses.Add(new SessionResponse
            {
                Id = session.Id,
                Description = session.Description,
                Capacity = session.Capacity,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                CategoryName = session.Category?.Name ?? "Unknown",
                TrainerName = session.Trainer?.Name ?? "Unknown",
                TrainerId = session.TrainerId,
                CategoryId = session.CategoryId,
                BookedCount = bookedCount
            });
        }

        return Result<IEnumerable<SessionResponse>>.Success(responses);
    }

    public async Task<Result> CreateAsync(CreateSessionRequest request, CancellationToken cancellationToken)
    {
        if (request.EndDate <= request.StartDate)
        {
            return Result.Failure("End date must be after start date.", nameof(request.EndDate));
        }

        if (request.Capacity < 1 || request.Capacity > 25)
        {
            return Result.Failure("Capacity must be between 1 and 25.", nameof(request.Capacity));
        }

        var categoryExists = await _categoryRepository.ExistAsyc(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return Result.Failure("Category is required.", nameof(request.CategoryId));
        }

        var trainerExists = await _trainerRepository.ExistAsyc(t => t.Id == request.TrainerId, cancellationToken);
        if (!trainerExists)
        {
            return Result.Failure("Trainer is required.", nameof(request.TrainerId));
        }

        var session = new Session
        {
            Description = request.Description,
            Capacity = request.Capacity,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CategoryId = request.CategoryId,
            TrainerId = request.TrainerId
        };

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _sessionRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<SessionDetailsResponse?> GetDetailsAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new SessionWithDetailsSpecification(id);
        var session = await _sessionRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (session == null) return null;

        var bookedCount = await _sessionRepository.GetBookedCountAsync(id, cancellationToken);

        return new SessionDetailsResponse
        {
            Id = session.Id,
            Description = session.Description,
            Capacity = session.Capacity,
            StartDate = session.StartDate,
            EndDate = session.EndDate,
            CategoryName = session.Category?.Name ?? "Unknown",
            TrainerName = session.Trainer?.Name ?? "Unknown",
            TrainerEmail = session.Trainer?.Email ?? "N/A",
            TrainerPhone = session.Trainer?.Phone ?? "N/A",
            TrainerSpecialization = session.Trainer?.Speciality.ToString() ?? "N/A",
            BookedCount = bookedCount
        };
    }

    public async Task<SessionEditResponse?> GetForEditAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new BaseSpecification<Session>(s => s.Id == id);
        var session = await _sessionRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (session == null) return null;

        return new SessionEditResponse
        {
            Id = session.Id,
            Description = session.Description,
            Capacity = session.Capacity,
            StartDate = session.StartDate,
            EndDate = session.EndDate,
            CategoryId = session.CategoryId,
            TrainerId = session.TrainerId
        };
    }

    public async Task<Result> UpdateAsync(int id, UpdateSessionRequest request, CancellationToken cancellationToken)
    {
        if (request.EndDate <= request.StartDate)
        {
            return Result.Failure("End date must be after start date.", nameof(request.EndDate));
        }

        if (request.Capacity < 1 || request.Capacity > 25)
        {
            return Result.Failure("Capacity must be between 1 and 25.", nameof(request.Capacity));
        }

        var categoryExists = await _categoryRepository.ExistAsyc(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return Result.Failure("Category is required.", nameof(request.CategoryId));
        }

        var trainerExists = await _trainerRepository.ExistAsyc(t => t.Id == request.TrainerId, cancellationToken);
        if (!trainerExists)
        {
            return Result.Failure("Trainer is required.", nameof(request.TrainerId));
        }

        var spec = new BaseSpecification<Session>(s => s.Id == id);
        var session = await _sessionRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (session == null)
        {
            return Result.Failure("Session not found.", nameof(id));
        }

        session.Description = request.Description;
        session.Capacity = request.Capacity;
        session.StartDate = request.StartDate;
        session.EndDate = request.EndDate;
        session.CategoryId = request.CategoryId;
        session.TrainerId = request.TrainerId;

        await _sessionRepository.UpdateAsync(session);
        await _sessionRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new BaseSpecification<Session>(s => s.Id == id);
        var session = await _sessionRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (session == null)
        {
            return Result.Failure("Session not found.", nameof(id));
        }

        var now = DateTime.Now;
        if (session.StartDate > now)
        {
            return Result.Failure("Cannot delete sessions that are still in the future.", nameof(id));
        }

        await _sessionRepository.SoftDeleteAsync(session, cancellationToken);
        await _sessionRepository.SaveChangesAsync();

        return Result.Success();
    }
}
