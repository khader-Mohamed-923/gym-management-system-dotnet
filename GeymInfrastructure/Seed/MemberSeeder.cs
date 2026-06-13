using GeymManagement.DbContexts;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Entities;
using GymManagement.Domain.ValueObjects;
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
              }
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
                }
            }
        };

            await context.Members.AddRangeAsync(testMembers);
            await context.SaveChangesAsync();
        }
    }


}
