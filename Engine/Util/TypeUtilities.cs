namespace Dive.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Utility functions and extensions dealing with Type and reflections.
    /// </summary>
    public static class TypeUtilities
    {
        /// <summary>
        /// Gets a type object from any assembly.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>The type object, or null if there is none.</returns>
        public static Type GetGlobalType(string typeName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }
    }
}
