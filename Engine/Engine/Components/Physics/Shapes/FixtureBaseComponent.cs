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
    /// Base component for fixtures. Don't use this as a component by itself; use a derived class.
    /// </summary>
    public class FixtureBaseComponent : AbstractComponent
    {
        private ComponentLookup<BodyComponent> body = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixtureBaseComponent"/> class.
        /// </summary>
        public FixtureBaseComponent()
        {
            this.Fixtures = new List<Fixture>();
        }

        /// <summary>
        /// Gets or sets the density. This must be set before finalization.
        /// </summary>
        /// <value>
        /// The density.
        /// </value>
        public float Density
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the restitution. This must be set before finalization.
        /// </summary>
        /// <value>
        /// The restitution.
        /// </value>
        public float Restitution
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fixtures.
        /// </summary>
        /// <value>
        /// The fixtures.
        /// </value>
        public List<Fixture> Fixtures
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            this.body = new ComponentLookup<BodyComponent>(this.ParentEntity);
        }

        /// <summary>
        /// Finalizes this instance. Should be called after all components have been initialized.
        /// </summary>
        public override void FinalizeEntity()
        {
            foreach (Fixture fixture in this.Fixtures)
            {
                fixture.Restitution = this.Restitution;
            }
        }

        /// <summary>
        /// Clears this instance. Called when the parent entity is removed from the world and
        /// is clearing itself.
        /// </summary>
        public override void Clear()
        {
            foreach (Fixture fixture in this.Fixtures)
            {
                this.body.Component.Body.DestroyFixture(fixture);
            }
        }

        /// <summary>
        /// Reads the properties.
        /// <para>
        /// Valid properties:
        /// <list type="bullet">
        /// <item>
        /// <description>density</description>
        /// </item>
        /// <item>
        /// <description>restitution</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="properties">The properties.</param>
        public override void BuildProperties(IDictionary<string, string> properties)
        {
            this.BuildProperty<float>(properties, "Fixture.Density", value => this.Density = value);
            this.BuildProperty<float>(properties, "Fixture.Restitution", value => this.Restitution = value);
        }
    }
}
