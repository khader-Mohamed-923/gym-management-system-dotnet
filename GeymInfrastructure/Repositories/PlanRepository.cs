using GeymManagement.DbContexts;
using GymManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace GeymInfrastructure.Repositories;

internal class PlanRepository : IPlanRepository
{
    public readonly GymDbContext _dbContext ;

    public PlanRepository(GymDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(Plan plan)
        =>_dbContext.Add(plan);


    public void Delete(Plan plan)
    {
        _dbContext.Remove(plan);
    }

    public async Task<IEnumerable<Plan>> GetAllAsync()
    => await _dbContext.Plans.ToListAsync();

    public async Task<Plan?> GetPlanById(int id)
      => await _dbContext.Plans.FindAsync(id);

    public  async Task<int> SaveChangesAsync()
    => await _dbContext.SaveChangesAsync();

    public void Update(Plan plan)
      => _dbContext.Update(plan);
    
}   

  
