using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Extensions
{
    static class CollectionExtension
    {
        public static int? IndexOf<T>(this IReadOnlyList<T> collection, T element)
        {
            foreach (var item in collection.Select((v, i) => new { Value = v, Index = i }))
            {
                var comparator = EqualityComparer<T>.Default;
                if (comparator.Equals(item.Value, element))
                {
                    return item.Index;
                }
            }
            return null;
        }
    }
}
