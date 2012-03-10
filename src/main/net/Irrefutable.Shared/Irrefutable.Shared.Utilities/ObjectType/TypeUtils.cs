using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.Utilities.ObjectType
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
    }
}
