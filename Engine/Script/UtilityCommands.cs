namespace Dive.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Script.Attributes;
    using Dive.Script.ConVars;

    /// <summary>
    /// Utility console commands.
    /// </summary>
    public static class UtilityCommands
    {
        /// <summary>
        /// Command that does nothing.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        [CommandDef(Name = "nullcmd", Usage = "nullcmd", Help = "Does nothing")]
        public static void NullCommand(ConsoleManager console, ExecutableCommand cmd)
        {
        }

        /// <summary>
        /// Sets a console variable.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "set", Usage = "set <variable> <value>", Help = "Set the value of a convar")]
        public static void SetVariable(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for set (expected 2, got {0})", cmd.Arguments.Count));
            }

            string varName = cmd.Arguments[0].Value;
            string value = cmd.Arguments[1].Value;
            IConVar var = null;

            if (!console.ContainsVariable(varName))
            {
                var = new BasicConVar();
                console.RegisterVariable(varName, var);
            }
            else
            {
                var = console.GetVariable(varName);
            }

            var.Value = value;

            ConsoleManager.ConsoleLog.Info(string.Format("set {0} = \"{1}\"", varName, value));
        }

        /// <summary>
        /// Gets a console variable.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "get", Usage = "get <variable>", Help = "Get the value of a convar")]
        public static void GetVariable(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for get (expected 1, got {0})", cmd.Arguments.Count));
            }

            string varName = cmd.Arguments[0].Value;

            IConVar var = console.GetVariable(varName);
            ConsoleManager.ConsoleLog.Info(string.Format("{0} = {1}", varName, var.Value));
        }

        /// <summary>
        /// Echoes text to the console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        [CommandDef(Name = "echo", Usage = "echo <message...>", Help = "Echo arguments to the console")]
        public static void Echo(ConsoleManager console, ExecutableCommand cmd)
        {
            string str = string.Empty;
            foreach (ICommandArgument arg in cmd.Arguments)
            {
                str += arg.Value + " ";
            }

            if (str.Length > 0)
            {
                str = str.Substring(0, str.Length - 1);
            }

            ConsoleManager.ConsoleLog.Info(str);
        }

        /// <summary>
        /// Clears the console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "clear", Usage = "clear", Help = "Clear the console")]
        public static void Clear(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for clear (expected 0, got {0})", cmd.Arguments.Count));
            }

            console.GameEngine.ConsoleViewer.Clear();
        }

        /// <summary>
        /// Prints help to the console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "help", Usage = "help [command]", Help = "Show help for a command, or list all commands")]
        public static void Help(ConsoleManager console, ExecutableCommand cmd)
        {
            switch (cmd.Arguments.Count)
            {
                default:
                    throw new ArgumentException(string.Format("Wrong number of arguments for help (expected 0 or 1, got {0})", cmd.Arguments.Count));

                case 0:
                    {
                        foreach (CommandInfo command in console.Commands.Values.OrderBy(i => i.Name))
                        {
                            ConsoleManager.ConsoleLog.Info(string.Format("{0} - {1}", command.Usage, command.Help));
                        }

                        break;
                    }

                case 1:
                    {
                        string name = cmd.Arguments[0].Value;
                        CommandInfo command = null;
                        try
                        {
                            command = console.GetCommand(name);
                        }
                        catch (Exception)
                        {
                            ConsoleManager.ConsoleLog.Warn(string.Format("Unknown command \"{0}\"", name));
                            return;
                        }

                        ConsoleManager.ConsoleLog.Info(string.Format("{0} - {1}", command.Usage, command.Help));

                        break;
                    }
            }
        }

        /// <summary>
        /// Print the list of console variables to the console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "convars", Usage = "convars", Help = "List all convars")]
        public static void ConVars(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for convars (expected 0, got {0})", cmd.Arguments.Count));
            }

            foreach (string name in console.Variables.Keys.OrderBy(i => i))
            {
                ConsoleManager.ConsoleLog.Info(name);
            }
        }

        /// <summary>
        /// Create an alias (user-defined function).
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "alias", Usage = "alias <name> <commands>", Help = "Create a command alias")]
        public static void Alias(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for alias (expected 2, got {0})", cmd.Arguments.Count));
            }

            string name = cmd.Arguments[0].Value;

            console.RegisterCommand(
                new CommandInfo()
                {
                    Command = AliasExecution,
                    Name = name,
                    Usage = name + " (ALIAS)",
                    Help = cmd.Arguments[1].Value
                },
                true);
        }

        /// <summary>
        /// Executes commands based on equality.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "if_equal", Usage = "if_equal <a> <b> <true> [false]", Help = "Execute <true> if <a> == <b> and [false] if not")]
        public static void IfEqual(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 3 && cmd.Arguments.Count != 4)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for if_equal (expected 3 or 4, got {0})", cmd.Arguments.Count));
            }

            string a = cmd.Arguments[0].Value;
            string b = cmd.Arguments[1].Value;
            string ifTrue = cmd.Arguments[2].Value;
            string ifFalse = cmd.Arguments.Count == 4 ? cmd.Arguments[3].Value : null;

            if (ifTrue != null && !string.Empty.Equals(ifTrue) && a.Equals(b))
            {
                CommandList commands = ScriptUtilities.ParseString(ifTrue);
                console.Execute(commands);
            }
            else if (ifFalse != null && !string.Empty.Equals(ifFalse))
            {
                CommandList commands = ScriptUtilities.ParseString(ifFalse);
                console.Execute(commands);
            }
        }

        /// <summary>
        /// Executes commands based on inequality.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "if_greater", Usage = "if_greater <a> <b> <true> [false]", Help = "Execute <true> if (double)<a> > <b> and [false] if not")]
        public static void IfGreater(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 3 && cmd.Arguments.Count != 4)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for if_greater (expected 3 or 4, got {0})", cmd.Arguments.Count));
            }

            double a = double.Parse(cmd.Arguments[0].Value);
            double b = double.Parse(cmd.Arguments[1].Value);
            string ifTrue = cmd.Arguments[2].Value;
            string ifFalse = cmd.Arguments.Count == 4 ? cmd.Arguments[3].Value : null;

            if (ifTrue != null && !string.Empty.Equals(ifTrue) && a > b)
            {
                CommandList commands = ScriptUtilities.ParseString(ifTrue);
                console.Execute(commands);
            }
            else if (ifFalse != null && !string.Empty.Equals(ifFalse))
            {
                CommandList commands = ScriptUtilities.ParseString(ifFalse);
                console.Execute(commands);
            }
        }

        /// <summary>
        /// Executes commands based on inequality.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "if_less", Usage = "if_less <a> <b> <true> [false]", Help = "Execute <true> if (double)<a> < <b> and [false] if not")]
        public static void IfLess(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 3 && cmd.Arguments.Count != 4)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for if_less (expected 3 or 4, got {0})", cmd.Arguments.Count));
            }

            double a = double.Parse(cmd.Arguments[0].Value);
            double b = double.Parse(cmd.Arguments[1].Value);
            string ifTrue = cmd.Arguments[2].Value;
            string ifFalse = cmd.Arguments.Count == 4 ? cmd.Arguments[3].Value : null;

            if (ifTrue != null && !string.Empty.Equals(ifTrue) && a < b)
            {
                CommandList commands = ScriptUtilities.ParseString(ifTrue);
                console.Execute(commands);
            }
            else if (ifFalse != null && !string.Empty.Equals(ifFalse))
            {
                CommandList commands = ScriptUtilities.ParseString(ifFalse);
                console.Execute(commands);
            }
        }

        /// <summary>
        /// Repeats commands a specified number of times.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "repeat", Usage = "repeat <n> <commands>", Help = "Repeat <commands> for <n> times")]
        public static void Repeat(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for repeat (expected 2, got {0})", cmd.Arguments.Count));
            }

            int n = int.Parse(cmd.Arguments[0].Value);
            string cmds = cmd.Arguments[1].Value;

            CommandList commands = ScriptUtilities.ParseString(cmds);
            for (int i = 0; i < n; i++)
            {
                console.Execute(commands);
            }
        }

        /// <summary>
        /// Used internally by the alias command to execute aliases.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        private static void AliasExecution(ConsoleManager console, ExecutableCommand cmd)
        {
            CommandList commands = ScriptUtilities.ParseString(console.GetCommand(cmd.Name).Help);
            console.Execute(commands);
        }
    }
}
