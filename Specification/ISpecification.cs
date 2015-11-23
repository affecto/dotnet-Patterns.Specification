using System.Collections.Generic;

namespace Affecto.Patterns.Specification
{
    public interface ISpecification<in TEntity>
    {
        IEnumerable<string> ReasonsForDissatisfaction { get; }
        string GetReasonsForDissatisfactionSeparatedWithNewLine();
        bool IsSatisfiedBy(TEntity entity);
    }
}