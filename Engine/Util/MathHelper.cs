namespace Dive.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SFML.Window;

    public static class MathHelper
    {
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }

            if (value.CompareTo(max) > 0)
            {
                return max;
            }

            return value;
        }

        public static T Min<T>(T value1, T value2) where T : IComparable<T>
        {
            if (value1.CompareTo(value2) > 0)
            {
                return value2;
            }

            return value1;
        }

        public static T Max<T>(T value1, T value2) where T : IComparable<T>
        {
            if (value1.CompareTo(value2) > 0)
            {
                return value2;
            }

            return value1;
        }

        public static float Distance(ref Vector2f a, ref Vector2f b)
        {
            return (float)(Math.Sqrt((double)DistanceSquared(ref a, ref b)));
        }

        public static float DistanceSquared(ref Vector2f a, ref Vector2f b)
        {
            return (float)(Math.Pow((double)(b.X - a.X), 2) + Math.Pow((double)(b.Y - a.Y), 2));
        }
    }
}
