using Moq;
using NUnit.Framework;

namespace Moker.Tests
{
    public interface IDependencyA
    {
        string Value { get; set; }
    }

    public interface IDependencyB
    {
        string Value { get; set; }
    }
    
    public class ClassWithNoDependencies
    {
    }

    public class ClassWithSingleDependency
    {
        private readonly IDependencyA _dependencyA;

        public ClassWithSingleDependency(IDependencyA dependencyA)
        {
            _dependencyA = dependencyA;
        }

        public IDependencyA GetDependecy()
        {
            return _dependencyA;
        }
    }

    [TestFixture]
    public class ClassWithSingleDependencyTests : TestFor<ClassWithSingleDependency>
    {
        [Test]
        public void Should_Initialize_Instance()
        {
            Assert.IsNotNull(Target);
            Assert.IsTrue(Target.GetType() == typeof(ClassWithSingleDependency));
        }
    }

    [TestFixture]
    public class ClassWithNoDependenciesTests : TestFor<ClassWithNoDependencies>
    {
        [Test]
        public void Should_Initialize_Instance()
        {
            Assert.IsNotNull(Target);
            Assert.IsTrue(Target.GetType() == typeof(ClassWithNoDependencies));
        }

        [Test]
        public void Should_Add_Any_Mocks()
        {
            var dependency = The<IDependencyA>();
            Assert.IsTrue(dependency.GetType() == typeof(Mock<IDependencyA>));
        }

        [Test]
        public void Should_Persist_Mocks_As_Singletons()
        {
            var expected = "A";
            var dependency = The<IDependencyA>();
            dependency.Setup(x => x.Value).Returns(expected);

            var anotherDependency = The<IDependencyA>();
            Assert.AreEqual("A", anotherDependency.Object.Value);
        }
    }
}
