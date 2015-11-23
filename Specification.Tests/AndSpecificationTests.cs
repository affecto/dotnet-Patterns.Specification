using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.Patterns.Specification.Tests
{
    [TestClass]
    public class AndSpecificationTests
    {
        private AndSpecification<string> sut;
        private ISpecification<string> spec1;
        private ISpecification<string> spec2;
        private const string TestString = "test";
        private const string Spec1Dissatisfaction = "spec1dis";
        private const string Spec2Dissatisfaction = "spec2dis";

        [TestInitialize]
        public void Setup()
        {
            spec1 = Substitute.For<ISpecification<string>>();
            spec2 = Substitute.For<ISpecification<string>>();
            spec1.ReasonsForDissatisfaction.Returns(new List<string> { Spec1Dissatisfaction });
            spec2.ReasonsForDissatisfaction.Returns(new List<string> { Spec2Dissatisfaction });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Spec1IsMandatoryParameterInConstruction()
        {
            sut = new AndSpecification<string>(null, spec2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Spec2IsMandatoryParameterInConstruction()
        {
            sut = new AndSpecification<string>(spec1, null);
        }

        [TestMethod]
        public void SpecificationIsNotSatisfiedWhenSpec1IsNotSatisfied()
        {
            sut = new AndSpecification<string>(spec1, spec2);
            spec1.IsSatisfiedBy(TestString).Returns(false);
            Assert.IsFalse(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(1, sut.ReasonsForDissatisfaction.Count());
            Assert.AreEqual(Spec1Dissatisfaction, sut.ReasonsForDissatisfaction.Single());
        }

        [TestMethod]
        public void SpecificationIsNotSatisfiedWhenSpec2IsNotSatisfied()
        {
            sut = new AndSpecification<string>(spec1, spec2);
            spec1.IsSatisfiedBy(TestString).Returns(true);
            spec2.IsSatisfiedBy(TestString).Returns(false);
            Assert.IsFalse(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(1, sut.ReasonsForDissatisfaction.Count());
            Assert.AreEqual(Spec2Dissatisfaction, sut.ReasonsForDissatisfaction.Single());
        }

        [TestMethod]
        public void Spec2IsNotEvaluatedWhenSpec1IsNotSatisfied()
        {
            sut = new AndSpecification<string>(spec1, spec2);
            spec1.IsSatisfiedBy(TestString).Returns(false);
            Assert.IsFalse(sut.IsSatisfiedBy(TestString));
            spec2.DidNotReceive().IsSatisfiedBy(TestString);
        }

        [TestMethod]
        public void SpecificationIsSatisfiedWhenSpecificationIsSatisfied()
        {
            sut = new AndSpecification<string>(spec1, spec2);
            spec1.IsSatisfiedBy(TestString).Returns(true);
            spec2.IsSatisfiedBy(TestString).Returns(true);
            Assert.IsTrue(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(0, sut.ReasonsForDissatisfaction.Count());
        }

        [TestMethod]
        public void ReasonsForDissatisfactionIsClearedBeforeSatisfactionIsEvaluated()
        {
            sut = new AndSpecification<string>(spec1, spec2);
            spec1.IsSatisfiedBy(TestString).Returns(false, true);
            spec2.IsSatisfiedBy(TestString).Returns(true);
            Assert.IsFalse(sut.IsSatisfiedBy(TestString));
            Assert.IsTrue(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(0, sut.ReasonsForDissatisfaction.Count());
        }
    }
}