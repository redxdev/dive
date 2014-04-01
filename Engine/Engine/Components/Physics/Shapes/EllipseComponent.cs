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
    using FarseerPhysics.Common.Decomposition;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;

    /// <summary>
    /// Component for generating an ellipse.
    /// </summary>
    [EntityComponent(Name = "Engine.Physics.Shapes.Ellipse")]
    public class EllipseComponent : FixtureBaseComponent
    {
        private ComponentLookup<BodyComponent> body = null;

        /// <summary>
        /// Gets or sets the width of the ellipse (diameter). This must be set before finalization.
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
        /// Gets or sets the height of the ellipse (diameter). This must be set before finalization.
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
        /// Gets or sets the edge count. This must be set before finalization.
        /// </summary>
        /// <value>
        /// The edge count.
        /// </value>
        public int EdgeCount
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
            Vertices vertices = 
                PolygonTools.CreateEllipse(
                    ConvertUnits.ToSimUnits(this.Width / 2),
                    ConvertUnits.ToSimUnits(this.Height / 2),
                    this.EdgeCount);

            List<Vertices> decomp = Triangulate.ConvexPartition(vertices, TriangulationAlgorithm.Bayazit);

            foreach (Vertices verts in decomp)
            {
                PolygonShape polygon = new PolygonShape(verts, this.Density);
                this.Fixtures.Add(this.body.Component.Body.CreateFixture(polygon, this));
            }

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
        /// <item>
        /// <description>edge-count</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="properties">The properties.</param>
        public override void BuildProperties(IDictionary<string, string> properties)
        {
            base.BuildProperties(properties);

            this.BuildProperty<float>(properties, "Fixture.Ellipse.Width", value => this.Width = value);
            this.BuildProperty<float>(properties, "Fixture.Ellipse.Height", value => this.Height = value);
            this.BuildProperty<int>(properties, "Fixture.Ellipse.Edges", value => this.EdgeCount = value);
        }
    }
}
