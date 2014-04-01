namespace Dive.Engine.Components.Physics.Shapes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Entity;
    using Dive.Entity.Attributes;
    using FarseerPhysics;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using SFML.Graphics;
    using SFML.Window;

    /// <summary>
    /// Component to generate a bounding box around a sprite.
    /// </summary>
    [EntityComponent(Name = "Engine.Physics.Shapes.BoundingBox")]
    public class BoundingBoxComponent : AbstractComponent
    {
        private ComponentLookup<BodyComponent> body = null;

        private ComponentLookup<Graphics.SpriteComponent> sprite = null;

        /// <summary>
        /// Gets or sets the fixture.
        /// </summary>
        /// <value>
        /// The fixture.
        /// </value>
        public Fixture Fixture
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            this.body = new ComponentLookup<BodyComponent>(this.ParentEntity);
            this.sprite = new ComponentLookup<Graphics.SpriteComponent>(this.ParentEntity);
        }

        /// <summary>
        /// Finalizes this instance. Should be called after all components have been initialized.
        /// </summary>
        public override void FinalizeEntity()
        {
            IntRect rect = this.sprite.Component.Drawable.TextureRect;
            PolygonShape polygon = new PolygonShape(
                PolygonTools.CreateRectangle(
                    (float)ConvertUnits.ToSimUnits(rect.Width / 2),
                    (float)ConvertUnits.ToSimUnits(rect.Height / 2),
                    ConvertUnits.ToSimUnits(new Microsoft.Xna.Framework.Vector2(rect.Width / 2, rect.Height / 2)),
                    0f),
                0f);
            this.Fixture = this.body.Component.Body.CreateFixture(polygon, this);
        }

        /// <summary>
        /// Clears this instance. Called when the parent entity is removed from the world and
        /// is clearing itself.
        /// </summary>
        public override void Clear()
        {
            this.body.Component.Body.DestroyFixture(this.Fixture);
            this.Fixture = null;
        }

        /// <summary>
        /// Reads the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public override void BuildProperties(IDictionary<string, string> properties)
        {
            this.BuildProperty<float>(properties, "BoundingBox.Restitution", value => this.Fixture.Restitution = value);
        }
    }
}
