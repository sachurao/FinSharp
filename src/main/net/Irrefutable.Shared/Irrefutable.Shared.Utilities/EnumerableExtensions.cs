using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.Utilities
{
    public static class EnumerableExtensions
    {
        public static void Do<T>(this IEnumerable<T> items, Action<T> actionToPerform)
        {
            foreach (var item in items)
            {
                actionToPerform(item);
            }
        }
    }
}
