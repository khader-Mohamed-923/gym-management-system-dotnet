using GeymManagement.DbContexts;
using GymManagement.Domain.Common;
using GymManagement.Domain.ViewModels.Trainer;
using GymManagement.Infrastructure.Enums;
using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.Repositories;
using GymManagement.Infrastructure.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Domain.Services.Trainers;

public class TrainerService(
    IMemberRepository<Trainer> trainerRepository,
    GymDbContext dbContext) : ITrainerService
{
    public async Task<Result<IEnumerable<TrainerIndexViewModel>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var trainers = await trainerRepository.GetAllAsync(cancellationToken);

        var viewModels = trainers.Select(t => new TrainerIndexViewModel
        {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Phone = t.Phone,
            Specialization = t.Speciality.ToString()
        });

        return Result<IEnumerable<TrainerIndexViewModel>>.Success(viewModels);
    }

    public async Task<Result> CreateAsync(TrainerCreateViewModel model, CancellationToken cancellationToken)
    {
        
        if (await dbContext.Users.AnyAsync(u => u.Email == model.Email, cancellationToken))
        {
            return Result.Failure("Email already exists.", nameof(model.Email));
        }

        if (await dbContext.Users.AnyAsync(u => u.Phone == model.Phone, cancellationToken))
        {
            return Result.Failure("Phone number already exists.", nameof(model.Phone));
        }

        if (!Enum.TryParse<Gender>(model.Gender, true, out var gender))
        {
            return Result.Failure("Invalid gender.", nameof(model.Gender));
        }

        if (!Enum.TryParse<Speciality>(model.Specialty, true, out var speciality))
        {
            return Result.Failure("Invalid specialty.", nameof(model.Specialty));
        }

        var trainer = new Trainer
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            DateOfBirth = model.DateOfBirth,
            Gender = gender,
            Speciality = speciality,
            HireDate = DateTime.UtcNow,
            Address = new Address
            {
                Street = model.Street,
                City = model.City,
                BuildingNumber = model.BuildingNumber
            }
        };

        await trainerRepository.AddAsync(trainer, cancellationToken);
        await trainerRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<TrainerDetailsViewModel?> GetDetailsAsync(int id, CancellationToken cancellationToken)
    {
        var trainer = await trainerRepository.GetByIdIncludedDeletedAsync(id, cancellationToken);

        if (trainer is null) return null;

        return new TrainerDetailsViewModel
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

    public async Task<TrainerEditViewModel?> GetForEditAsync(int id, CancellationToken cancellationToken)
    {
        var trainer = await trainerRepository.GetByIdIncludedDeletedAsync(id, cancellationToken);

        if (trainer == null) return null;

        return new TrainerEditViewModel
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

    public async Task<Result> UpdateAsync(int id, TrainerEditViewModel model, CancellationToken cancellationToken)
    {
        var normalizedEmail = model.Email.Trim().ToLower();
        var normalizedPhone = model.Phone.Trim();

        var trainer = await trainerRepository.GetByIdIncludedDeletedAsync(id, cancellationToken);

        if (trainer == null)
        {
            return Result.Failure("Trainer not found.", nameof(id));
        }

        if (await dbContext.Users.AnyAsync(u => u.Email == normalizedEmail && u.Id != id, cancellationToken))
        {
            return Result.Failure("Email is already taken by another user.", nameof(model.Email));
        }

        if (await dbContext.Users.AnyAsync(u => u.Phone == normalizedPhone && u.Id != id, cancellationToken))
        {
            return Result.Failure("Phone number is already taken by another user.", nameof(model.Phone));
        }

        if (!Enum.TryParse<Speciality>(model.Specialty, true, out var speciality))
        {
            return Result.Failure("Invalid specialty.", nameof(model.Specialty));
        }

        trainer.Email = normalizedEmail;
        trainer.Phone = normalizedPhone;
        trainer.Speciality = speciality;
        trainer.Address = new Address
        {
            Street = model.Street,
            City = model.City,
            BuildingNumber = model.BuildingNumber
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
