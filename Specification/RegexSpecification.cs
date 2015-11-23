using System;
using System.Text.RegularExpressions;

namespace Affecto.Patterns.Specification
{
    public class RegexSpecification : Specification<string>
    {
        private readonly string regexPattern;
        private readonly string reasonForDissatisfactionWithParameterPlaceholder;

        public RegexSpecification(string regexPattern, string reasonForDissatisfactionWithParameterPlaceholder)
        {
            if (string.IsNullOrWhiteSpace(regexPattern))
            {
                throw new ArgumentException("Value must be given.", "regexPattern");
            }
            if (string.IsNullOrWhiteSpace(reasonForDissatisfactionWithParameterPlaceholder))
            {
                throw new ArgumentException("Value must be given.", "reasonForDissatisfactionWithParameterPlaceholder");
            }

            this.regexPattern = regexPattern;
            this.reasonForDissatisfactionWithParameterPlaceholder = reasonForDissatisfactionWithParameterPlaceholder;
        }

        protected override bool IsSatisfied(string entity)
        {
            if (entity != null && Regex.IsMatch(entity, regexPattern))
            {
                return true;
            }
            AddReasonForDissatisfaction(string.Format(reasonForDissatisfactionWithParameterPlaceholder, entity));
            return false;
        }
    }
}