using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquire.SpecFlow.Generation.Generator
{
    internal static class EnumerableExtensions
    {
        public static string Join(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }
    }
}
