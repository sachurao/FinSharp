using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Utilities.ObjectType
{
    public static class TypeUtils
    {
        public static bool IsDefault<T>(T instance)
        {
            return EqualityComparer<T>.Default.Equals(instance, default(T));
        }

        public static bool Implements(Type derivedType, Type expectedBaseType)
        {
            return expectedBaseType.IsAssignableFrom(derivedType);
        }

        public static object GetDefault(Type t)
        {
            if (t.IsValueType) return Activator.CreateInstance(t);
            else return null;
        }
    }
}
