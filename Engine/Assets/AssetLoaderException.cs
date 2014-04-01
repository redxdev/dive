namespace Dive.Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Exception class for asset loader related errors.
    /// </summary>
    [Serializable]
    public class AssetLoaderException : AssetException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetLoaderException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public AssetLoaderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetLoaderException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public AssetLoaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
