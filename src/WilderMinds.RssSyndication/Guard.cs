using System;

namespace WilderMinds.RssSyndication
{
    internal static class Guard
    {
        public static void CheckNullOrWhitespace(string name, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"The provided {name} was entirely whitespace or empty", name);
            }
        }

        public static void CheckNull(string name, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}