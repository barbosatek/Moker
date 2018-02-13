using System;
using System.Linq;
using System.Reflection;

namespace Softmex.Test
{
  internal static class TypeUtility
  {
    public static ConstructorInfo GetConstructorWithLongestParamList(Type type)
    {
      ConstructorInfo[] publicConstructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

      if (!publicConstructors.Any())
      {
        throw new ArgumentException("Class has no public constructors");
      }

      return publicConstructors
        .OrderByDescending(x => x.GetParameters().Length)
        .First();
    }
  }
}