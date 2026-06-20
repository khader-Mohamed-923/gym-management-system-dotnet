using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Members;
using GymManagement.Domain.DTOs.Members.Requests;
using GymManagement.Domain.DTOs.Members.Responses;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.ValueObjects;
using GymManagement.Domain.Specifications.Members;
using Mapster;

namespace GymManagement.Domain.Services;

public class MemberService(IMemberRepository memberRepository) : IMemberService
{
    public async Task<Result<IEnumerable<MemberResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var members = await memberRepository.GetAllAsync(cancellationToken);

        var responses = members.Select(m => new MemberResponse
        {
            Id = m.Id,
            Name = m.Name,
            Email = m.Email,
            PhoneNumber = m.Phone,
            PhotoPath = m.Photo,
            JoinDate = m.JoinDate,
            Gender = m.Gender.ToString()
        });

        return Result<IEnumerable<MemberResponse>>.Success(responses);
    }

    public async Task<Result> CreateAsync(CreateMemberRequest request, CancellationToken cancellationToken)
    {
        if (await memberRepository.ExistsAsync(e => e.Email == request.Email, cancellationToken))
        {
            return Result.Failure("Email already exists.", nameof(request.Email));
        }

        if (await memberRepository.ExistsAsync(e => e.Phone == request.Phone, cancellationToken))
        {
            return Result.Failure("Phone number already exists.", nameof(request.Phone));
        }

        if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
        {
            return Result.Failure("Invalid gender.", nameof(request.Gender));
        }

        if (!Enum.TryParse<BloodType>(request.HealthRecord.BloodType, true, out var bloodType))
        {
            return Result.Failure("Invalid blood type.", "HealthRecord.BloodType");
        }

        var member = new Member
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            Gender = gender,
            Photo = request.PhotoUrl,
            JoinDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Address = new Address
            {
                Street = request.Street,
                City = request.City,
                BuildingNumber = request.BuildingNumber
            }
        };

        var healthRecord = new HealthRecord
        {
            Height = request.HealthRecord.Height,
            Weight = request.HealthRecord.Weight,
            BloodType = bloodType.ToString(),
            MedicalConditions = request.HealthRecord.Note ?? string.Empty
        };

        member.HealthRecord = healthRecord;

        await memberRepository.AddAsync(member, cancellationToken);
        await memberRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<MemberDetailsResponse?> GetDetailsAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new MemberDetailsWithPlanSpecification(id);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member is null) return null;

        var activeMembership = member.MemberShips.FirstOrDefault(m => m.EndDate >= DateTime.Today);

        return new MemberDetailsResponse
        {
            Id = member.Id,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            PhotoUrl = member.Photo,
            Gender = member.Gender.ToString(),
            DateOfBirth = member.DateOfBirth.ToShortDateString(),
            Address = $"{member.Address.Street}, {member.Address.BuildingNumber}, {member.Address.City}",
            PlanName = activeMembership?.Plan?.Name ?? "No Active Membership",
            MembershipStartDate = activeMembership?.StartDate.ToShortDateString() ?? "N/A",
            MembershipEndDate = activeMembership?.EndDate.ToShortDateString() ?? "N/A"
        };
    }

    public async Task<HealthRecordResponse?> GetHealthRecordAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new MemberHealthRecordSpecification(id);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member?.HealthRecord == null) return null;

        return new HealthRecordResponse
        {
            Height = member.HealthRecord.Height,
            Weight = member.HealthRecord.Weight,
            BloodType = member.HealthRecord.BloodType,
            Note = member.HealthRecord.MedicalConditions
        };
    }

    public async Task<MemberEditResponse?> GetForEditAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new MemberByIdSpecification(id);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member == null) return null;

        return new MemberEditResponse
        {
            Id = member.Id,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            Photo = member.Photo,
            BuildingNumber = member.Address?.BuildingNumber ?? 0,
            City = member.Address?.City ?? string.Empty,
            Street = member.Address?.Street ?? string.Empty
        };
    }

    public async Task<Result> UpdateAsync(int id, UpdateMemberRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        var normalizedPhone = request.Phone.Trim();
        var spec = new MemberByIdSpecification(id);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member == null)
        {
            return Result.Failure("Member not found.", nameof(id));
        }

        if (await memberRepository.IsEmailTakenAsync(request.Email, id, cancellationToken))
        {
            return Result.Failure("Email is already taken by another member.", nameof(request.Email));
        }

        if (await memberRepository.IsPhoneTakenAsync(request.Phone, id, cancellationToken))
        {
            return Result.Failure("Phone number is already taken by another member.", nameof(request.Phone));
        }

        member.Email = normalizedEmail;
        member.Phone = normalizedPhone;
        member.Address = new Address
        {
            Street = request.Street,
            City = request.City,
            BuildingNumber = request.BuildingNumber
        };

        await memberRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var memberSpec = new MemberByIdSpecification(id);
        var member = await memberRepository.GetEntityWithSpecAsync(memberSpec, cancellationToken);

        if (member == null)
        {
            return Result.Failure("Member not found.", nameof(id));
        }

        var now = DateTime.Now;
        var hasUpcomingBookings = await memberRepository.HasUpcomingBookingsAsync(id, now, cancellationToken);

        if (hasUpcomingBookings)
        {
            return Result.Failure("Cannot delete member with upcoming bookings.", nameof(id));
        }

        await memberRepository.SoftDeleteAsync(member, cancellationToken);
        await memberRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<int?> GetIdByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var spec = new MemberByApplicationUserIdSpecification(userId);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);
        return member?.Id;
    }

    public async Task<MemberProfileResponse?> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var spec = new MemberByApplicationUserIdSpecification(userId);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member == null) return null;

        return new MemberProfileResponse
        {
            Id = member.Id,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            PhotoUrl = member.Photo ?? string.Empty,
            Gender = member.Gender.ToString(),
            DateOfBirth = member.DateOfBirth != DateOnly.MinValue ? member.DateOfBirth.ToString("MMM dd, yyyy") : "N/A",
            JoinDate = member.JoinDate.ToString("MMM dd, yyyy"),
            Address = member.Address != null
                ? member.Address.BuildingNumber > 0
                    ? $"{member.Address.BuildingNumber} {member.Address.Street}, {member.Address.City}"
                    : !string.IsNullOrWhiteSpace(member.Address.Street)
                        ? $"{member.Address.Street}, {member.Address.City}"
                        : "Not provided"
                : "Not provided",
            Street = member.Address?.Street ?? string.Empty,
            City = member.Address?.City ?? string.Empty,
            BuildingNumber = member.Address?.BuildingNumber ?? 0,
            HealthRecord = member.HealthRecord != null ? new HealthRecordResponse
            {
                Height = member.HealthRecord.Height,
                Weight = member.HealthRecord.Weight,
                BloodType = member.HealthRecord.BloodType,
                Note = member.HealthRecord.MedicalConditions
            } : new HealthRecordResponse()
        };
    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateMemberProfileRequest request, CancellationToken cancellationToken = default)
    {
        var spec = new MemberByApplicationUserIdSpecification(userId);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member == null)
            return Result.Failure("Member not found", "User");

        if (member.Address == null)
            member.Address = new Address();
            
        member.Address.Street = request.Street ?? string.Empty;
        member.Address.City = request.City ?? string.Empty;
        member.Address.BuildingNumber = request.BuildingNumber;

        if (member.HealthRecord == null)
            member.HealthRecord = new HealthRecord();

        member.HealthRecord.Height = request.Height;
        member.HealthRecord.Weight = request.Weight;
        member.HealthRecord.BloodType = request.BloodType ?? "Unknown";
        member.HealthRecord.MedicalConditions = request.Notes ?? string.Empty;

        await memberRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<IEnumerable<MemberBookingResponse>> GetBookingsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var spec = new MemberByApplicationUserIdSpecification(userId);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member == null || member.Bookings == null) return Enumerable.Empty<MemberBookingResponse>();

        return member.Bookings.Select(b => new MemberBookingResponse
        {
            BookingId = b.Id,
            SessionName = b.Session?.Description ?? "Unknown",
            TrainerName = b.Session?.Trainer?.Name ?? "Unassigned",
            Date = b.Session?.StartDate.ToShortDateString() ?? "N/A",
            Time = b.Session != null ? $"{b.Session.StartDate:HH:mm} - {b.Session.EndDate:HH:mm}" : "N/A",
            Status = b.Session?.StartDate >= DateTime.Now ? SessionStatus.Upcoming : SessionStatus.Completed
        }).OrderByDescending(b => b.Status == SessionStatus.Upcoming).ThenBy(b => b.Date);
    }

    public async Task<IEnumerable<MemberMembershipResponse>> GetMembershipsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var spec = new MemberByApplicationUserIdSpecification(userId);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member == null || member.MemberShips == null) return Enumerable.Empty<MemberMembershipResponse>();

        return member.MemberShips.Select(m => new MemberMembershipResponse
        {
            MembershipId = m.Id,
            PlanName = m.Plan?.Name ?? "Unknown Plan",
            Price = m.Plan?.Price ?? 0,
            StartDate = m.StartDate.ToShortDateString(),
            EndDate = m.EndDate.ToShortDateString(),
            IsActive = m.EndDate >= DateTime.Today
        }).OrderByDescending(m => m.IsActive).ThenByDescending(m => m.EndDate);
    }

    public async Task<Result<MemberDashboardResponse>> GetMemberDashboardAsync(string userId, CancellationToken cancellationToken = default)
    {
        var dashboardData = await memberRepository.GetDashboardDataAsync(userId, cancellationToken);
        
        if (dashboardData == null)
            return Result<MemberDashboardResponse>.Failure("Member not found", "User");
            
        return Result<MemberDashboardResponse>.Success(dashboardData);
    }
}

