namespace Dive.Engine.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SFML.Graphics;

    /// <summary>
    /// Extensions for the SFML color class.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Parses the specified string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The color.</returns>
        /// <exception cref="Dive.Engine.ParseException">Invalid format for Color.</exception>
        public static Color Parse(string str)
        {
            string[] colors = str.Split(',');
            if (colors.Length != 3 && colors.Length != 4)
            {
                throw new ParseException("Invalid format for Color.");
            }

            if (colors.Length == 3)
            {
                return new Color(byte.Parse(colors[0]), byte.Parse(colors[1]), byte.Parse(colors[2]));
            }
            else
            {
                return new Color(byte.Parse(colors[0]), byte.Parse(colors[1]), byte.Parse(colors[2]), byte.Parse(colors[3]));
            }
        }
    }
}
