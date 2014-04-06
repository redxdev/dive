namespace Dive.Assets.Loaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Assets.Attributes;

    /// <summary>
    /// Handles loading font assets.
    /// </summary>
    [AssetLoader(AssetType = typeof(Assembly))]
    public class DLLLoader : IAssetLoader
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
            Assembly assembly = Assembly.LoadFrom(key);
            return assembly;
        }
    }
}
