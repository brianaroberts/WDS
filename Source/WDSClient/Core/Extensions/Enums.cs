using System;

namespace WDS.Core.Extensions
{
    public static class Enums
    {
        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}
