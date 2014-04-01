namespace Dive.Entity.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Attribute specifying a component's name and its execution layer.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EntityComponent : System.Attribute
    {
        /// <summary>
        /// Gets or sets the name of the component. Used by <see cref="EntityManager.CreateComponent"/>.
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the execution layer.
        /// </summary>
        /// <value>
        /// The execution layer.
        /// </value>
        public int ExecutionLayer
        {
            get;
            set;
        }
    }
}
