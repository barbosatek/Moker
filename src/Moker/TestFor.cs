using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;

namespace Moker
{
    public class TestFor<T> where T : class
    {
        public T Target { get; private set; }
        private readonly IDictionary<Type, IMockInstance> _mocks = new Dictionary<Type, IMockInstance>();

        public TestFor()
        {
            if (TargetRequiresDependencies())
            {
            }
            else
            {
                Target = (T)Activator.CreateInstance(typeof(T));
            }
        }
        
        public Mock<T2> The<T2>() where T2 : class
        {
            if (!_mocks.ContainsKey(typeof (T2)))
            {
                var instance = new MockInstance {Mock = new Mock<T2>()};
                _mocks.Add(typeof (T2), instance);
            }

            return (Mock<T2>) _mocks[typeof (T2)].Mock;
        }

        private bool TargetRequiresDependencies()
        {
            var publicConstructors = typeof (T).GetConstructors(BindingFlags.Public);
            return publicConstructors.Any(x => x.GetParameters().Length == 0);
        }
    }

    internal interface IMockInstance
    {
        object Mock { get; set; }
    }

    internal class MockInstance : IMockInstance
    {
        public object Mock { get; set; }
    }

}
