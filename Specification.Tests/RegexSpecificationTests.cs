using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.Patterns.Specification.Tests
{
    [TestClass]
    public class RegexSpecificationTests
    {
        private const string ReasonForDissatisfaction = "Parameter '{0}' didn't meet specification.";
        private const string RegexPattern = @"\d";

        private RegexSpecification sut;

        [TestMethod]
        public void IsSatisfiedWhenRegexIsMatched()
        {
            sut = new RegexSpecification(RegexPattern, ReasonForDissatisfaction);

            Assert.IsTrue(sut.IsSatisfiedBy("A5"));
        }

        [TestMethod]
        public void IsNotSatisfiedWhenRegexIsNotMatched()
        {
            sut = new RegexSpecification(RegexPattern, ReasonForDissatisfaction);

            Assert.IsFalse(sut.IsSatisfiedBy("A"));
        }

        [TestMethod]
        public void NullIsNotSatisfactory()
        {
            sut = new RegexSpecification(RegexPattern, ReasonForDissatisfaction);

            Assert.IsFalse(sut.IsSatisfiedBy(null));
        }

        [TestMethod]
        public void ReasonForDissatisfactionIsSet()
        {
            const string value = "A";
            sut = new RegexSpecification(RegexPattern, ReasonForDissatisfaction);

            sut.IsSatisfiedBy(value);

            Assert.AreEqual(string.Format(ReasonForDissatisfaction, value), sut.ReasonsForDissatisfaction.Single());
        }
    }
}