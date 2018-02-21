# AutoMock, unit test what matters!
This framework allows testing units in a cleaner way by removing noisy code and focus on the code that matters. It allows the developer to:
* Completely ignore dependencies not being used in the test unit, even if they're needed in the constructor.
* Ignore delcaring mocks, the framework keeps track of them via the `The<T>()` method.
* Ignore instantiating the target class, the framework provides an instance.
* Not worry about breaking tests when changing the parameter order in constructors.

The framework currently provides implementations for Moq and NSubstitute, but it is flexible enough to add any other mocking framework.

## Properties and Methods
`TestFor<T>` is the abstract base class, and it provides the following methods and properties.
| Property / Method        | Description           |
| ------------- |-------------|
| `Target`      | Builds and returns an instance of the class under test. If the instance was already built, it returns it and doesn't re-create it. |
| `T GetDependency<T>()` | Gets the dependency of `T`, if the dependency wasn't set, it returns null.|
| `void SetDependency<T>(T dependency)` | Sets the dependency of `T`, if the dependency had already been set, it overrides it.      |
| `void SetDependency(Type type, object dependency)` | Sets the dependency of provided type, if the dependency had already been set, it overrides it.      |

`MoqTestFor<T>` is the Moq implementation, and in addition to the base class functionality, it provides:
| Property / Method        | Description           |
| ------------- |-------------|
| `Mock<T> The<T>()` | Creates and sets a `Mock` instance. If the instance was already created, it returns it.|
| `Mock The(Type type)` | Creates and sets a `Mock` instance. If the instance was already created, it returns it.      |

`NSubstituteTestFor<T>` is the NSubstitute implementation, and in addition to the base class functionality, it provides:
| Property / Method        | Description           |
| ------------- |-------------|
| `T The<T>()` | TDB      |
| `object The(Type type)` | TDB      |

## Examples
Consider the following class, and consider testing the `IsDependencyValid()` unit.
```
  public class ClassUnderTest
  {
    private readonly IDependency _dependency;
    private readonly ISecondDependency _secondDependency;
    private readonly IThirdDependency _thirdDependency;
    private readonly AbstractDependency _abstractDependency;

    public ClassUnderTest(IDependency dependency, ISecondDependency secondDependency, IThirdDependency thirdDependency, AbstractDependency abstractDependency)
    {
      _dependency = dependency;
      _abstractDependency = abstractDependency;
      _secondDependency = secondDependency;
      _thirdDependency = thirdDependency;
    }
    
    public bool IsDependencyValid()
    {
      return _dependency != null;
    }
  }
```

The traditional way to test this class would be something like this
```
    [Fact]
    public void UnitTestExample()
    {
      // Arrange
      var dependencyMock = new Mock<IDependency>();
      var secondDependencyMock = new Mock<ISecondDependency>();
      var thirdDependencyMock = new Mock<IThirdDependency>();
      var abstractDependencyMock = new Mock<AbstractDependency>();

      dependencyMock.Setup(x => x.GetValue()).Returns(Guid.NewGuid().ToString());
	  
      var target = new ClassUnderTest(dependencyMock.Object, secondDependencyMock.Object, thirdDependencyMock.Object, abstractDependencyMock.Object);

      // Act
      var isValid = target.IsDependencyValid();

      // Assert
      isValid.Should().BeTrue();
    }
```

AutoMock will allow the developer to remove most of the code, the following example shows:
* No need to create mocks for `ISecondDependency, IThirdDependency, AbstractDependency`.
* No need to declare a local variable for the `IDependency` mock, the `The<T>()` method will track it.
* No need to instantiate the target class, the `Target` property is the instance.

```
  public class MoqTests : MoqTestFor<ClassUnderTest>
  {
    [Fact]
    public void UnitTestExample()
    {
      // Arrange
      The<IDependency>().Setup(x => x.GetValue()).Returns(Guid.NewGuid().ToString());

      // Act
      var isValid = Target.IsDependencyValid();

      // Assert
      isValid.Should().BeTrue();
    }
  }
```

In a similar fashion, NSubstitute is also supported by inheriting from `NsubstituteTestFor<T>`.
```
  public class NSubstituteTests : NsubstituteTestFor<ClassUnderTest>
  {
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
  }
```