namespace Dive.Assets.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// TMX map tile class.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="horizontalFlip">If set to <c>true</c> [horizontal flip].</param>
        /// <param name="verticalFlip">If set to <c>true</c> [vertical flip].</param>
        /// <param name="diagonalFlip">If set to <c>true</c> [diagonal flip].</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Tile(int id, bool horizontalFlip, bool verticalFlip, bool diagonalFlip, int x, int y)
        {
            this.Id = id;
            this.HorizontalFlip = horizontalFlip;
            this.VerticalFlip = verticalFlip;
            this.DiagonalFlip = diagonalFlip;
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [horizontal flip].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [horizontal flip]; otherwise, <c>false</c>.
        /// </value>
        public bool HorizontalFlip
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [vertical flip].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [vertical flip]; otherwise, <c>false</c>.
        /// </value>
        public bool VerticalFlip
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [diagonal flip].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [diagonal flip]; otherwise, <c>false</c>.
        /// </value>
        public bool DiagonalFlip
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the x coordinate.
        /// </summary>
        /// <value>
        /// The x coordinate.
        /// </value>
        public float X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        /// <value>
        /// The y coordinate.
        /// </value>
        public float Y
        {
            get;
            set;
        }
    }
}
