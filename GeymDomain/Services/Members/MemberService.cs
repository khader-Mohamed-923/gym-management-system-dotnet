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
        if (await memberRepository.ExistAsyc(e => e.Email == request.Email, cancellationToken))
        {
            return Result.Failure("Email already exists.", nameof(request.Email));
        }

        if (await memberRepository.ExistAsyc(e => e.Phone == request.Phone, cancellationToken))
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
            Hight = request.HealthRecord.Height,
            Weight = request.HealthRecord.Weight,
            BloodType = bloodType,
            Notes = request.HealthRecord.Note
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
            Hight = member.HealthRecord.Hight,
            Weight = member.HealthRecord.Weight,
            BloodType = member.HealthRecord.BloodType.ToString(),
            Note = member.HealthRecord.Notes
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
}
