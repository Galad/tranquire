using System;

namespace Tranquire
{
    //[ExcludeFromCodeCoverage]
    internal static class Guard
    {
        public static void ForNull<T>(T value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ForNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
