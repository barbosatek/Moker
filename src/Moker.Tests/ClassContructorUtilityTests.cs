﻿using System;
using System.Reflection;
using Moker.Tests.Artifacts;
using NUnit.Framework;

namespace Moker.Tests
{
    [TestFixture]
    public class ClassContructorUtilityTests
    {
        [Test]
        public void Class_With_Only_Private_Constructor_Should_Throw()
        {
            Exception exception = null;
            try
            {
                ClassContructorUtility.GetConstructorWithLongestParamList(typeof (PrivateConstructorClass));
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.That(exception is ArgumentException);
            Assert.AreEqual("Class has no public constructors", exception.Message);
        }

        [Test]
        public void Class_With_Only_Default_Constructor_Should_Return()
        {
            ConstructorInfo constructor = ClassContructorUtility.GetConstructorWithLongestParamList(typeof(NoConstructorClass));

            Assert.IsNotNull(constructor);
            CollectionAssert.IsEmpty(constructor.GetParameters());
        }

        [Test]
        public void Class_With_Empty_Constructor_Should_Return()
        {
            ConstructorInfo constructor = ClassContructorUtility.GetConstructorWithLongestParamList(typeof(PublicConstructorClass));

            Assert.IsNotNull(constructor);
            CollectionAssert.IsEmpty(constructor.GetParameters());
        }

        [Test]
        public void Class_With_Multiple_Constructors_Should_Return_Constructor_With_Longest_Parameters()
        {
            ConstructorInfo constructor = ClassContructorUtility.GetConstructorWithLongestParamList(typeof(MultipleConstructorClass));

            Assert.IsNotNull(constructor);
            Assert.That(constructor.GetParameters().Length > 0);
        }
    }

    internal class PrivateConstructorClass
    {
        private PrivateConstructorClass()
        {
        }
    }

    internal class NoConstructorClass
    {
    }

    internal class PublicConstructorClass
    {
        public PublicConstructorClass()
        {
        }
    }

    internal class MultipleConstructorClass
    {
        private readonly IDependencyA _dependencyA;

        public MultipleConstructorClass()
        {
        }

        public MultipleConstructorClass(IDependencyA dependencyA)
        {
            _dependencyA = dependencyA;
        }
    }
}