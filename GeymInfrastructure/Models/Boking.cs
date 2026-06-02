

namespace GymManagement.Infrastructure.Models;

public class Boking : BaseEntity
{

    public DateTime Date { get; set; }

    public bool IsAttended { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;


    public int MemberId { get; set; }

    public Member Member { get; set; }=null!;


}
