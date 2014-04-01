namespace Dive.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Exception for component property errors.
    /// </summary>
    [Serializable]
    public class PropertyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public PropertyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public PropertyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
