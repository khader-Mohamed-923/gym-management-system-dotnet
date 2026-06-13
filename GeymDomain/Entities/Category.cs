namespace GymManagement.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Session> Sessions { get; set; } = [];

}
