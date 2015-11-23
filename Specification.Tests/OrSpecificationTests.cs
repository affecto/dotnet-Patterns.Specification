using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.Patterns.Specification.Tests
{
    [TestClass]
    public class OrSpecificationTests
    {
        private OrSpecification<string> sut;
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
            sut = new OrSpecification<string>(null, spec2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Spec2IsMandatoryParameterInConstruction()
        {
            sut = new OrSpecification<string>(spec1, null);
        }

        [TestMethod]
        public void SpecificationIsNotSatisfiedWhenSpec1AndSpec2AreNotSatisfied()
        {
            sut = new OrSpecification<string>(spec1, spec2);
            spec1.IsSatisfiedBy(TestString).Returns(false);
            spec2.IsSatisfiedBy(TestString).Returns(false);
            Assert.IsFalse(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(2, sut.ReasonsForDissatisfaction.Count());
            Assert.AreEqual(1, sut.ReasonsForDissatisfaction.Count(r => r == Spec1Dissatisfaction));
            Assert.AreEqual(1, sut.ReasonsForDissatisfaction.Count(r => r == Spec2Dissatisfaction));
        }

        [TestMethod]
        public void ReasonsForDissatisfactionIsClearedBeforeSatisfactionIsEvaluated()
        {
            sut = new OrSpecification<string>(spec1, spec2);
            spec1.IsSatisfiedBy(TestString).Returns(false, true);
            spec2.IsSatisfiedBy(TestString).Returns(false, true);
            Assert.IsFalse(sut.IsSatisfiedBy(TestString));
            Assert.IsTrue(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(0, sut.ReasonsForDissatisfaction.Count());
        }

        [TestMethod]
        public void Spec2IsNotEvaluatedWhenSpec1IsSatisfied()
        {
            sut = new OrSpecification<string>(spec1, spec2);
            spec1.IsSatisfiedBy(TestString).Returns(true);
            Assert.IsTrue(sut.IsSatisfiedBy(TestString));
            spec2.DidNotReceive().IsSatisfiedBy(TestString);
        }

        [TestMethod]
        public void SpecificationIsSatisfiedWhenSpec1IsNotSatisfiedAndSpec2IsSatisfied()
        {
            sut = new OrSpecification<string>(spec1, spec2);
            spec1.IsSatisfiedBy(TestString).Returns(false);
            spec2.IsSatisfiedBy(TestString).Returns(true);
            Assert.IsTrue(sut.IsSatisfiedBy(TestString));
            Assert.AreEqual(0, sut.ReasonsForDissatisfaction.Count());
        }
    }
}