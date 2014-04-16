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
    [EntityComponent(Name = "TextRenderer", ExecutionLayer = EngineLayers.DrawGame)]
    public class TextRendererComponent : AbstractComponent
    {
        private ComponentLookup<TransformComponent> transform = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextRendererComponent"/> class.
        /// </summary>
        public TextRendererComponent()
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

            this.BuildProperty<int>(properties, "Text.Depth", value => this.Depth = value);
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

                    this.Drawable.Font = GameEngine.Instance.AssetManager.Load<Font>(asset);
                });
            this.BuildProperty<string>(properties, "Text.Color", value => this.Drawable.Color = ColorExtensions.Parse(value));
            this.BuildProperty<uint>(properties, "Text.CharacterSize", value => this.Drawable.CharacterSize = value);
        }
    }
}
