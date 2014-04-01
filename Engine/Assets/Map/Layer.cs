namespace Dive.Assets.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract layer class.
    /// </summary>
    public abstract class Layer
    {
        /// <summary>
        /// The horizontal flip flag.
        /// </summary>
        public const uint HorizontalFlipFlag = 0x80000000;

        /// <summary>
        /// The vertical flip flag.
        /// </summary>
        public const uint VerticalFlipFlag = 0x40000000;

        /// <summary>
        /// The diagonal flip flag.
        /// </summary>
        public const uint DiagonalFlipFlag = 0x20000000;

        /// <summary>
        /// Gets or sets the name of the layer.
        /// </summary>
        /// <value>
        /// The name of the layer.
        /// </value>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        public float Opacity
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [visible]; otherwise, <c>false</c>.
        /// </value>
        public bool Visible
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public Dictionary<string, string> Properties
        {
            get;
            protected set;
        }
    }
}
