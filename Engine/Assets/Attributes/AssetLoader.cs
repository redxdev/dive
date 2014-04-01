namespace Dive.Assets.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Any <see cref="Dive.Util.Assets.IAssetLoader"/> marked with this attribute will automatically be registered with
    /// the <see cref="Dive.Util.Assets.AssetManager"/>.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AssetLoader : System.Attribute
    {
        /// <summary>
        /// Gets or sets the type of the asset for this loader.
        /// </summary>
        /// <value>The type of the asset for this loader.</value>
        public Type AssetType
        {
            get;
            set;
        }
    }
}
