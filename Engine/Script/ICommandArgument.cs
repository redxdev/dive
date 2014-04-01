namespace Dive.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Command argument interface. Passed to command functions by the ConsoleManager.
    /// </summary>
    public interface ICommandArgument
    {
        /// <summary>
        /// Gets the raw value. This is usually the value passed to the argument by
        /// the parser. Use Value instead of this when inside a command.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        string RawValue
        {
            get;
        }

        /// <summary>
        /// Gets the value. This should be used by commands as the RawValue may just be a
        /// variable name instead of the value of a variable.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        string Value
        {
            get;
        }

        /// <summary>
        /// Gets or sets the console.
        /// </summary>
        /// <value>
        /// The console.
        /// </value>
        ConsoleManager Console
        {
            get;
            set;
        }
    }
}
