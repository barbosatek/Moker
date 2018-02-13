namespace Softmex.Test.Tests.TestArtifacts
{
  public class ClassUnderTest
  {
    private readonly IDependency _dependency;
    private readonly AbstractDependency _abstractDependency;

    public ClassUnderTest(IDependency dependency, AbstractDependency abstractDependency)
    {
      _dependency = dependency;
      _abstractDependency = abstractDependency;
    }

    public IDependency GetInterfaceDependency()
    {
      return _dependency;
    }

    public bool AreDependenciesValid()
    {
      return _dependency != null && _abstractDependency != null;
    }
  }
}