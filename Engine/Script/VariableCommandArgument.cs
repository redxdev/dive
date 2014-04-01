namespace Dive.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;

    /// <summary>
    /// Argument to a command that is backed by a variable.
    /// </summary>
    public class VariableCommandArgument : BasicCommandArgument
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(VariableCommandArgument));

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public override string Value
        {
            get
            {
                if (!this.Console.ContainsVariable(this.RawValue))
                {
                    Log.Warn(string.Format("Unknown variable \"{0}\", using null", this.RawValue));
                    return null;
                }

                return this.Console.GetVariable(this.RawValue).Value;
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
            return string.Format("VariableCommandArgument{{ RawValue = \"{0}\" }}", this.RawValue);
        }
    }
}
