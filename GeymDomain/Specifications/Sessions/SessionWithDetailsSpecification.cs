using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Specifications;

public class SessionWithDetailsSpecification : BaseSpecification<Session>
{
    public SessionWithDetailsSpecification() : base()
    {
        AddInclude(s => s.Category);
        AddInclude(s => s.Trainer);
    }

    public SessionWithDetailsSpecification(int id) : base(s => s.Id == id)
    {
        AddInclude(s => s.Category);
        AddInclude(s => s.Trainer);
    }
}
