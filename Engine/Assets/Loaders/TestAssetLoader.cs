namespace Dive.Assets.Loaders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Assets.Attributes;
    using log4net;

    /// <summary>
    /// A simple test asset loader. It doesn't actually load anything; it simply returns the asset key as the asset itself.
    /// </summary>
    //// There is no reason to actually use this, so the following line is commented out so that asset managers won't auto-load this class.
    ////[AssetLoader(AssetType = typeof(string))]
    public class TestAssetLoader : IAssetLoader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TestAssetLoader));

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
            Log.Debug("TestLoader.Load(\"" + key + "\")");
            return key;
        }
    }
}
