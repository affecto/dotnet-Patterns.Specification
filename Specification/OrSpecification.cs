using System;

namespace Affecto.Patterns.Specification
{
    internal class OrSpecification<TEntity> : Specification<TEntity>
    {
        private readonly ISpecification<TEntity> spec1;
        private readonly ISpecification<TEntity> spec2;

        public OrSpecification(ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
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
            if (spec1.IsSatisfiedBy(candidate))
            {
                return true;
            }
            if (spec2.IsSatisfiedBy(candidate))
            {
                return true;
            }

            AddReasonsForDissatisfaction(spec1.ReasonsForDissatisfaction);
            AddReasonsForDissatisfaction(spec2.ReasonsForDissatisfaction);

            return false;
        }
    }
}