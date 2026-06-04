

using GymManagement.Infrastructure.Enums;

namespace GymManagement.Infrastructure.Models;

public class HealthRecord : BaseEntity
{
    public decimal Hight { get; set; }

    public decimal Weight { get; set; }

    public BloodType BloodType { get; set; }

    public string? Notes { get; set; }
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
}
