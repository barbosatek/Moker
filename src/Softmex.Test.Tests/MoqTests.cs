using FluentAssertions;
using Softmex.Test.Tests.TestArtifacts;
using Xunit;

namespace Softmex.Test.Tests
{
  public class MoqTests : MoqTestFor<ClassUnderTest>
  {
    [Fact]
    public void Automock_WhenTargetIsInvoked_ResolvesDependencies()
    {
      // Arrange + Act
      var dependencyValidation = Target.AreDependenciesValid();

      // Assert
      dependencyValidation.Should().BeTrue();
    }

    [Fact]
    public void Automock_WhenDependencyIsSet_DependencyIsUsed()
    {
      // Arrange
      var dependency = NSubstitute.Substitute.For<IDependency>();
      SetDependency(dependency);

      // Act
      var dependencyValidation = Target.AreDependenciesValid();

      // Assert
      dependencyValidation.Should().BeTrue();
      dependency.GetHashCode().Should().Be(Target.GetInterfaceDependency().GetHashCode());
    }

    [Fact]
    public void Automock_WhenNullDependencyIsSet_DependencyIsUsed()
    {
      // Arrange
      SetDependency((IDependency) null);

      // Act
      Target.AreDependenciesValid();

      // Assert
      Target.GetInterfaceDependency().Should().BeNull();
    }
  }
}