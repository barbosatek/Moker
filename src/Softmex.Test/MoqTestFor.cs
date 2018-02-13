using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Softmex.Test
{
  public class MoqTestFor<T> : TestFor<T>, IDisposable where T : class
  {
    private readonly IDictionary<Type, Mock> _mocks;

    public MoqTestFor()
    {
      _mocks = new Dictionary<Type, Mock>();
    }

    public new void Dispose()
    {
      _mocks.Clear();
      base.Dispose();
    }

    public Mock<T2> The<T2>() where T2 : class
    {
      return (Mock<T2>)The(typeof(T2));
    }

    public Mock The(Type type)
    {
      if (!_mocks.ContainsKey(type))
      {
        _mocks.Add(type, BuildMock(type));
      }

      return _mocks[type];
    }

    protected override IDictionary<Type, object> GetMockedDependencies()
    {
      return _mocks.ToDictionary(x => x.Key, x => x.Value.Object);
    }

    protected override object MockDependency(Type type)
    {
      var mock = BuildMock(type);

      if (!_mocks.ContainsKey(type))
      {
        _mocks.Add(type, mock);
      }

      return mock.Object;
    }

    private Mock BuildMock(Type objectType)
    {
      var mockType = typeof(Mock<>).MakeGenericType(objectType);
      return (Mock)Activator.CreateInstance(mockType);
    }
  }
}