namespace Dive.Entity
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine;
    using Dive.Entity.Attributes;
    using log4net;

    /// <summary>
    /// Manages entities and the entity world. Can be thought of as the entity world itself.
    /// </summary>
    public class EntityManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EntityManager));

        private long nextId = 0;

        private SortedDictionary<long, Entity> entities = null;

        private Dictionary<string, Entity> entityNames = null;

        private Dictionary<string, Type> componentRegistry = null;

        private Dictionary<string, ITemplate> templateRegistry = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityManager" /> class.
        /// </summary>
        public EntityManager()
        {
            this.entities = new SortedDictionary<long, Entity>();
            this.entityNames = new Dictionary<string, Entity>();
            this.componentRegistry = new Dictionary<string, Type>();
            this.templateRegistry = new Dictionary<string, ITemplate>();
        }

        /// <summary>
        /// Gets the active entity count.
        /// </summary>
        /// <value>
        /// The active entity count.
        /// </value>
        public int Count
        {
            get
            {
                return this.entities.Count;
            }
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public SortedDictionary<long, Entity> Entities
        {
            get
            {
                return this.entities;
            }
        }

        /// <summary>
        /// Gets the entity templates.
        /// </summary>
        /// <value>
        /// The entity templates.
        /// </value>
        public Dictionary<string, ITemplate> TemplateRegistry
        {
            get
            {
                return this.templateRegistry;
            }
        }

        /// <summary>
        /// Gets the component registry.
        /// </summary>
        /// <value>
        /// The component registry.
        /// </value>
        public Dictionary<string, Type> ComponentRegistry
        {
            get
            {
                return this.componentRegistry;
            }
        }

        /// <summary>
        /// Registers entity component and template types with the entity manager.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void ImportAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IComponent).IsAssignableFrom(type))
                {
                    try
                    {
                        EntityComponent compAttr = (EntityComponent)Attribute.GetCustomAttribute(type, typeof(EntityComponent));
                        this.componentRegistry.Add(compAttr.Name, type);
                    }
                    catch
                    {
                    }
                }
                
                if (typeof(ITemplate).IsAssignableFrom(type))
                {
                    try
                    {
                        EntityTemplate tempAttr = (EntityTemplate)Attribute.GetCustomAttribute(type, typeof(EntityTemplate));
                        this.templateRegistry.Add(tempAttr.Name, (ITemplate)Activator.CreateInstance(type));
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="name">The entity name. If this is null or made up of whitespace it will be ignored.</param>
        /// <returns>
        /// The new entity.
        /// </returns>
        /// <exception cref="System.ArgumentException">An entity with the specified name already exists.</exception>
        public Entity CreateEntity(string name = null)
        {
            if (!string.IsNullOrWhiteSpace(name) && this.entityNames.ContainsKey(name))
            {
                throw new ArgumentException("An entity with the specified name already exists");
            }

            Entity entity = new Entity(this.nextId++, name);
            this.entities.Add(entity.Id, entity);
            if (!string.IsNullOrWhiteSpace(name))
            {
                this.entityNames.Add(name, entity);
            }

            entity.IsActive = true;

            return entity;
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="builder">The entity builder.</param>
        /// <returns>
        /// The new entity.
        /// </returns>
        public Entity CreateEntity(Action<Entity> builder)
        {
            return this.CreateEntity(null, builder);
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="name">The entity name. If this is null or made up of whitespace it will be ignored.</param>
        /// <param name="builder">The entity builder.</param>
        /// <returns>
        /// The new entity.
        /// </returns>
        public Entity CreateEntity(string name, Action<Entity> builder)
        {
            Entity entity = this.CreateEntity(name);
            builder.Invoke(entity);
            return entity;
        }

        /// <summary>
        /// Creates the entity from template.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The entity created from the template.</returns>
        /// <exception cref="System.ArgumentException">Unknown template name \ + templateName + \.</exception>
        public Entity CreateEntityFromTemplate(string templateName, params object[] args)
        {
            ITemplate template = null;
            if (!this.templateRegistry.TryGetValue(templateName, out template))
            {
                throw new ArgumentException("Unknown template name \"" + templateName + "\"");
            }

            Entity entity = this.CreateEntity();
            return template.BuildEntity(entity, args);
        }

        /// <summary>
        /// Creates the entity from template.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="name">The entity's name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The entity created from the template.
        /// </returns>
        /// <exception cref="System.ArgumentException">Unknown template name \ + templateName + \.</exception>
        public Entity CreateEntityFromTemplateWithName(string templateName, string name, params object[] args)
        {
            ITemplate template = null;
            if (!this.templateRegistry.TryGetValue(templateName, out template))
            {
                throw new ArgumentException("Unknown template name \"" + templateName + "\"");
            }

            Entity entity = this.CreateEntity(name);
            return template.BuildEntity(entity, args);
        }

        /// <summary>
        /// Creates a component from its name.
        /// </summary>
        /// <param name="name">The name of the component, as specified by the <see cref="Attributes.EntityComponent"/> attribute.</param>
        /// <returns>The component.</returns>
        /// <exception cref="System.ArgumentException">Unknown component \ + name + \.</exception>
        /// <exception cref="System.InvalidCastException">Cannot cast from  + type.FullName +  to  + typeof(IComponent).FullName.</exception>
        public IComponent CreateComponent(string name)
        {
            Type type = null;
            if (!this.componentRegistry.TryGetValue(name, out type))
            {
                throw new ArgumentException("Unknown component \"" + name + "\"");
            }

            if (!typeof(IComponent).IsAssignableFrom(type))
            {
                throw new InvalidCastException("Cannot cast from " + type.FullName + " to " + typeof(IComponent).FullName);
            }

            return (IComponent)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Gets the type of the component.
        /// </summary>
        /// <param name="name">The component name.</param>
        /// <returns>The type of the component.</returns>
        /// <exception cref="System.ArgumentException">Unknown component \ + name + \.</exception>
        /// <exception cref="System.InvalidCastException">Cannot cast from  + type.FullName +  to  + typeof(IComponent).FullName.</exception>
        public Type GetComponentType(string name)
        {
            Type type = null;
            if (!this.componentRegistry.TryGetValue(name, out type))
            {
                throw new ArgumentException("Unknown component \"" + name + "\"");
            }

            if (!typeof(IComponent).IsAssignableFrom(type))
            {
                throw new InvalidCastException("Cannot cast from " + type.FullName + " to " + typeof(IComponent).FullName);
            }

            return type;
        }

        /// <summary>
        /// Gets the entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The entity or null if the entity doesn't exist.</returns>
        public Entity GetEntityById(long id)
        {
            Entity entity = null;
            if (!this.entities.TryGetValue(id, out entity))
            {
                return null;
            }

            return entity;
        }

        /// <summary>
        /// Gets an entity by name.
        /// </summary>
        /// <param name="name">The entity name.</param>
        /// <returns>The entity or null if the entity doesn't exist.</returns>
        public Entity GetEntityByName(string name)
        {
            Entity entity = null;
            if (!this.entityNames.TryGetValue(name, out entity))
            {
                return null;
            }

            return entity;
        }

        /// <summary>
        /// Removes the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void RemoveEntity(Entity entity)
        {
            entity.Clear();
            entity.IsActive = false;
            this.entities.Remove(entity.Id);

            if (!string.IsNullOrWhiteSpace(entity.Name))
            {
                this.entityNames.Remove(entity.Name);
            }
        }

        /// <summary>
        /// Updates the entity world.
        /// </summary>
        public void Update()
        {
            Entity[] entityList = this.entities.Values.ToArray(); // Allows modification of the entity list while updating.

            foreach (Entity entity in entityList)
            {
                if (!entity.IsActive)
                {
                    continue;
                }

                entity.Update();
            }
        }

        /// <summary>
        /// Draws the entity world.
        /// </summary>
        public void Draw()
        {
            Entity[] entityList = this.entities.Values.ToArray(); // Allows modification of the entity list while drawing.

            foreach (Entity entity in entityList)
            {
                if (!entity.IsActive)
                {
                    continue;
                }

                entity.Draw();
            }
        }

        /// <summary>
        /// Called when [input action].
        /// </summary>
        /// <param name="action">The input action.</param>
        public void OnInputAction(string action)
        {
            Entity[] entityList = this.entities.Values.ToArray();

            foreach (Entity entity in entityList)
            {
                if (!entity.IsActive)
                {
                    continue;
                }

                entity.OnInputAction(action);
            }
        }

        /// <summary>
        /// Clears the entity world. This does not reset the entity id counter. The only way to reset
        /// entity ids is to recreate the entity manager itself, which is an expensive operation.
        /// <para>
        /// Do not try to modify the entity list during a clear operation.
        /// </para>
        /// </summary>
        public void Clear()
        {
            Entity[] entityList = this.entities.Values.ToArray();
            foreach (Entity entity in entityList)
            {
                this.RemoveEntity(entity);
            }

            this.entities.Clear();

            this.nextId = 0;
        }

        /// <summary>
        /// Notifies the name change.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="newName">The new name.</param>
        /// <exception cref="System.ArgumentException">
        /// An entity with the specified name already exists.
        /// </exception>
        public void NotifyNameChange(Entity entity, string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName) && this.entityNames.ContainsKey(newName))
            {
                throw new ArgumentException("An entity with the specified name already exists");
            }

            if (this.entityNames.ContainsValue(entity))
            {
                this.entityNames.Remove(this.entityNames.First(i => i.Value == entity).Key);
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                return;
            }

            this.entityNames.Add(newName, entity);
        }
    }
}
