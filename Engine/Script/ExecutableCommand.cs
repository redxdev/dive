namespace Dive.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Script.Arguments;

    /// <summary>
    /// Used to execute commands and pass arguments.
    /// </summary>
    public class ExecutableCommand
    {
        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public List<ICommandArgument> Arguments
        {
            get;
            set;
        }
    }
}
