namespace Dive.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Script;
    using Dive.Script.Attributes;

    /// <summary>
    /// Entity console commands.
    /// </summary>
    public static class EntityCommands
    {
        private static Dictionary<string, string> properties = new Dictionary<string, string>();

        /// <summary>
        /// Lists all entities.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "entity_list", Usage = "entity_list", Help = "List all entities")]
        public static void List(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_list (expected 0, got {0})", cmd.Arguments.Count));
            }

            foreach (Entity entity in console.GameEngine.EntityManager.Entities.Values)
            {
                ConsoleManager.ConsoleLog.Info(entity.ToString());
            }
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "entity_create", Usage = "entity_create [name]", Help = "Create an entity")]
        public static void Create(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count > 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_create (expected 0 or 1, got {0})", cmd.Arguments.Count));
            }

            string name = null;
            if (cmd.Arguments.Count == 1)
            {
                name = cmd.Arguments[0].Value;
            }

            Entity entity = console.GameEngine.EntityManager.CreateEntity(name);
            ConsoleManager.ConsoleLog.Info("Created entity " + entity.Id);
        }

        /// <summary>
        /// Creates an entity from a template.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "entity_create_template", Usage = "entity_create_template <template> [name]", Help = "Create an entity from a template")]
        public static void CreateTemplate(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1 && cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_create_template (expected 1 or 2, got {0})", cmd.Arguments.Count));
            }

            Entity entity = null;
            if (cmd.Arguments.Count == 1)
            {
                entity = console.GameEngine.EntityManager.CreateEntityFromTemplate(cmd.Arguments[0].Value);
            }
            else
            {
                entity = console.GameEngine.EntityManager.CreateEntityFromTemplateWithName(cmd.Arguments[0].Value, cmd.Arguments[1].Value);
            }

            ConsoleManager.ConsoleLog.Info("Created entity " + entity.Id);
        }

        /// <summary>
        /// Lists entity templates.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "entity_template_list", Usage = "entity_template_list", Help = "List available entity templates")]
        public static void ListTemplates(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_template_list (expected 0, got {0})", cmd.Arguments.Count));
            }

            foreach (string template in console.GameEngine.EntityManager.TemplateRegistry.Keys)
            {
                ConsoleManager.ConsoleLog.Info(template);
            }
        }

        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        /// <exception cref="System.NullReferenceException">Unknown entity id.</exception>
        [CommandDef(Name = "entity_remove", Usage = "entity_remove <id>", Help = "Remove an entity")]
        public static void Remove(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_remove (expected 1, got {0})", cmd.Arguments.Count));
            }

            long id = long.Parse(cmd.Arguments[0].Value);

            Entity entity = console.GameEngine.EntityManager.GetEntityById(id);
            if (entity == null)
            {
                throw new NullReferenceException(string.Format("Unknown entity id \"{0}\"", id));
            }

            console.GameEngine.EntityManager.RemoveEntity(entity);
        }

        /// <summary>
        /// Attaches a component to an entity.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        /// <exception cref="System.NullReferenceException">Unknown entity id.</exception>
        [CommandDef(Name = "entity_attach", Usage = "entity_attach <id> <name>", Help = "Attach a component to an entity")]
        public static void AttachComponent(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_attach (expected 2, got {0})", cmd.Arguments.Count));
            }

            long id = long.Parse(cmd.Arguments[0].Value);

            Entity entity = console.GameEngine.EntityManager.GetEntityById(id);
            if (entity == null)
            {
                throw new NullReferenceException(string.Format("Unknown entity id \"{0}\"", id));
            }

            IComponent component = console.GameEngine.EntityManager.CreateComponent(cmd.Arguments[1].Value);
            entity.AddComponent(component.GetType(), component);
        }

        /// <summary>
        /// Detaches a component from an entity.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        /// <exception cref="System.NullReferenceException">Unknown entity id.</exception>
        [CommandDef(Name = "entity_detach", Usage = "entity_detatch <id> <name>", Help = "Detach a component from an entity")]
        public static void DetachComponent(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_detatch (expected 2, got {0})", cmd.Arguments.Count));
            }

            long id = long.Parse(cmd.Arguments[0].Value);

            Entity entity = console.GameEngine.EntityManager.GetEntityById(id);
            if (entity == null)
            {
                throw new NullReferenceException(string.Format("Unknown entity id \"{0}\"", id));
            }

            entity.RemoveComponent(console.GameEngine.EntityManager.GetComponentType(cmd.Arguments[1].Value));
        }

        /// <summary>
        /// Lists all available components. This only includes ones explicitly registered with the entity manager (usually using the EntityComponent attribute).
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "entity_component_list", Usage = "entity_component_list", Help = "List all available components")]
        public static void ListComponents(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_component_list (expected 0, got {0})", cmd.Arguments.Count));
            }

            foreach (string component in console.GameEngine.EntityManager.ComponentRegistry.Keys.OrderBy(i => i))
            {
                ConsoleManager.ConsoleLog.Info(component);
            }
        }

        /// <summary>
        /// Finalizes an entity.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        /// <exception cref="System.NullReferenceException">Unknown entity id.</exception>
        [CommandDef(Name = "entity_finalize", Usage = "entity_finalize <id>", Help = "Finalize an entity")]
        public static void Finalize(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_finalize (expected 1, got {0})", cmd.Arguments.Count));
            }

            long id = long.Parse(cmd.Arguments[0].Value);

            Entity entity = console.GameEngine.EntityManager.GetEntityById(id);
            if (entity == null)
            {
                throw new NullReferenceException(string.Format("Unknown entity id \"{0}\"", id));
            }

            entity.FinalizeEntity();
        }

        /// <summary>
        /// Sets an entity's name.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "entity_set_name", Usage = "entity_set_name <id> <name>", Help = "Set an entity's name")]
        public static void SetName(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_set_name (expected 2, got {0})", cmd.Arguments.Count));
            }

            long id = long.Parse(cmd.Arguments[0].Value);
            string name = cmd.Arguments[1].Value;

            Entity entity = console.GameEngine.EntityManager.GetEntityById(id);
            if (entity == null)
            {
                throw new NullReferenceException(string.Format("Unknown entity id \"{0}\"", id));
            }

            entity.Name = name;
        }

        /// <summary>
        /// Enables an entity.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        /// <exception cref="System.NullReferenceException">Unknown entity id.</exception>
        [CommandDef(Name = "entity_enable", Usage = "entity_enable <id>", Help = "Set an entity's state to active")]
        public static void Enable(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_enable (expected 1, got {0})", cmd.Arguments.Count));
            }

            long id = long.Parse(cmd.Arguments[0].Value);

            Entity entity = console.GameEngine.EntityManager.GetEntityById(id);
            if (entity == null)
            {
                throw new NullReferenceException(string.Format("Unknown entity id \"{0}\"", id));
            }

            entity.IsActive = true;
        }

        /// <summary>
        /// Disables an entity.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        /// <exception cref="System.NullReferenceException">Unknown entity id.</exception>
        [CommandDef(Name = "entity_disable", Usage = "entity_disable <id>", Help = "Set an entity's state to inactive")]
        public static void Disable(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_disable (expected 1, got {0})", cmd.Arguments.Count));
            }

            long id = long.Parse(cmd.Arguments[0].Value);

            Entity entity = console.GameEngine.EntityManager.GetEntityById(id);
            if (entity == null)
            {
                throw new NullReferenceException(string.Format("Unknown entity id \"{0}\"", id));
            }

            entity.IsActive = false;
        }

        /// <summary>
        /// Prints entity information to the console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        /// <exception cref="System.NullReferenceException">Unknown entity id.</exception>
        [CommandDef(Name = "entity_info", Usage = "entity_info <id>", Help = "Get information pertaining to an entity such as components")]
        public static void Info(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_info (expected 1, got {0})", cmd.Arguments.Count));
            }

            long id = long.Parse(cmd.Arguments[0].Value);

            Entity entity = console.GameEngine.EntityManager.GetEntityById(id);
            if (entity == null)
            {
                throw new NullReferenceException(string.Format("Unknown entity id \"{0}\"", id));
            }

            ConsoleManager.ConsoleLog.Info(entity.ToString());
            ConsoleManager.ConsoleLog.Info("Components:");
            foreach (IComponent component in entity.Components.Values)
            {
                ConsoleManager.ConsoleLog.Info("\t" + component.ToString());
            }
        }

        /// <summary>
        /// Initializes the property list.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "entity_prop_init", Usage = "entity_prop_init", Help = "Initialize the property list")]
        public static void PropertyInit(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_prop_init (expected 0, got {0})", cmd.Arguments.Count));
            }

            properties.Clear();
        }

        /// <summary>
        /// Sets a property.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "entity_prop_set", Usage = "entity_prop_set <key> <value>", Help = "Set a property")]
        public static void PropertySet(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_prop_add (expected 2, got {0})", cmd.Arguments.Count));
            }

            properties[cmd.Arguments[0].Value] = cmd.Arguments[1].Value;
        }

        /// <summary>
        /// Build properties on an entity.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        /// <exception cref="System.NullReferenceException">Unknown entity id.</exception>
        [CommandDef(Name = "entity_prop_build", Usage = "entity_prop_build <id>", Help = "Build properties on an entity")]
        public static void PropertyBuild(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_prop_build (expected 1, got {0})", cmd.Arguments.Count));
            }

            long id = long.Parse(cmd.Arguments[0].Value);

            Entity entity = console.GameEngine.EntityManager.GetEntityById(id);
            if (entity == null)
            {
                throw new NullReferenceException(string.Format("Unknown entity id \"{0}\"", id));
            }

            entity.BuildProperties(properties);
        }

        /// <summary>
        /// List all properties.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "entity_prop_list", Usage = "entity_prop_list", Help = "List current properties")]
        public static void PropertyList(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for entity_prop_list (expected 0, got {0})", cmd.Arguments.Count));
            }

            foreach (string key in properties.Keys)
            {
                ConsoleManager.ConsoleLog.Info(string.Format("{0} = {1}", key, properties[key]));
            }
        }
    }
}
