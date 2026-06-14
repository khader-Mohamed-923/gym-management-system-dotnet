using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Entities;

public class Trainer : User
{


    public Speciality Speciality { get; set; }

    public DateTime HireDate { get; set; }

    public ICollection<Session> Sessions { get; set; } = [];

}
