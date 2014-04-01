namespace Dive.Assets.Loaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Assets.Attributes;
    using SFML.Graphics;

    /// <summary>
    /// Handles loading image assets.
    /// </summary>
    [AssetLoader(AssetType = typeof(Image))]
    public class ImageLoader : IAssetLoader
    {
        /// <summary>
        /// Loads the specified asset.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="key">The asset key.</param>
        /// <returns>
        /// The loaded asset.
        /// </returns>
        public object Load(AssetManager manager, string key)
        {
            Image image = new Image(key);
            return image;
        }
    }
}
