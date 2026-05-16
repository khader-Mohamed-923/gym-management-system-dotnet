

namespace GymManagement.Infrastructure.Models;

public class Member : User
{

    public string? Photo { get; set; }

    public DateTime JoinDate { get; set; }

    public HealthRecord HealthRecord { get; set; } = null!;


    public ICollection<MemberShip> MemberShips { get; set; } = [];

    public ICollection<Boking> Bokings { get; set; } = [];


}
