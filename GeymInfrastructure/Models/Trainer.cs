

using GymManagement.Infrastructure.Enums;

namespace GymManagement.Infrastructure.Models;

public class Trainer : User
{


    public Speciality Speciality { get; set; }

    public DateTime HireDate { get; set; }

    public ICollection<Session> Sessions { get; set; } = [];

}
