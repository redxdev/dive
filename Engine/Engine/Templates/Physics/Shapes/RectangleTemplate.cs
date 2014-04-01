namespace Dive.Engine.Templates.Physics.Shapes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine.Components;
    using Dive.Engine.Components.Physics;
    using Dive.Engine.Components.Physics.Shapes;
    using Dive.Entity;
    using Dive.Entity.Attributes;

    /// <summary>
    /// Template for rectangle physics entities.
    /// </summary>
    [EntityTemplate(Name = "Engine.Physics.Shapes.Rectangle")]
    public class RectangleTemplate : ITemplate
    {
        /// <summary>
        /// Builds the entity from the template.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The same entity passed in, but with components added per the template.
        /// </returns>
        public Entity BuildEntity(Entity entity, params object[] args)
        {
            entity.AddComponent<TransformComponent>();
            entity.AddComponent<BodyComponent>();
            entity.AddComponent<UpdateComponent>();
            entity.AddComponent<RectangleComponent>();

            return entity;
        }
    }
}
