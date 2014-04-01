namespace Dive.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for entity templates.
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// Builds the entity from the template.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The same entity passed in, but with components added per the template.</returns>
        Entity BuildEntity(EntityManager manager, Entity entity, params object[] args);
    }
}
