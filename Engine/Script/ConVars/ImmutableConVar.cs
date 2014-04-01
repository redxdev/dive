namespace Dive.Script.ConVars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Console variable that is backed by a string that cannot be changed.
    /// </summary>
    public class ImmutableConVar : IConVar
    {
        private string value = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableConVar"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ImmutableConVar(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        /// <exception cref="System.AccessViolationException">Variable is immutable.</exception>
        public virtual string Value
        {
            get
            {
                return this.value;
            }

            set
            {
                throw new AccessViolationException("Variable is immutable");
            }
        }
    }
}
