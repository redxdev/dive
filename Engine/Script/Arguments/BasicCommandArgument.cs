namespace Dive.Script.Arguments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Basic command argument backed by a string.
    /// </summary>
    public class BasicCommandArgument : ICommandArgument
    {
        /// <summary>
        /// Gets or sets the raw value. This is usually the value passed to the argument by
        /// the parser. Use Value instead of this when inside a command.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public virtual string RawValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value. This should be used by commands as the RawValue may just be a
        /// variable name instead of the value of a variable.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual string Value
        {
            get
            {
                return this.RawValue;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("BasicCommandArgument{{ RawValue = \"{0}\" }}", this.RawValue);
        }
    }
}
