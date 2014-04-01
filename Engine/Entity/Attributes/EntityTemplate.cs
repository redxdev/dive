namespace Dive.Entity.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Attribute for specifying entity template options.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EntityTemplate : System.Attribute
    {
        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        /// <value>
        /// The name of the template.
        /// </value>
        public string Name
        {
            get;
            set;
        }
    }
}
