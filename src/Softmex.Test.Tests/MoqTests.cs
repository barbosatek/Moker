using FluentAssertions;
using Moq;
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
      var dependency = new Mock<IDependency>().Object;
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

    [Fact]
    public void Automock_WhenAlreadySetDependencyIsSet_DependencyIsOverwritten()
    {
      // Arrange
      SetDependency(The<IDependency>().Object);
      SetDependency((IDependency)null);

      // Act
      Target.AreDependenciesValid();

      // Assert
      Target.GetInterfaceDependency().Should().BeNull();
    }

    [Fact]
    public void Automock_WhenDependencyAutomocked_DepdencyIsUsed()
    {
      // Arrange
      var dependency = The<IDependency>();

      // Act
      var dependencyValidation = Target.AreDependenciesValid();

      // Assert
      dependencyValidation.Should().BeTrue();
      dependency.GetHashCode().Should().Be(Target.GetInterfaceDependency().GetHashCode());
    }

    [Fact]
    public void Automock_WhenDependencyIsPull_ReturnsDepdency()
    {
      // Arrange
      var dependency = The<IDependency>().Object;
      SetDependency(dependency);

      // Act + Assert
      GetDependency<IDependency>().GetHashCode().Should().Be(dependency.GetHashCode());
    }

    [Fact]
    public void Automock_WhenDependencyHasNotBeenSetAndItIsPull_ReturnsNullDepdency()
    {
      // Act + Assert
      GetDependency<IDependency>().Should().BeNull();
    }
  }
}