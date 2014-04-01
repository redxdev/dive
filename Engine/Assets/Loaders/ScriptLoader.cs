namespace Dive.Assets.Loaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Assets.Attributes;
    using Dive.Script;

    /// <summary>
    /// Handles loading script assets.
    /// </summary>
    [AssetLoader(AssetType = typeof(CommandList))]
    public class ScriptLoader : IAssetLoader
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
            return ScriptUtilities.ParseFile(key);
        }
    }
}
