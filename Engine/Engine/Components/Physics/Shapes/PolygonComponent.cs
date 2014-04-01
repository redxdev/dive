namespace Dive.Engine.Components.Physics.Shapes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine.Extensions;
    using Dive.Entity;
    using Dive.Entity.Attributes;
    using FarseerPhysics;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Common.Decomposition;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using SFML.Window;

    /// <summary>
    /// Component to generate a polygon.
    /// </summary>
    [EntityComponent(Name = "Engine.Physics.Shapes.Polygon")]
    public class PolygonComponent : FixtureBaseComponent
    {
        private ComponentLookup<BodyComponent> body = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonComponent"/> class.
        /// </summary>
        public PolygonComponent()
        {
            this.Points = new List<Vector2f>();
        }

        /// <summary>
        /// Gets or sets the points. This must be set before finalization.
        /// </summary>
        /// <value>
        /// The polygon's points.
        /// </value>
        public List<Vector2f> Points
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the triangulation algorithm.
        /// </summary>
        /// <value>
        /// The triangulation algorithm.
        /// </value>
        public TriangulationAlgorithm Algorithm
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
            this.Algorithm = TriangulationAlgorithm.Bayazit;
        }

        /// <summary>
        /// Finalizes this instance. Should be called after all components have been initialized.
        /// </summary>
        public override void FinalizeEntity()
        {
            Vertices vertices = new Vertices(this.Points.Count);
            foreach (Vector2f point in this.Points)
            {
                vertices.Add(ConvertUnits.ToSimUnits(point.ToXnaVector()));
            }

            List<Vertices> decomp = Triangulate.ConvexPartition(vertices, this.Algorithm);

            foreach (Vertices verts in decomp)
            {
                PolygonShape polygon = new PolygonShape(verts, this.Density);
                this.Fixtures.Add(this.body.Component.Body.CreateFixture(polygon, this));
            }

            this.Points.Clear();

            base.FinalizeEntity();
        }

        /// <summary>
        /// Reads the properties.
        /// <para>
        /// <list type="bullet">
        /// <item>
        /// <description>polygon: space delimited list of coordinates</description>
        /// </item>
        /// <item>
        /// <description>decomposition: the decomposition algorithm</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="properties">The properties.</param>
        public override void BuildProperties(IDictionary<string, string> properties)
        {
            base.BuildProperties(properties);

            if (properties.ContainsKey("polygon"))
            {
                string[] points = properties["polygon"].Split(' ');
                foreach (string pointDef in points)
                {
                    if (string.IsNullOrWhiteSpace(pointDef))
                    {
                        continue;
                    }

                    string[] values = pointDef.Split(',');
                    if (values.Length != 2)
                    {
                        throw new PropertyException("Property \"polygon\" is in an invalid format");
                    }

                    this.Points.Add(new Vector2f(float.Parse(values[0]), float.Parse(values[1])));
                }
            }

            if (properties.ContainsKey("decomposition"))
            {
                this.Algorithm = (TriangulationAlgorithm)Enum.Parse(typeof(TriangulationAlgorithm), properties["decomposition"]);
            }
        }
    }
}
