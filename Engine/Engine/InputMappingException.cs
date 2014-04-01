namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Exception for input mapping related errors.
    /// </summary>
    [Serializable]
    public class InputMappingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputMappingException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public InputMappingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputMappingException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public InputMappingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
