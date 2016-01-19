using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;

namespace Softmex.Test
{
    public class TestFor<T> where T : class
    {
        public T Target
        {
            get
            {
                // TODO: Make thread safe
                if (_target == null)
                {
                    CreateInstances();
                }

                return _target;
            }

            private set { _target = value; }
        }

        private T _target { get; set; }
        private readonly IDictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();

        public void CreateInstances()
        {
            ConstructorInfo constructor = ClassContructorUtility.GetConstructorWithLongestParamList(typeof (T));
            List<Type> parameterTypes = constructor.GetParameters().Select(x => x.ParameterType).ToList();

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
    }
}
