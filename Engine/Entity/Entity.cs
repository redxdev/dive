namespace Dive.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine;

    /// <summary>
    /// Entity class, which is a container for <see cref="IComponent"/>s.
    /// </summary>
    public class Entity
    {
        private SortedDictionary<Type, IComponent> components = null;

        private string name = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity" /> class. This should not be called except by the <see cref="EntityManager" />.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The entity name or null.</param>
        public Entity(long id, string name)
        {
            this.Id = id;
            this.name = name;
            this.components = new SortedDictionary<Type, IComponent>(new ComponentComparer());
        }

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        public long Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is active].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is active]; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entity's name.
        /// </summary>
        /// <value>
        /// The entity's name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                GameEngine.Instance.EntityManager.NotifyNameChange(this, value);
                this.name = value;
            }
        }

        /// <summary>
        /// Gets the components.
        /// </summary>
        /// <value>
        /// The components.
        /// </value>
        public IDictionary<Type, IComponent> Components
        {
            get
            {
                return this.components;
            }
        }

        /// <summary>
        /// Finalizes this instance. Calls finalize on all components.
        /// This should always be called when you are done adding components, and should only ever be called once.
        /// </summary>
        public void FinalizeEntity()
        {
            foreach (IComponent component in this.components.Values)
            {
                component.FinalizeEntity();
            }
        }

        /// <summary>
        /// Adds the component to this instance.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="component">The component.</param>
        /// <returns>The component that was added.</returns>
        public T AddComponent<T>(T component) where T : IComponent
        {
            return (T)this.AddComponent(typeof(T), component);
        }

        /// <summary>
        /// Adds the component to this instance.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <returns>The component that was added.</returns>
        public T AddComponent<T>() where T : IComponent
        {
            return (T)this.AddComponent<T>(Activator.CreateInstance<T>());
        }

        /// <summary>
        /// Adds the component to this instance.
        /// </summary>
        /// <param name="type">The component type.</param>
        /// <param name="component">The component.</param>
        /// <returns>The component that was added.</returns>
        public IComponent AddComponent(Type type, IComponent component)
        {
            this.components.Add(type, component);
            component.IsActive = true;
            component.Initialize(this);

            return component;
        }

        /// <summary>
        /// Removes a component.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <returns>The removed component.</returns>
        public T RemoveComponent<T>() where T : IComponent
        {
            return (T)this.RemoveComponent(typeof(T));
        }

        /// <summary>
        /// Removes a component.
        /// </summary>
        /// <param name="type">The component type.</param>
        /// <returns>The removed component.</returns>
        /// <exception cref="System.ArgumentException">Unknown component type  + type.FullName.</exception>
        public IComponent RemoveComponent(Type type)
        {
            IComponent component = this.GetComponent(type);
            if (component == null)
            {
                throw new ArgumentException("Unknown component type " + type.FullName);
            }

            this.components.Remove(type);
            component.IsActive = false;
            component.Clear();

            return component;
        }

        /// <summary>
        /// Gets the component from this instance.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <returns>The component or null if this instance does not have the specified component type.</returns>
        public T GetComponent<T>() where T : IComponent
        {
            return (T)this.GetComponent(typeof(T));
        }

        /// <summary>
        /// Gets the component from this instance.
        /// </summary>
        /// <param name="type">The component type.</param>
        /// <returns>The component or null if this instance does not have the specified component type.</returns>
        public IComponent GetComponent(Type type)
        {
            IComponent component = null;
            if (!this.components.TryGetValue(type, out component))
            {
                return null;
            }

            return component;
        }

        /// <summary>
        /// Determines whether this instance has the specified component.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <returns>True if this instance has the specified component.</returns>
        public bool HasComponent<T>()
        {
            return this.HasComponent(typeof(T));
        }

        /// <summary>
        /// Determines whether this instance has the specified component.
        /// </summary>
        /// <param name="type">The component type.</param>
        /// <returns>True if this instance has the specified component.</returns>
        public bool HasComponent(Type type)
        {
            return this.components.ContainsKey(type);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            IComponent[] componentList = this.components.Values.ToArray(); // Allows modification of the component list while updating.
            foreach (IComponent component in componentList)
            {
                if (component.IsActive)
                {
                    component.Update();
                }
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            IComponent[] componentList = this.components.Values.ToArray(); // Allows modification of the component list while drawing.
            foreach (IComponent component in componentList)
            {
                if (component.IsActive)
                {
                    component.Draw();
                }
            }
        }

        /// <summary>
        /// Called when [input action].
        /// </summary>
        /// <param name="action">The action.</param>
        public void OnInputAction(string action)
        {
            IComponent[] componentList = this.components.Values.ToArray();
            foreach (IComponent component in componentList)
            {
                if (component.IsActive)
                {
                    component.OnInputAction(action);
                }
            }
        }

        /// <summary>
        /// Called when [event].
        /// </summary>
        /// <param name="name">The event name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>True if all components returned true, false if not.</returns>
        public bool OnEvent(string name, params object[] args)
        {
            bool result = true;

            IComponent[] componentList = this.components.Values.ToArray();
            foreach (IComponent component in componentList)
            {
                if (component.IsActive && !component.OnEvent(name, args))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Clears this instance and removes all components. Called when removed from the
        /// entity world.
        /// </summary>
        public void Clear()
        {
            IComponent[] componentList = this.components.Values.ToArray();
            foreach (IComponent component in componentList)
            {
                if (component.IsActive)
                {
                    this.RemoveComponent(component.GetType());
                }
            }

            this.components.Clear();
        }

        /// <summary>
        /// Builds the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public void BuildProperties(IDictionary<string, string> properties)
        {
            foreach (IComponent component in this.Components.Values)
            {
                component.BuildProperties(properties);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}{1}<{2}>", this.IsActive ? '+' : '-', this.Name, this.Id);
        }
    }
}
