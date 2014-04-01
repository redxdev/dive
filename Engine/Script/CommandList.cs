namespace Dive.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Contains a list of commands.
    /// </summary>
    public class CommandList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandList"/> class.
        /// </summary>
        /// <param name="commands">The commands.</param>
        public CommandList(List<ExecutableCommand> commands)
        {
            this.Commands = commands;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandList"/> class.
        /// </summary>
        public CommandList()
        {
            this.Commands = new List<ExecutableCommand>();
        }

        /// <summary>
        /// Gets or sets the commands.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        public List<ExecutableCommand> Commands
        {
            get;
            set;
        }
    }
}
