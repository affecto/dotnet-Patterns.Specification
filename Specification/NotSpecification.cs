using System;

namespace Affecto.Patterns.Specification
{
    internal class NotSpecification<TEntity> : Specification<TEntity>
    {
        private readonly ISpecification<TEntity> wrapped;

        public NotSpecification(ISpecification<TEntity> spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException("spec");
            }
            wrapped = spec;
        }

        protected override bool IsSatisfied(TEntity candidate)
        {
            if (!wrapped.IsSatisfiedBy(candidate))
            {
                return true;
            }
            AddReasonForDissatisfaction(string.Format("NotSpecification for wrapped specification '{0}' was dissatisfied.", wrapped));
            return false;
        }
    }
}