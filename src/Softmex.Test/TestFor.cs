using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Softmex.Test
{
  public abstract class TestFor<T> : IDisposable where T : class
  {
    public T Target => _target ?? (_target = BuildTarget());
    
    private T _target;
    protected abstract IDictionary<Type, object> GetMockedDependencies();
    protected abstract object MockDependency(Type type);

    private readonly Dictionary<Type, object> _dependencies;

    protected TestFor()
    {
      _dependencies = new Dictionary<Type, object>();
    }

    public void SetDependency<T2>(T2 dependency) where T2 : class
    {
      var type = typeof(T2);
      SetDependency(type, dependency);
    }

    public void SetDependency(Type type, object dependency)
    {
      if (!_dependencies.ContainsKey(type))
      {
        _dependencies.Add(type, dependency);
      }
      else
      {
        _dependencies[type] = dependency;
      }
    }

    public T2 GetDependency<T2>() where T2 : class
    {
      var type = typeof(T2);
      if (!_dependencies.ContainsKey(type))
      {
        return null;
      }
      else
      {
        return (T2)_dependencies[type];
      }
    }

    private T BuildTarget()
    {
      ConstructorInfo constructor = TypeUtility.GetConstructorWithLongestParamList(typeof(T));
      List<Type> parameterTypes = constructor.GetParameters().Select(x => x.ParameterType).ToList();
      var mocks = GetMockedDependencies();

      if (parameterTypes.Count > 0)
      {
        var parameters = new List<object>();
        foreach (Type type in parameterTypes)
        {
          if (!_dependencies.ContainsKey(type))
          {
            object dependency = null;

            if (mocks.ContainsKey(type))
            {
              dependency = mocks[type];
            }
            else
            {
              dependency = MockDependency(type);
            }

            _dependencies.Add(type, dependency);
          }

          parameters.Add(_dependencies[type]);
        }

        return (T)constructor.Invoke(parameters.ToArray());
      }

      return (T)Activator.CreateInstance(typeof(T));
    }
    
    public void Dispose()
    {
      _target = null;
      _dependencies.Clear();
    }
  }
}
