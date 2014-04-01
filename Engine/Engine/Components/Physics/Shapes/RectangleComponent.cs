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

    /// <summary>
    /// Component for generating a rectangle.
    /// </summary>
    [EntityComponent(Name = "Engine.Physics.Shapes.Rectangle")]
    public class RectangleComponent : FixtureBaseComponent
    {
        private ComponentLookup<BodyComponent> body = null;

        /// <summary>
        /// Gets or sets the width of the rectangle. This must be set before finalization.
        /// </summary>
        /// <value>
        /// The width of the rectangle.
        /// </value>
        public float Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the rectangle. This must be set before finalization.
        /// </summary>
        /// <value>
        /// The height of the rectangle.
        /// </value>
        public float Height
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.body = new ComponentLookup<BodyComponent>(this.ParentEntity);
        }

        /// <summary>
        /// Finalizes this instance. Should be called after all components have been initialized.
        /// </summary>
        public override void FinalizeEntity()
        {
            PolygonShape polygon = new PolygonShape(
                PolygonTools.CreateRectangle(
                    ConvertUnits.ToSimUnits(this.Width / 2),
                    ConvertUnits.ToSimUnits(this.Height / 2),
                    ConvertUnits.ToSimUnits(new Microsoft.Xna.Framework.Vector2(this.Width / 2, this.Height / 2)),
                    this.Density),
                this.Density);
            this.Fixtures.Add(this.body.Component.Body.CreateFixture(polygon, this));

            base.FinalizeEntity();
        }

        /// <summary>
        /// Reads the properties.
        /// <para>
        /// Valid properties:
        /// <list type="bullet">
        /// <item>
        /// <description>width</description>
        /// </item>
        /// <item>
        /// <description>height</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="properties">The properties.</param>
        public override void BuildProperties(IDictionary<string, string> properties)
        {
            base.BuildProperties(properties);

            this.BuildProperty<float>(properties, "Fixture.Rectangle.Width", value => this.Width = value);
            this.BuildProperty<float>(properties, "Fixture.Rectangle.Height", value => this.Height = value);
        }
    }
}
