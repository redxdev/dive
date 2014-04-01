namespace Dive.Assets.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// TMX object class.
    /// </summary>
    public class MapObject : Tile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapObject"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="horizontalFlip">If set to <c>true</c> [horizontal flip].</param>
        /// <param name="verticalFlip">If set to <c>true</c> [vertical flip].</param>
        /// <param name="diagonalFlip">If set to <c>true</c> [diagonal flip].</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="name">The name of the object.</param>
        /// <param name="type">The type of the object.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="visible">If set to <c>true</c> [visible].</param>
        public MapObject(int id, bool horizontalFlip, bool verticalFlip, bool diagonalFlip, int x, int y, string name, string type, float rotation, bool visible)
            : base(id, horizontalFlip, verticalFlip, diagonalFlip, x, y)
        {
            this.Name = name;
            this.Type = type;
            this.Rotation = rotation;
            this.Visible = visible;
            this.Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        /// <value>
        /// The name of the object.
        /// </value>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the type of the object.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        public string Type
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public float Rotation
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
