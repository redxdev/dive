namespace Dive.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SFML.Window;

    /// <summary>
    /// Contains useful math functions.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Clamps the specified value between min and max.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>The value, clamped to min and max.</returns>
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

        /// <summary>
        /// Get the smaller of two values. Returns the first if both are equal.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The minimum of the values.</returns>
        public static T Min<T>(T value1, T value2) where T : IComparable<T>
        {
            if (value1.CompareTo(value2) > 0)
            {
                return value2;
            }

            return value1;
        }

        /// <summary>
        /// Get the larger of two values. Returns the first if both are equal.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The maximum of the values.</returns>
        public static T Max<T>(T value1, T value2) where T : IComparable<T>
        {
            if (value1.CompareTo(value2) > 0)
            {
                return value2;
            }

            return value1;
        }

        /// <summary>
        /// Gets the distance between a and b.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The distance between a and b.</returns>
        public static float Distance(ref Vector2f a, ref Vector2f b)
        {
            return (float)Math.Sqrt((double)DistanceSquared(ref a, ref b));
        }

        /// <summary>
        /// Gets the squared distance between a and b.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The squared distance between a and b.</returns>
        public static float DistanceSquared(ref Vector2f a, ref Vector2f b)
        {
            return (float)(Math.Pow((double)(b.X - a.X), 2) + Math.Pow((double)(b.Y - a.Y), 2));
        }
    }
}
