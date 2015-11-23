using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.Patterns.Specification.Tests
{
    [TestClass]
    public class SpecificationTests
    {
        private const string FirstReasonForDissatisfaction = "Reason1";
        private const string SecondReasonForDissatisfaction = "Reason2";

        private FailingSpecification failingSpecification;
        private PassingSpecification passingSpecification;

        [TestInitialize]
        public void Setup()
        {
            failingSpecification = new FailingSpecification();
            passingSpecification = new PassingSpecification();
        }
        
        [TestMethod]
        public void ReasonsForDissatisfactionAreCollected()
        {
            failingSpecification.IsSatisfiedBy("entity");

            List<string> reasons = failingSpecification.ReasonsForDissatisfaction.ToList();

            Assert.AreEqual(2, reasons.Count);
            Assert.IsTrue(reasons.Contains(FirstReasonForDissatisfaction));
            Assert.IsTrue(reasons.Contains(SecondReasonForDissatisfaction));
        }

        [TestMethod]
        public void ReasonsForDissatisfactionCanBeConcatenatedToOneString()
        {
            failingSpecification.IsSatisfiedBy("entity");

            string reasons = failingSpecification.GetReasonsForDissatisfactionSeparatedWithNewLine();

            Assert.AreEqual(string.Format("{0}\r\n{1}", FirstReasonForDissatisfaction, SecondReasonForDissatisfaction), reasons);
        }

        [TestMethod]
        public void ReasonsForDissatisfactionIsEmptyWhenSpecificationPasses()
        {
            passingSpecification.IsSatisfiedBy("entity");

            IEnumerable<string> reasons = passingSpecification.ReasonsForDissatisfaction;

            Assert.IsFalse(reasons.Any());
        }

        [TestMethod]
        public void ConcatenatedReasonsForDissatisfactionIsEmptyWhenSpecificationPasses()
        {
            passingSpecification.IsSatisfiedBy("entity");

            string reasons = passingSpecification.GetReasonsForDissatisfactionSeparatedWithNewLine();

            Assert.IsTrue(string.IsNullOrEmpty(reasons));
        }

        private class FailingSpecification : Specification<string>
        {
            protected override bool IsSatisfied(string entity)
            {
                AddReasonForDissatisfaction(FirstReasonForDissatisfaction);
                AddReasonForDissatisfaction(SecondReasonForDissatisfaction);
                return false;
            }
        }

        private class PassingSpecification : Specification<string>
        {
            protected override bool IsSatisfied(string entity)
            {
                return true;
            }
        }
    }
}