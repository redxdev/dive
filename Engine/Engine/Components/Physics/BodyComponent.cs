namespace Dive.Engine.Components.Physics
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
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;

    /// <summary>
    /// Component for physics bodies.
    /// </summary>
    [EntityComponent(Name = "Engine.Physics.Body", ExecutionLayer = EngineLayers.UpdatePhysics)]
    public class BodyComponent : AbstractComponent
    {
        private ComponentLookup<TransformComponent> transform = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyComponent"/> class.
        /// </summary>
        public BodyComponent()
        {
        }

        /// <summary>
        /// Gets or sets the physics body.
        /// </summary>
        /// <value>
        /// The physics body.
        /// </value>
        public Body Body
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

            this.Body = BodyFactory.CreateBody(this.ParentEntity.Engine.PhysicsWorld, this);
            this.Body.BodyType = BodyType.Static;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            this.transform.Component.Position = VectorExtensions.FromXnaVector(ConvertUnits.ToDisplayUnits(this.Body.Position));
            this.transform.Component.Rotation = (float)(this.Body.Rotation * (180 / Math.PI));
        }

        /// <summary>
        /// Clears this instance. Called when the parent entity is removed from the world and
        /// is clearing itself.
        /// </summary>
        public override void Clear()
        {
            this.ParentEntity.Engine.PhysicsWorld.RemoveBody(this.Body);
        }

        /// <summary>
        /// Reads the properties.
        /// <para>
        /// Valid properties:
        /// <list type="bullet">
        /// <item>
        /// <description>body-type: static, kinematic, or dynamic</description>
        /// </item>
        /// <item>
        /// <description>angular-damping</description>
        /// </item>
        /// <item>
        /// <description>friction</description>
        /// </item>
        /// <item>
        /// <description>linear-damping</description>
        /// </item>
        /// <item>
        /// <description>mass</description>
        /// </item>
        /// <item>
        /// <description></description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="properties">The properties.</param>
        public override void BuildProperties(IDictionary<string, string> properties)
        {
            this.BuildProperty<string>(
                properties,
                "Body.Type",
                value =>
                {
                    switch (value.ToLower().Trim())
                    {
                        default:
                            throw new PropertyException(string.Format("Property Body.Type cannot be set to \"{0}\"", value));

                        case "static":
                            this.Body.BodyType = BodyType.Static;
                            break;

                        case "kinematic":
                            this.Body.BodyType = BodyType.Kinematic;
                            break;

                        case "dynamic":
                            this.Body.BodyType = BodyType.Dynamic;
                            break;
                    }
                });
            this.BuildProperty<float>(properties, "Body.AngularDamping", value => this.Body.AngularDamping = value);
            this.BuildProperty<float>(properties, "Body.Friction", value => this.Body.Friction = value);
            this.BuildProperty<float>(properties, "Body.LinearDamping", value => this.Body.LinearDamping = value);
            this.BuildProperty<float>(properties, "Body.Mass", value => this.Body.Mass = value);
        }
    }
}
