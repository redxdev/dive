namespace Dive.Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Exception class for asset loading related errors.
    /// </summary>
    [Serializable]
    public class AssetLoadException : AssetException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetLoadException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public AssetLoadException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetLoadException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public AssetLoadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
