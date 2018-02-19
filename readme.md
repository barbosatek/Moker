# AutoMock framework
This framework allows a cleaner way to mock dependencies on top of mocking frameworks. This allows developers to focus on the code being tested rather than on the mocking setup and its maintenance.
## Properties and Methods
| Property / Method        | Description           |
| ------------- |-------------|
| `Target`      | A generic method that returns the mocked dependency. If the dependency hasn't been set, it creates it. |
| `Mock<T2> The<T2>()` | TDB      |
| `Mock The(Type type)` | TDB      |
| `T2 The<T2>()` | TDB      |
| `object The(Type type)` | TDB      |
| `T2 GetDependency<T2>()` | TDB      |
| `void SetDependency<T2>(T2 dependency)` | TDB      |
| `void SetDependency(Type type, object dependency)` | TDB      |

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