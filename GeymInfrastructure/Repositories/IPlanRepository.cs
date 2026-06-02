using GymManagement.Infrastructure.Models;

namespace GeymInfrastructure.Repositories;

public interface IPlanRepository
{
    Task<IEnumerable<Plan>> GetAllAsync();
    Task<Plan?> GetPlanById(int id);
    void Add(Plan plan);
    void Update(Plan plan);
    void Delete(Plan plan);
    Task<int> SaveChangesAsync();
}
