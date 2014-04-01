namespace Dive.Script.ConVars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Console variable backed by a string.
    /// </summary>
    public class BasicConVar : IConVar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicConVar"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public BasicConVar(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicConVar"/> class.
        /// </summary>
        public BasicConVar()
        {
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual string Value
        {
            get;
            set;
        }
    }
}
