namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Exception for parsing related errors.
    /// </summary>
    [Serializable]
    public class ParseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ParseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
