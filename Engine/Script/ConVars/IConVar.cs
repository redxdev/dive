namespace Dive.Script.ConVars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Console variable interface.
    /// </summary>
    public interface IConVar
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        string Value
        {
            get;
            set;
        }
    }
}
