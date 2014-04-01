namespace Dive.Script.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a command (via attributes).
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method)]
    public class CommandDef : System.Attribute
    {
        /// <summary>
        /// Gets or sets the command name.
        /// </summary>
        /// <value>
        /// The command name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        public string Usage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the command help.
        /// </summary>
        /// <value>
        /// The command help.
        /// </value>
        public string Help
        {
            get;
            set;
        }
    }
}
