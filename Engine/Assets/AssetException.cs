namespace Dive.Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Exception for asset-related errors.
    /// </summary>
    [Serializable]
    public class AssetException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public AssetException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public AssetException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
