namespace Softmex.Test.Tests.TestClasses
{
    public class ClassWithSingleInterfaceConstructor
    {
        private readonly IDependencyA _dependencyA;

        public ClassWithSingleInterfaceConstructor(IDependencyA dependencyA)
        {
            _dependencyA = dependencyA;
        }

        public IDependencyA GetDependecy()
        {
            return _dependencyA;
        }
    }
}
