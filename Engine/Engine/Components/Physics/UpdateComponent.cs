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

    /// <summary>
    /// Component to update physics body position.
    /// </summary>
    [EntityComponent(Name = "Engine.Physics.Update", ExecutionLayer = EngineLayers.UpdatePhysicsPositions)]
    public class UpdateComponent : AbstractComponent
    {
        private ComponentLookup<TransformComponent> transform = null;

        private ComponentLookup<BodyComponent> body = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            this.transform = new ComponentLookup<TransformComponent>(this.ParentEntity);
            this.body = new ComponentLookup<BodyComponent>(this.ParentEntity);
        }

        /// <summary>
        /// Finalizes this instance. Should be called after all components have been initialized.
        /// </summary>
        public override void FinalizeEntity()
        {
            this.Update();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            this.body.Component.Body.SetTransform(
                ConvertUnits.ToSimUnits(this.transform.Component.Position.ToXnaVector()),
                (float)(this.transform.Component.Rotation * (Math.PI / 180)));
        }
    }
}
