namespace GymManagement.Domain.Entities;

public class HealthRecord : BaseEntity
{
    public int MemberId { get; set; }  
    public Member Member { get; set; } = null!;
    public string BloodType { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    public string MedicalConditions { get; set; } = string.Empty;
}