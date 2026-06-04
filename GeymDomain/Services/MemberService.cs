using GymManagement.Domain.Common;
using GymManagement.Domain.ViewModels.Member;
using GymManagement.Infrastructure.Enums;
using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.Repositories;
using GymManagement.Infrastructure.ValueObjects;

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

    public async Task<Result> CreateAsync(CreateMemberViewModel model, CancellationToken cancellationToken)
    {
        if (await memberRepository.ExistAsyc(e => e.Email == model.Email, cancellationToken))
        {
            return Result.Failure("Email already exists.", nameof(model.Email));
        }

        if (await memberRepository.ExistAsyc(e => e.Phone == model.Phone, cancellationToken))
        {
            return Result.Failure("Phone number already exists.", nameof(model.Phone));
        }

        // 🌟 تعديل جوهري: تحويل الـ Gender بأمان مع تحديد النوع <Gender> وتمرير true لتجاهل الحالة
        if (!Enum.TryParse<Gender>(model.Gender, true, out var gender))
        {
            return Result.Failure("Invalid gender.", nameof(model.Gender));
        }

        // 🌟 تعديل جوهري: تحويل الـ BloodType بأمان
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
            JoinDate = DateTime.UtcNow,
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
        await memberRepository.SaveChangesAsync(); // الحفظ الفعلي في الـ SQL Server

        return Result.Success();
    }
}