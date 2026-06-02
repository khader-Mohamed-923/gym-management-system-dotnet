
namespace GymManagement.Infrastructure.ValueObjects;

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    public int BuildingNumber { get; set; }

}
