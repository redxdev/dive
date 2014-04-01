namespace Dive.Engine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine.Extensions;
    using Dive.Entity;
    using Dive.Entity.Attributes;
    using SFML.Window;

    /// <summary>
    /// Component for holding tranforms (position and rotation).
    /// </summary>
    [EntityComponent(Name = "Engine.Transform")]
    public class TransformComponent : AbstractComponent
    {
        private Vector2f position = new Vector2f();

        /// <summary>
        /// Gets or sets the transform's position. Don't try setting the x and y coordinates
        /// through this, as Vector2f is a struct and it will give you compiler errors. Use the
        /// functions in this class instead.
        /// </summary>
        /// <value>
        /// The trasform's position.
        /// </value>
        public Vector2f Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }

        /// <summary>
        /// Gets or sets the transform's rotation.
        /// </summary>
        /// <value>
        /// The transform's rotation.
        /// </value>
        public float Rotation
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the x coordinate.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        public void SetX(float x)
        {
            this.position.X = x;
        }

        /// <summary>
        /// Sets the y coordinate.
        /// </summary>
        /// <param name="y">The y coordinate.</param>
        public void SetY(float y)
        {
            this.position.Y = y;
        }

        /// <summary>
        /// Sets the transform's position.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public void SetPosition(float x, float y)
        {
            this.SetX(x);
            this.SetY(y);
        }

        /// <summary>
        /// Adds to the x coordiate.
        /// </summary>
        /// <param name="aX">The addition to the x coordinate.</param>
        public void AddX(float aX)
        {
            this.SetX(this.Position.X + aX);
        }

        /// <summary>
        /// Adds to the y coordinate.
        /// </summary>
        /// <param name="aY">The addition to the y coordinate.</param>
        public void AddY(float aY)
        {
            this.SetY(this.Position.Y + aY);
        }

        /// <summary>
        /// Adds to the transform's position.
        /// </summary>
        /// <param name="aX">The addition to the x coordinate.</param>
        /// <param name="aY">The addition to the y coordinate.</param>
        public void AddPosition(float aX, float aY)
        {
            this.AddX(aX);
            this.AddY(aY);
        }

        /// <summary>
        /// Reads the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public override void BuildProperties(IDictionary<string, string> properties)
        {
            base.BuildProperties(properties);

            this.BuildProperty<float>(properties, "Tranform.Rotation", value => this.Rotation = value);
            this.BuildProperty<float>(properties, "Transform.X", value => this.SetX(value));
            this.BuildProperty<float>(properties, "Transform.Y", value => this.SetY(value));
        }
    }
}
