using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Members;
using GymManagement.Domain.ViewModels.HealthRecord;
using GymManagement.Domain.ViewModels.Member;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.ValueObjects;
using GymManagement.Domain.Specifications.Members; 

namespace GymManagement.Domain.Services;

public class MemberService(IMemberRepository memberRepository) : IMemberService
{
    public async Task<Result<IEnumerable<MemberIndexViewModel>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var members = await memberRepository.GetAllAsync(cancellationToken);

        var viewModels = members.Select(m => new MemberIndexViewModel
        {
            Id = m.Id,
            Name = m.Name,
            Email = m.Email,
            PhoneNumber = m.Phone,
            PhotoPath = m.Photo,
            JoinDate = m.JoinDate,
            Gender = m.Gender.ToString()
        });

        return Result<IEnumerable<MemberIndexViewModel>>.Success(viewModels);
    }

    public async Task<Result> CreateAsync(MemberCreateViewModel model, CancellationToken cancellationToken)
    {
        if (await memberRepository.ExistAsyc(e => e.Email == model.Email, cancellationToken))
        {
            return Result.Failure("Email already exists.", nameof(model.Email));
        }

        if (await memberRepository.ExistAsyc(e => e.Phone == model.Phone, cancellationToken))
        {
            return Result.Failure("Phone number already exists.", nameof(model.Phone));
        }

        if (!Enum.TryParse<Gender>(model.Gender, true, out var gender))
        {
            return Result.Failure("Invalid gender.", nameof(model.Gender));
        }

        if (model.HealthRecord == null || !Enum.TryParse<BloodType>(model.HealthRecord.BloodType, true, out var bloodType))
        {
            return Result.Failure("Invalid blood type.", "HealthRecord.BloodType");
        }

        var member = new Member
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            DateOfBirth = model.DateOfBirth,
            Gender = gender,
            JoinDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Address = new Address
            {
                Street = model.Street,
                City = model.City,
                BuildingNumber = model.BuildingNumber
            }
        };

        var healthRecord = new HealthRecord
        {
            Hight = model.HealthRecord.Height ?? 0,
            Weight = model.HealthRecord.Weight ?? 0,
            BloodType = bloodType,
            Notes = model.HealthRecord.Note
        };

        member.HealthRecord = healthRecord;

        await memberRepository.AddAsync(member, cancellationToken);
        await memberRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<MemberDetailsViewModel?> GetDetailsAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new MemberDetailsWithPlanSpecification(id);

        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member is null) return null;

        var activeMembership = member.MemberShips.FirstOrDefault(m => m.EndDate >= DateTime.Today);

        return new MemberDetailsViewModel
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

    public async Task<HealthRecordDetailsViewModel?> GetHealthRecordAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new MemberHealthRecordSpecification(id);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member?.HealthRecord == null) return null;

        return new HealthRecordDetailsViewModel
        {
            Hight = member.HealthRecord.Hight, 
            Weight = member.HealthRecord.Weight,
            BloodType = member.HealthRecord.BloodType.ToString(),
            Note = member.HealthRecord.Notes
        };
    }

    public async Task<MemberEditViewModel?> GetForEditAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new MemberByIdSpecification(id);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member == null) return null;

        return new MemberEditViewModel
        {
            Id= member.Id,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            Photo = member.Photo, 
            BuildingNumber = member.Address?.BuildingNumber ?? 0,
            City = member.Address?.City ?? string.Empty,
            Street = member.Address?.Street ?? string.Empty
        };
    }

    public async Task<Result> UpdateAsync( int id,MemberEditViewModel model, CancellationToken cancellationToken)
    {
        var normalizedEmail = model.Email.Trim().ToLower();
        var normalizedPhone = model.Phone.Trim();
        var spec = new MemberByIdSpecification(id);
        var member = await memberRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (member == null)
        {
            return Result.Failure("Member not found.", nameof(id));
        }

        if(member.Name != model.Name)
            Result.Failure("Name cannot be changed.", nameof(model.Name));



        if (await memberRepository.IsEmailTakenAsync(model.Email, model.Id, cancellationToken))
        {
            return Result.Failure("Email is already taken by another member.", nameof(model.Email));
        }

        if (await memberRepository.IsPhoneTakenAsync(model.Phone, model.Id , cancellationToken))
        {
            return Result.Failure("Phone number is already taken by another member.", nameof(model.Phone));
        }

        member.Email = normalizedEmail;
        member.Phone = normalizedPhone;

        member.Address = new Address
        {
            Street = model.Street,
            City = model.City,
            BuildingNumber = model.BuildingNumber
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
