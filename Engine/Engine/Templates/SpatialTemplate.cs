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

    [EntityTemplate(Name = "Engine.Spatial")]
    public class SpatialTemplate : ITemplate
    {
        public Entity BuildEntity(Entity entity, params object[] args)
        {
            entity.AddComponent<TransformComponent>();

            return entity;
        }
    }
}
