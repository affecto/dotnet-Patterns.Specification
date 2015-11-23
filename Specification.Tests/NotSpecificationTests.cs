using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.Patterns.Specification.Tests
{
    [TestClass]
    public class NotSpecificationTests
    {
        private NotSpecification<string> sut;
        private ISpecification<string> spec;
        private const string SpecDissatisfaction = "specdis";
        private const string TestString = "test";

        [TestInitialize]
        public void Setup()
        {
            spec = Substitute.For<ISpecification<string>>();
            spec.ReasonsForDissatisfaction.Returns(new List<string> { SpecDissatisfaction });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpecIsMandatoryParameterInConstruction()
        {
            sut = new NotSpecification<string>(null);
        }

        [TestMethod]
        public void SpecificationIsNotSatisfiedWhenSpecIsSatisfied()
        {
            sut = new NotSpecification<string>(spec);
            spec.IsSatisfiedBy(TestString).Returns(true);
            Assert.IsFalse(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(1, sut.ReasonsForDissatisfaction.Count());
        }

        [TestMethod]
        public void SpecificationIsSatisfiedWhenSpecIsNotSatisfied()
        {
            sut = new NotSpecification<string>(spec);
            spec.IsSatisfiedBy(TestString).Returns(false);
            Assert.IsTrue(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(0, sut.ReasonsForDissatisfaction.Count());
        }

        [TestMethod]
        public void ReasonsForDissatisfactionIsClearedBeforeSatisfactionIsEvaluated()
        {
            sut = new NotSpecification<string>(spec);
            spec.IsSatisfiedBy(TestString).Returns(true, false);
            Assert.IsFalse(sut.IsSatisfiedBy(TestString));
            Assert.IsTrue(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(0, sut.ReasonsForDissatisfaction.Count());
        }
    }
}