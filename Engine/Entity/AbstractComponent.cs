namespace Dive.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;

    /// <summary>
    /// Abstract class for entity components.
    /// </summary>
    public abstract class AbstractComponent : IComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractComponent));

        /// <summary>
        /// Gets or sets a value indicating whether [is attached].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is attached]; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
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
        public Entity ParentEntity
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes with the specified parent entity.
        /// </summary>
        /// <param name="ent">The parent entity.</param>
        public void Initialize(Entity ent)
        {
            this.ParentEntity = ent;
            this.Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Finalizes this instance. Should be called after all components have been initialized.
        /// </summary>
        public virtual void FinalizeEntity()
        {
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public virtual void Draw()
        {
        }

        /// <summary>
        /// Called when [input action].
        /// </summary>
        /// <param name="action">The action.</param>
        public virtual void OnInputAction(string action)
        {
        }

        /// <summary>
        /// Called when [event].
        /// </summary>
        /// <param name="name">The event name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// Event specific.
        /// </returns>
        public virtual bool OnEvent(string name, params object[] args)
        {
            return true;
        }

        /// <summary>
        /// Clears this instance. Called when the parent entity is removed from the world and
        /// is clearing itself.
        /// </summary>
        public virtual void Clear()
        {
        }

        /// <summary>
        /// Reads the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public virtual void BuildProperties(IDictionary<string, string> properties)
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string typeName = this.GetType().Name;
            foreach (object attr in this.GetType().GetCustomAttributes(typeof(Attributes.EntityComponent), false))
            {
                typeName = ((Attributes.EntityComponent)attr).Name;
                break;
            }

            return string.Format("{0}{1}", this.IsActive ? '+' : '-', typeName);
        }

        /// <summary>
        /// Utility method to call an action when a property is built on this entity.
        /// </summary>
        /// <typeparam name="T">The type of the action parameter.</typeparam>
        /// <param name="properties">The properties.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="action">The action.</param>
        protected void BuildProperty<T>(IDictionary<string, string> properties, string name, Action<T> action)
        {
            if (!properties.ContainsKey(name))
            {
                return;
            }

            string value = properties[name];
            try
            {
                action.Invoke((T)Convert.ChangeType(value, typeof(T)));
            }
            catch (Exception e)
            {
                Log.Warn(string.Format("Unable to build property \"{0}\" = \"{1}\"", name, value), e);
            }
        }
    }
}
