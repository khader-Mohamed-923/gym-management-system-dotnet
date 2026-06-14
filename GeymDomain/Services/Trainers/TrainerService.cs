using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Trainers.Requests;
using GymManagement.Domain.DTOs.Trainers.Responses;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.ValueObjects;

namespace GymManagement.Domain.Services.Trainers;

public class TrainerService(ITrainerRepository trainerRepository) : ITrainerService
{
    public async Task<Result<IEnumerable<TrainerResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var trainers = await trainerRepository.GetAllAsync(cancellationToken);

        var responses = trainers.Select(t => new TrainerResponse
        {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Phone = t.Phone,
            Specialization = t.Speciality.ToString()
        });

        return Result<IEnumerable<TrainerResponse>>.Success(responses);
    }

    public async Task<Result> CreateAsync(CreateTrainerRequest request, CancellationToken cancellationToken)
    {
        if (await trainerRepository.IsEmailTakenAsync(request.Email, null, cancellationToken))
        {
            return Result.Failure("Email already exists.", nameof(request.Email));
        }

        if (await trainerRepository.IsPhoneTakenAsync(request.Phone, null, cancellationToken))
        {
            return Result.Failure("Phone number already exists.", nameof(request.Phone));
        }

        if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
        {
            return Result.Failure("Invalid gender.", nameof(request.Gender));
        }

        if (!Enum.TryParse<Speciality>(request.Specialty, true, out var speciality))
        {
            return Result.Failure("Invalid specialty.", nameof(request.Specialty));
        }

        var trainer = new Trainer
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            Gender = gender,
            Speciality = speciality,
            HireDate = DateTime.UtcNow,
            Address = new Address
            {
                Street = request.Street,
                City = request.City,
                BuildingNumber = request.BuildingNumber
            }
        };

        await trainerRepository.AddAsync(trainer, cancellationToken);
        await trainerRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<TrainerDetailsResponse?> GetDetailsAsync(int id, CancellationToken cancellationToken)
    {
        var trainer = await trainerRepository.GetByIdIncludedDeletedAsync(id, cancellationToken);

        if (trainer is null) return null;

        return new TrainerDetailsResponse
        {
            Id = trainer.Id,
            Name = trainer.Name,
            Email = trainer.Email,
            Phone = trainer.Phone,
            DateOfBirth = trainer.DateOfBirth.ToString("dd MMM yyyy"),
            Specialization = trainer.Speciality.ToString(),
            Address = $"{trainer.Address.BuildingNumber} · {trainer.Address.Street} · {trainer.Address.City}"
        };
    }

    public async Task<TrainerEditResponse?> GetForEditAsync(int id, CancellationToken cancellationToken)
    {
        var trainer = await trainerRepository.GetByIdIncludedDeletedAsync(id, cancellationToken);

        if (trainer == null) return null;

        return new TrainerEditResponse
        {
            Id = trainer.Id,
            Name = trainer.Name,
            DateOfBirth = trainer.DateOfBirth.ToString("dd MMM yyyy"),
            Gender = trainer.Gender.ToString(),
            Email = trainer.Email,
            Phone = trainer.Phone,
            Specialty = trainer.Speciality.ToString(),
            BuildingNumber = trainer.Address?.BuildingNumber ?? 0,
            City = trainer.Address?.City ?? string.Empty,
            Street = trainer.Address?.Street ?? string.Empty
        };
    }

    public async Task<Result> UpdateAsync(int id, UpdateTrainerRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        var normalizedPhone = request.Phone.Trim();

        var trainer = await trainerRepository.GetByIdIncludedDeletedAsync(id, cancellationToken);

        if (trainer == null)
        {
            return Result.Failure("Trainer not found.", nameof(id));
        }

        if (await trainerRepository.IsEmailTakenAsync(normalizedEmail, id, cancellationToken))
        {
            return Result.Failure("Email is already taken by another user.", nameof(request.Email));
        }

        if (await trainerRepository.IsPhoneTakenAsync(normalizedPhone, id, cancellationToken))
        {
            return Result.Failure("Phone number is already taken by another user.", nameof(request.Phone));
        }

        if (!Enum.TryParse<Speciality>(request.Specialty, true, out var speciality))
        {
            return Result.Failure("Invalid specialty.", nameof(request.Specialty));
        }

        trainer.Email = normalizedEmail;
        trainer.Phone = normalizedPhone;
        trainer.Speciality = speciality;
        trainer.Address = new Address
        {
            Street = request.Street,
            City = request.City,
            BuildingNumber = request.BuildingNumber
        };

        await trainerRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var trainer = await trainerRepository.GetByIdIncludedDeletedAsync(id, cancellationToken);

        if (trainer == null)
        {
            return Result.Failure("Trainer not found.", nameof(id));
        }

        await trainerRepository.SoftDeleteAsync(trainer, cancellationToken);
        await trainerRepository.SaveChangesAsync();

        return Result.Success();
    }
}
