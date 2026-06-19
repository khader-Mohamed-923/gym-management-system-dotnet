using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Entities;
using GymManagement.Domain.ValueObjects;
using GymManagement.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.Infrastructure.Seed;


public static class MemberSeeder
{

    public static async Task SeedAsync(GymDbContext context)
    {
        if (!context.Members.Any())
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var appUser1 = await context.Set<ApplicationUser>().FirstOrDefaultAsync(u => u.Email == "khader.dev@gmail.com");
            if (appUser1 == null)
            {
                appUser1 = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "khader.dev@gmail.com",
                    NormalizedUserName = "KHADER.DEV@GMAIL.COM",
                    Email = "khader.dev@gmail.com",
                    NormalizedEmail = "KHADER.DEV@GMAIL.COM",
                    FirstName = "Khader",
                    LastName = "Mohamed",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                appUser1.PasswordHash = hasher.HashPassword(appUser1, "Password123!");
                await context.Set<ApplicationUser>().AddAsync(appUser1);
            }

            var appUser2 = await context.Set<ApplicationUser>().FirstOrDefaultAsync(u => u.Email == "ahmed.ali@gmail.com");
            if (appUser2 == null)
            {
                appUser2 = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "ahmed.ali@gmail.com",
                    NormalizedUserName = "AHMED.ALI@GMAIL.COM",
                    Email = "ahmed.ali@gmail.com",
                    NormalizedEmail = "AHMED.ALI@GMAIL.COM",
                    FirstName = "Ahmed",
                    LastName = "Ali",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                appUser2.PasswordHash = hasher.HashPassword(appUser2, "Password123!");
                await context.Set<ApplicationUser>().AddAsync(appUser2);
            }

            var testMembers = new List<Member>
            {
                new Member
                {
                    Name = "Khader Mohamed",
                    Email = "khader.dev@gmail.com",
                    Phone = "01012345678",
                    JoinDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Gender = Gender.Male, 
                    Address = new Address
                    {
                        Street = "Main Street",
                        City = "Basyoun",
                        BuildingNumber = 12
                    },
                    ApplicationUserId = appUser1.Id
                },
                new Member
                {
                    Name = "Ahmed Ali",
                    Email = "ahmed.ali@gmail.com",
                    Phone = "01234567890",
                    JoinDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Gender = Gender.Male,
                    Address = new Address
                    {
                        Street = "El-Galaa Street",
                        City = "Tanta",
                        BuildingNumber = 4
                    },
                    ApplicationUserId = appUser2.Id
                }
            };

            await context.Members.AddRangeAsync(testMembers);
            await context.SaveChangesAsync();
        }

        if (!await context.Set<MemberShip>().AnyAsync())
        {
            var plans = await context.Plans.ToListAsync();
            var members = await context.Members.ToListAsync();

            if (plans.Any() && members.Any())
            {
                var memberships = new List<MemberShip>();
                var planAssignments = new[] { "Basic Plan", "Premium Plan", "VIP Plan" };

                for (int i = 0; i < members.Count; i++)
                {
                    var plan = plans.FirstOrDefault(p => p.Name == planAssignments[i % planAssignments.Length])
                               ?? plans[i % plans.Count];

                    var isActive = true;

                    memberships.Add(new MemberShip
                    {
                        MemberId = members[i].Id,
                        PlanId = plan.Id,
                        StartDate = DateTime.Now.AddDays(-15),
                        EndDate = isActive ? DateTime.Now.AddMonths(1) : DateTime.Now.AddMonths(-1)
                    });
                }

                await context.Set<MemberShip>().AddRangeAsync(memberships);
                await context.SaveChangesAsync();
            }
        }
    }
}
