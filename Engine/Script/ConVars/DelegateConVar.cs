namespace Dive.Script.ConVars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Console variable that uses delegates for reading/writing.
    /// </summary>
    public class DelegateConVar : IConVar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateConVar"/> class.
        /// </summary>
        public DelegateConVar()
        {
            this.GetFunc = null;
            this.SetFunc = null;
        }

        /// <summary>
        /// Delegate for getting a console variable.
        /// </summary>
        /// <returns>The value of the console variable.</returns>
        public delegate string GetDelegate();

        /// <summary>
        /// Delegate for setting a console variable.
        /// </summary>
        /// <param name="value">The value to set the variable to.</param>
        public delegate void SetDelegate(string value);

        /// <summary>
        /// Gets or sets the get function.
        /// </summary>
        /// <value>
        /// The get function.
        /// </value>
        public GetDelegate GetFunc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the set function.
        /// </summary>
        /// <value>
        /// The set function.
        /// </value>
        public SetDelegate SetFunc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        /// <exception cref="System.AccessViolationException">
        /// No delegate specified for read on ConVar
        /// or
        /// No delegate specified for write on ConVar.
        /// </exception>
        public string Value
        {
            get
            {
                if (this.GetFunc == null)
                {
                    throw new AccessViolationException("No delegate specified for read on ConVar");
                }

                return this.GetFunc();
            }

            set
            {
                if (this.SetFunc == null)
                {
                    throw new AccessViolationException("No delegate specified for write on ConVar");
                }

                this.SetFunc(value);
            }
        }
    }
}
