namespace Dive.Engine.Components.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Entity;
    using Dive.Entity.Attributes;
    using SFML.Graphics;

    /// <summary>
    /// Component for tile map layers. Note that this component does not support reading from
    /// properties, and will ignore everything when ReadProperties is called.
    /// </summary>
    [EntityComponent(Name = "Engine.Map.Layer", ExecutionLayer = EngineLayers.DrawGame)]
    public class LayerComponent : AbstractComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayerComponent"/> class.
        /// </summary>
        public LayerComponent()
        {
            this.Tiles = new List<Sprite>();
        }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>
        /// The tiles.
        /// </value>
        public List<Sprite> Tiles
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the draw layer.
        /// </summary>
        /// <value>
        /// The draw layer.
        /// </value>
        public int DrawLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            foreach (Sprite tile in this.Tiles)
            {
                this.ParentEntity.Engine.AddToRenderQueue(tile, this.DrawLayer);
            }
        }
    }
}
