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
    /// Handles loading shader assets.
    /// </summary>
    [AssetLoader(AssetType = typeof(Shader))]
    public class ShaderLoader : IAssetLoader
    {
        /// <summary>
        /// Load a set of shaders from an asset key. Separate the vertex and fragment shaders with a semicolon, and
        /// use null if you don't want to load one of the shaders.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="key">The asset key.</param>
        /// <returns>The loaded shader.</returns>
        public object Load(AssetManager manager, string key)
        {
            if (!Shader.IsAvailable)
            {
                throw new AssetLoadException("Shaders are unavailable on this system");
            }

            string[] files = key.Split(new char[] { ';' }, 2);
            if (files.Length != 2)
            {
                throw new AssetLoadException("Expected 2 asset keys (use format \"key1;key2\")");
            }

            return new Shader(files[0].ToLower() == "null" ? null : files[0], files[1].ToLower() == "null" ? null : files[1]);
        }
    }
}
