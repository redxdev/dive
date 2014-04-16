﻿namespace Dive.Engine.Components.Graphics
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
    /// Component for sprite rendering.
    /// </summary>
    [EntityComponent(Name = "Sprite", ExecutionLayer = EngineLayers.DrawGame)]
    public class SpriteComponent : AbstractComponent
    {
        private ComponentLookup<TransformComponent> transform = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteComponent"/> class.
        /// </summary>
        public SpriteComponent()
        {
            this.Drawable = new Sprite();
        }

        /// <summary>
        /// Gets or sets the sprite.
        /// </summary>
        /// <value>
        /// The sprite.
        /// </value>
        public Sprite Drawable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the draw depth.
        /// </summary>
        /// <value>
        /// The draw depth.
        /// </value>
        public int Depth
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            this.transform = new ComponentLookup<TransformComponent>(this.ParentEntity);
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            this.Drawable.Position = this.transform.Component.Position;
            this.Drawable.Rotation = this.transform.Component.Rotation;

            GameEngine.Instance.RenderManager.AddToRenderQueue(this.Drawable, this.Depth);
        }

        /// <summary>
        /// Reads the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public override void BuildProperties(IDictionary<string, string> properties)
        {
            base.BuildProperties(properties);

            this.BuildProperty<int>(properties, "Sprite.Depth", value => { this.Depth = value; });
            this.BuildProperty<string>(
                properties,
                "Sprite.Texture",
                asset =>
                {
                    if (asset == null)
                    {
                        this.Drawable.Texture = null;
                        return;
                    }

                    this.Drawable.Texture = GameEngine.Instance.AssetManager.Load<Texture>(asset);
                });
        }
    }
}
