namespace Moker.Tests.TestClasses
{
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