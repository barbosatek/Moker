# AutoMock framework
This framework allows a cleaner way to mock dependencies on top of mocking frameworks. This allows developers to focus on the code being tested rather than on the mocking setup and its maintenance.
## Properties and Methods
| Property / Method        | Description           |
| ------------- |:-------------:|
| Target      | A generic method that returns the mocked dependency. If the dependency hasn't been set, it creates it. |
| The<T>      | TDB      |
| The() | TDB      |

## Moq Examples
```
    [Fact]
    public void UnitTestExample()
    {
      // Arrange
      The<IDependency>().Setup(x => x.GetValue()).Returns(Guid.NewGuid().ToString());

      // Act
      var isValid = Target.AreDependenciesValid();

      // Assert
      isValid.Should().BeTrue();
    }
```
## NSubstitute Examples
```
    [Fact]
    public void UnitTestExample()
    {
      // Arrange
      The<IDependency>().GetValue().Returns(Guid.NewGuid().ToString());

      // Act
      var isValid = Target.AreDependenciesValid();

      // Assert
      isValid.Should().BeTrue();
    }
```