namespace Dive.Engine.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SFML.Window;

    /// <summary>
    /// Extensions for SFML vector classes.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Converts an SFML vector to an xna vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The converted vector.</returns>
        public static Vector2 ToXnaVector(this Vector2f vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        /// <summary>
        /// Converts an xna vector to an SFML vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The converted vector.</returns>
        public static Vector2f FromXnaVector(Vector2 vector)
        {
            return new Vector2f(vector.X, vector.Y);
        }

        /// <summary>
        /// Parses the specified string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        /// The Vector2f.
        /// </returns>
        /// <exception cref="Dive.Engine.ParseException">Invalid format for Vector2f.</exception>
        public static Vector2f Parse(string str)
        {
            string[] coords = str.Split(',');
            if (coords.Length != 2)
            {
                throw new ParseException("Invalid format for Vector2f.");
            }

            return new Vector2f(float.Parse(coords[0]), float.Parse(coords[1]));
        }
    }
}
