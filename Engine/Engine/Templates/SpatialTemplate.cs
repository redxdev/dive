namespace Dive.Engine.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine.Components;
    using Dive.Entity;
    using Dive.Entity.Attributes;

    /// <summary>
    /// Template for spatial entity.
    /// </summary>
    [EntityTemplate(Name = "Engine.Spatial")]
    public class SpatialTemplate : ITemplate
    {
        /// <summary>
        /// Builds the entity from the template.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The same entity passed in, but with components added per the template.
        /// </returns>
        public Entity BuildEntity(Entity entity, params object[] args)
        {
            entity.AddComponent<TransformComponent>();

            return entity;
        }
    }
}
