using System;

namespace Affecto.Patterns.Specification
{
    internal class AndSpecification<TEntity> : Specification<TEntity>
    {
        private readonly ISpecification<TEntity> spec1;
        private readonly ISpecification<TEntity> spec2;

        internal AndSpecification(ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
        {
            if (spec1 == null)
            {
                throw new ArgumentNullException("spec1");
            }

            if (spec2 == null)
            {
                throw new ArgumentNullException("spec2");
            }

            this.spec1 = spec1;
            this.spec2 = spec2;
        }

        protected override bool IsSatisfied(TEntity candidate)
        {
            if (!spec1.IsSatisfiedBy(candidate))
            {
                AddReasonsForDissatisfaction(spec1.ReasonsForDissatisfaction);
                return false;
            }
            if (!spec2.IsSatisfiedBy(candidate))
            {
                AddReasonsForDissatisfaction(spec2.ReasonsForDissatisfaction);
                return false;
            }

            return true;
        }
    }
}