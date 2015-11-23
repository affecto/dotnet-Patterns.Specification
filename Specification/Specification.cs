using System.Collections.Generic;
using System.Text;

namespace Affecto.Patterns.Specification
{
    public abstract class Specification<TEntity> : ISpecification<TEntity>
    {
        private readonly List<string> reasonsForDissatisfaction;        

        protected Specification()
        {
            reasonsForDissatisfaction = new List<string>();
        }

        protected abstract bool IsSatisfied(TEntity entity);

        public IEnumerable<string> ReasonsForDissatisfaction
        {
            get { return reasonsForDissatisfaction; }
        }

        public string GetReasonsForDissatisfactionSeparatedWithNewLine()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string reason in reasonsForDissatisfaction)
            {
                sb.AppendLine(reason);
            }
            if (sb.Length > 2)
            {
                sb = sb.Remove(sb.Length - 2, 2);
            }
            return sb.ToString();
        }

        public bool IsSatisfiedBy(TEntity entity)
        {
            reasonsForDissatisfaction.Clear();
            return IsSatisfied(entity);
        }

        protected void AddReasonForDissatisfaction(string reason)
        {
            reasonsForDissatisfaction.Add(reason);
        }

        protected void AddReasonsForDissatisfaction(IEnumerable<string> reasons)
        {
            reasonsForDissatisfaction.AddRange(reasons);
        }
    }
}