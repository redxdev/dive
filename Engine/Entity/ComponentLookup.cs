namespace Dive.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Speeds up component retrieval time by caching component references. This is the preferred way to access
    /// components from inside other components.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    public class ComponentLookup<T> where T : class, IComponent
    {
        private T component = null;

        private Entity entity = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentLookup{T}"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public ComponentLookup(Entity entity)
        {
            this.entity = entity;
        }

        /// <summary>
        /// Gets the component.
        /// </summary>
        /// <value>
        /// The component.
        /// </value>
        public T Component
        {
            get
            {
                if (this.component == null || !this.component.IsActive || this.component.ParentEntity != this.entity)
                {
                    this.component = this.entity.GetComponent<T>();
                }

                return this.component;
            }
        }
    }
}
