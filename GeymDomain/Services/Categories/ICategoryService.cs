using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Services.Categories;

public interface ICategoryService
{
    Task<IReadOnlyList<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
}
