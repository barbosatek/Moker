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
        private readonly IDictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();

        public TestFor()
        {
            var parameterTypes = GetMaxParameterContructurList();

            if (parameterTypes.Count > 0)
            {
                var parameterMocks = new List<Mock>();
                foreach (Type type in parameterTypes)
                {
                    Mock mock;
                    if (!_mocks.ContainsKey(type))
                    {
                        mock = BuildMock(type);
                        _mocks.Add(type, mock);
                    }
                    else
                    {
                        mock = _mocks[type];
                    }

                    parameterMocks.Add(mock);
                }

                var constructor = GetConstructorWithLongestParamList();
                Target = (T) constructor.Invoke(parameterMocks.Select(x => x.Object).ToArray());
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
                _mocks.Add(typeof (T2), BuildMock(typeof(T2)));
            }

            return (Mock<T2>) _mocks[typeof (T2)];
        }

        private Mock BuildMock(Type objectType)
        {
            var mockType = typeof (Mock<>).MakeGenericType(objectType);
            return (Mock) Activator.CreateInstance(mockType);
        }
        
        // NOTE: Worth unit testing due many possible scenarios:
        // no constructos (private parameterless), one constructor, multiple constructors
        // parameter type contraints?
        private ICollection<Type> GetMaxParameterContructurList()
        {
            ConstructorInfo maxParameterConstructor = GetConstructorWithLongestParamList();

            if (maxParameterConstructor == null)
            {
                throw new Exception("Class has no public constructors");
            }

            return maxParameterConstructor.GetParameters().Select(x => x.ParameterType).ToList();
        }

        private ConstructorInfo GetConstructorWithLongestParamList()
        {
            var publicConstructors = typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            return publicConstructors
                .OrderBy(x => x.GetParameters().Length)
                .FirstOrDefault();
        }
    }
}
