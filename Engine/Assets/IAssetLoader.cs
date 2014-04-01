namespace Dive.Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for all asset loaders managed by an <see cref="AssetManager"/>.
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// Loads the specified asset.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="key">The asset key.</param>
        /// <returns>The loaded asset.</returns>
        object Load(AssetManager manager, string key);
    }
}
