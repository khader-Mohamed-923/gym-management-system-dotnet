

namespace GymManagement.Infrastructure.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Session> Sessions { get; set; } = [];

}
