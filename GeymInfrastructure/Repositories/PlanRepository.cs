using GeymManagement.DbContexts;
using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeymInfrastructure.Repositories;

internal class PlanRepository(GymDbContext dbContext) : Repository<Plan>(dbContext),IPlanRepository
{
   
   
    
}   

  
