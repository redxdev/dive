namespace Dive.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for entity components.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Gets or sets a value indicating whether [is attached].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is attached]; otherwise, <c>false</c>.
        /// </value>
        bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the parent entity.
        /// </summary>
        /// <value>
        /// The parent entity.
        /// </value>
        Entity ParentEntity
        {
            get;
        }

        /// <summary>
        /// Initializes this component with the specified entity as parent.
        /// </summary>
        /// <param name="ent">The parent entity.</param>
        void Initialize(Entity ent);

        /// <summary>
        /// Finalizes this instance. Should be called after all components have been initialized.
        /// </summary>
        void FinalizeEntity();

        /// <summary>
        /// Updates this instance.
        /// </summary>
        void Update();

        /// <summary>
        /// Draws this instance.
        /// </summary>
        void Draw();

        /// <summary>
        /// Called when [input action].
        /// </summary>
        /// <param name="action">The action.</param>
        void OnInputAction(string action);

        /// <summary>
        /// Called when [event].
        /// </summary>
        /// <param name="name">The event name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>Event specific.</returns>
        bool OnEvent(string name, params object[] args);

        /// <summary>
        /// Clears this instance. Called when the parent entity has been removed from the entity world
        /// and is clearing itself, or when the component is removed from the parent entity.
        /// </summary>
        void Clear();

        /// <summary>
        /// Reads the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        void BuildProperties(IDictionary<string, string> properties);
    }
}
