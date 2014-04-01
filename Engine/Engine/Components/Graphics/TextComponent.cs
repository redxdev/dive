namespace Dive.Engine.Components.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine.Extensions;
    using Dive.Entity;
    using Dive.Entity.Attributes;
    using SFML.Graphics;

    /// <summary>
    /// Component for text rendering.
    /// </summary>
    [EntityComponent(Name = "Engine.Graphics.Text", ExecutionLayer = EngineLayers.DrawGame)]
    public class TextComponent : AbstractComponent
    {
        private ComponentLookup<TransformComponent> transform = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextComponent"/> class.
        /// </summary>
        public TextComponent()
        {
            this.Drawable = new Text();
        }

        /// <summary>
        /// Gets or sets the drawable text.
        /// </summary>
        /// <value>
        /// The drawable text.
        /// </value>
        public Text Drawable
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

            this.ParentEntity.Engine.AddToRenderQueue(this.Drawable, this.DrawLayer);
        }

        /// <summary>
        /// Reads the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public override void BuildProperties(IDictionary<string, string> properties)
        {
            base.BuildProperties(properties);

            this.BuildProperty<int>(properties, "Text.DrawLayer", value => this.DrawLayer = value);
            this.BuildProperty<string>(properties, "Text.String", value => this.Drawable.DisplayedString = value);
            this.BuildProperty<string>(
                properties,
                "Text.Font",
                asset =>
                {
                    if (asset == null)
                    {
                        this.Drawable.Font = null;
                        return;
                    }

                    this.Drawable.Font = this.ParentEntity.Engine.AssetManager.Load<Font>(asset);
                });
            this.BuildProperty<string>(properties, "Text.Color", value => this.Drawable.Color = ColorExtensions.Parse(value));
            this.BuildProperty<uint>(properties, "Text.CharacterSize", value => this.Drawable.CharacterSize = value);
        }
    }
}
