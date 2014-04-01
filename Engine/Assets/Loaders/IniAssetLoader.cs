namespace Dive.Assets.Loaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Assets.Attributes;
    using IniParser;

    /// <summary>
    /// Loads ini configuration assets.
    /// </summary>
    [AssetLoader(AssetType = typeof(IniData))]
    public class IniAssetLoader : IAssetLoader
    {
        private FileIniDataParser parser = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="IniAssetLoader" /> class.
        /// </summary>
        public IniAssetLoader()
        {
            this.parser = new FileIniDataParser();
        }

        /// <summary>
        /// Gets the ini parser.
        /// </summary>
        /// <value>The ini parser.</value>
        public FileIniDataParser Parser
        {
            get
            {
                return this.parser;
            }
        }

        /// <summary>
        /// Save an ini file.
        /// </summary>
        /// <param name="key">The file path.</param>
        /// <param name="data">The ini data.</param>
        public void Save(string key, IniData data)
        {
            this.Parser.SaveFile(key, data);
        }

        /// <summary>
        /// Loads the specified asset.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="key">The asset key.</param>
        /// <returns>
        /// The loaded asset.
        /// </returns>
        /// <exception cref="AssetLoadException">Unable to load ini file (LoadFile returned null).</exception>
        public object Load(AssetManager manager, string key)
        {
            IniData data = this.Parser.LoadFile(key);
            if (data == null)
            {
                throw new AssetLoadException("Unable to load ini file (LoadFile returned null)");
            }

            return data;
        }
    }
}
