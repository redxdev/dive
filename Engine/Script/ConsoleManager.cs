namespace Dive.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine;
    using Dive.Script.ConVars;
    using log4net;

    /// <summary>
    /// Delegate for console commands.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="command">The command.</param>
    public delegate void ConsoleCommand(ConsoleManager console, ExecutableCommand command);

    /// <summary>
    /// Manages the console and script execution.
    /// </summary>
    public class ConsoleManager
    {
        /// <summary>
        /// The console log.
        /// </summary>
        public static readonly ILog ConsoleLog = LogManager.GetLogger("CONSOLE");

        private static readonly ILog Log = LogManager.GetLogger(typeof(ConsoleManager));

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="autoLoad">If set to <c>true</c> [automatic load].</param>
        public ConsoleManager(Engine engine, bool autoLoad = true)
        {
            this.GameEngine = engine;
            this.Variables = new Dictionary<string, IConVar>();
            this.Commands = new Dictionary<string, CommandInfo>();
            this.SetupVariables();

            if (autoLoad)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        foreach (MethodInfo method in type.GetMethods())
                        {
                            foreach (Attributes.CommandDef attribute in method.GetCustomAttributes<Attributes.CommandDef>(false))
                            {
                                try
                                {
                                    CommandInfo info = new CommandInfo()
                                    {
                                        Command = (ConsoleCommand)ConsoleCommand.CreateDelegate(typeof(ConsoleCommand), method, true),
                                        Name = attribute.Name,
                                        Usage = attribute.Usage,
                                        Help = attribute.Help
                                    };

                                    this.RegisterCommand(info);
                                }
                                catch (Exception e)
                                {
                                    Log.Warn(string.Format("Unable to register command \"{0}\"", attribute.Name), e);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the console variables.
        /// </summary>
        /// <value>
        /// The console variables.
        /// </value>
        public Dictionary<string, IConVar> Variables
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the console commands.
        /// </summary>
        /// <value>
        /// The console commands.
        /// </value>
        public Dictionary<string, CommandInfo> Commands
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the game engine.
        /// </summary>
        /// <value>
        /// The game engine.
        /// </value>
        public Engine GameEngine
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a console variable.
        /// </summary>
        /// <param name="name">The convar name.</param>
        /// <returns>The convar interface.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Unknown variable.</exception>
        public IConVar GetVariable(string name)
        {
            IConVar value = null;
            if (!this.Variables.TryGetValue(name, out value))
            {
                throw new KeyNotFoundException(string.Format("Unknown variable \"{0}\"", name));
            }

            return value;
        }

        /// <summary>
        /// Registers a console variable.
        /// </summary>
        /// <param name="name">The convar name.</param>
        /// <param name="value">The convar interface.</param>
        /// <exception cref="System.ArgumentException">Tried to register existing variable.</exception>
        public void RegisterVariable(string name, IConVar value)
        {
            if (this.Variables.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("Tried to register existing variable \"{0}\"", name));
            }

            this.Variables.Add(name, value);
        }

        /// <summary>
        /// Determines whether the specified convar exists.
        /// </summary>
        /// <param name="name">The convar name.</param>
        /// <returns>True if the convar exists, false if not.</returns>
        public bool ContainsVariable(string name)
        {
            return this.Variables.ContainsKey(name);
        }

        /// <summary>
        /// Registers a console command.
        /// </summary>
        /// <param name="info">The command definition.</param>
        /// <param name="force">If set to <c>true</c>, force register the command (overwriting existing commands).</param>
        /// <exception cref="System.ArgumentException">Tried to register existing command.</exception>
        public void RegisterCommand(CommandInfo info, bool force = false)
        {
            if (this.Commands.ContainsKey(info.Name))
            {
                if (force)
                {
                    this.Commands.Remove(info.Name);
                }
                else
                {
                    throw new ArgumentException(string.Format("Tried to register existing command \"{0}\"", info.Name));
                }
            }

            this.Commands.Add(info.Name, info);
        }

        /// <summary>
        /// Removes a console command.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Cannot remove non-existant command.</exception>
        public void RemoveCommand(string name)
        {
            if (!this.Commands.ContainsKey(name))
            {
                throw new KeyNotFoundException(string.Format("Cannot remove non-existant command \"{0}\"", name));
            }

            this.Commands.Remove(name);
        }

        /// <summary>
        /// Gets a console command.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>The command definition.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Unknown command.</exception>
        public CommandInfo GetCommand(string name)
        {
            CommandInfo info = null;
            if (!this.Commands.TryGetValue(name, out info))
            {
                throw new KeyNotFoundException(string.Format("Unknown command \"{0}\"", name));
            }

            return info;
        }

        /// <summary>
        /// Determines whether the console contains the specified command.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>True if the command exists; false if not.</returns>
        public bool ContainsCommand(string name)
        {
            return this.Commands.ContainsKey(name);
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <exception cref="System.ArgumentException">
        /// Unknown command or variable
        /// or
        /// Wrong number of arguments.
        /// </exception>
        public void Execute(ExecutableCommand command)
        {
            foreach (ICommandArgument arg in command.Arguments)
            {
                arg.Console = this;
            }

            CommandInfo info = null;

            try
            {
                info = this.GetCommand(command.Name);
            }
            catch (KeyNotFoundException)
            {
                IConVar var = null;
                try
                {
                    var = this.GetVariable(command.Name);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException(string.Format("Unknown command or variable \"{0}\"", command.Name));
                }

                switch (command.Arguments.Count)
                {
                    default:
                        throw new ArgumentException(string.Format("Wrong number of arguments for <internal-get-set> (expected 0 or 1, got {0})", command.Arguments.Count));

                    case 0:
                        command.Arguments.Insert(0, new BasicCommandArgument() { RawValue = command.Name, Console = this });
                        command.Name = "get";
                        break;

                    case 1:
                        command.Arguments.Insert(0, new BasicCommandArgument() { RawValue = command.Name, Console = this });
                        command.Name = "set";
                        break;
                }

                this.Execute(command);

                return;
            }

            info.Command.Invoke(this, command);
        }

        /// <summary>
        /// Executes the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        public void Execute(CommandList commands)
        {
            foreach (ExecutableCommand command in commands.Commands)
            {
                this.Execute(command);
            }
        }

        /// <summary>
        /// Sets up basic script variables.
        /// </summary>
        protected virtual void SetupVariables()
        {
            this.RegisterVariable("null", new ImmutableConVar(null));
            this.RegisterVariable("s_version", new ImmutableConVar("1.0"));
        }
    }
}
