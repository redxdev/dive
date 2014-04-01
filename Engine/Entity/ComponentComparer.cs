namespace Dive.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Entity.Attributes;

    /// <summary>
    /// Compares component execution layers.
    /// </summary>
    public class ComponentComparer : IComparer<Type>
    {
        /// <summary>
        /// Compares the specified types.
        /// </summary>
        /// <param name="a">The type a.</param>
        /// <param name="b">The type b.</param>
        /// <returns>A value representing the comparison.</returns>
        public int Compare(Type a, Type b)
        {
            EntityComponent compA = (EntityComponent)Attribute.GetCustomAttribute(a, typeof(EntityComponent));
            EntityComponent compB = (EntityComponent)Attribute.GetCustomAttribute(b, typeof(EntityComponent));

            int result = compA.ExecutionLayer.CompareTo(compB.ExecutionLayer);

            // If result is 0, then use the default comparer to make sure they are the same type. Otherwise,
            // return result.
            return result == 0 ? Comparer<string>.Default.Compare(a.FullName, b.FullName) : result;
        }
    }
}
