using System.Linq.Expressions;

namespace GymManagement.Domain.Specifications;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>>? Criteria { get; }

    List<Expression<Func<TEntity, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
}
