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
    /// Handles loading texture assets.
    /// </summary>
    [AssetLoader(AssetType = typeof(Texture))]
    public class TextureLoader : IAssetLoader
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
            Texture texture = new Texture(key);
            return texture;
        }
    }
}
