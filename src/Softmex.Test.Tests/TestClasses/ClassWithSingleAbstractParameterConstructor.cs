namespace Softmex.Test.Tests.TestClasses
{
    public class ClassWithSingleAbstractParameterConstructor
    {
        private readonly AbstractClass _abstractClass;

        public ClassWithSingleAbstractParameterConstructor(AbstractClass abstractClass)
        {
            _abstractClass = abstractClass;
        }

        public AbstractClass GetDependecy()
        {
            return _abstractClass;
        }
    }
}
