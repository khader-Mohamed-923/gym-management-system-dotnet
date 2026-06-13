using System.Linq.Expressions;

namespace GymManagement.Domain.Specifications;

public class BaseSpecification<TEntity> : ISpecification<TEntity>
{
    public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();

    public BaseSpecification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public BaseSpecification() { }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }
}
