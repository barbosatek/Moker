using System;
using System.Collections.Generic;
using System.Linq;

namespace Softmex.Test
{
  public class NsubstituteTestFor<T> : TestFor<T>, IDisposable where T : class
  {
    private readonly IDictionary<Type, object> _mocks;

    public NsubstituteTestFor()
    {
      _mocks = new Dictionary<Type, object>();
    }

    public new void Dispose()
    {
      _mocks.Clear();
      base.Dispose();
    }

    protected override IDictionary<Type, object> GetMockedDependencies()
    {
      return _mocks.ToDictionary(x => x.Key, x => x.Value);
    }

    protected override object MockDependency(Type type)
    {
      var mock = BuildMock(type);

      if (!_mocks.ContainsKey(type))
      {
        _mocks.Add(type, mock);
      }

      return mock;
    }

    public T2 The<T2>() where T2 : class
    {
      return (T2)The(typeof(T2));
    }

    public object The(Type type)
    {
      if (!_mocks.ContainsKey(type))
      {
        _mocks.Add(type, BuildMock(type));
      }

      return _mocks[type];
    }

    private object BuildMock(Type objectType)
    {
      return NSubstitute.Substitute.For(new List<Type> { objectType}.ToArray(), new List<object>().ToArray());
    }
  }
}