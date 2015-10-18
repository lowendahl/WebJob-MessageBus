using System;
using System.Collections.Generic;

namespace WebJob.MessageBus
{
    public static class EnumerableExtensions
    {
        public static void ForEachDo<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list) action(item);
        }
    }
}