namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine.Scheduler;
    using Dive.Script;
    using Dive.Script.Attributes;
    using Dive.Util;

    /// <summary>
    /// Engine console commands.
    /// </summary>
    public static class EngineCommands
    {
        /// <summary>
        /// Exits the engine safely.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        [CommandDef(Name = "exit", Usage = "exit", Help = "Exit the engine")]
        public static void Exit(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for exit (expected 0, got {0})", cmd.Arguments.Count));
            }

            GameEngine.Instance.Stop();
        }

        /// <summary>
        /// Changes the game state.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">
        /// Wrong number of arguments
        /// or
        /// Unknown type
        /// or
        /// Type is not assignable to IGameState.
        /// </exception>
        [CommandDef(Name = "changestate", Usage = "changestate <classname>", Help = "Change the main game state")]
        public static void ChangeState(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for changestate (expected 1, got {0})", cmd.Arguments.Count));
            }

            string typeName = cmd.Arguments[0].Value;
            Type type = TypeUtilities.GetGlobalType(typeName);
            if (type == null)
            {
                throw new ArgumentException(string.Format("Unknown type \"{0}\"", typeName));
            }

            if (!typeof(IGameState).IsAssignableFrom(type))
            {
                throw new ArgumentException(string.Format("Type \"{0}\" is not assignable to IGameState", typeName));
            }

            GameEngine.Instance.StateManager.ChangeState(type);
        }

        /// <summary>
        /// Creates a task with the specified time.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">
        /// Wrong number of arguments
        /// or
        /// Unable to parse to a float.
        /// </exception>
        [CommandDef(Name = "timer", Usage = "timer <seconds> <command>", Help = "Execute <commands> after <seconds>")]
        public static void Timer(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for timer (expected 2, got {0})", cmd.Arguments.Count));
            }

            string secondsString = cmd.Arguments[0].Value;
            float seconds = 0;
            if (!float.TryParse(secondsString, out seconds))
            {
                throw new ArgumentException(string.Format("Unable to parse \"{0}\" to a float", secondsString));
            }

            string command = cmd.Arguments[1].Value;

            TaskInfo task = new TaskInfo()
            {
                ExecuteAfter = seconds,
                Task = () =>
                    {
                        try
                        {
                            console.Execute(ScriptUtilities.ParseString(command));
                        }
                        catch (Exception e)
                        {
                            ConsoleManager.ConsoleLog.Warn("Unable to execute delayed commands", e);
                        }
                    }
            };

            GameEngine.Instance.Scheduler.ScheduleTask(task);
        }

        /// <summary>
        /// Cancels all tasks. This is a very dangerous command to use, as parts of the game (or engine) may
        /// depend on certain tasks running.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "canceltasks", Usage = "canceltasks", Help = "Clear all tasks from the scheduler !!! THIS IS DANGEROUS !!!")]
        public static void CancelTasks(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for canceltasks (expected 0, got {0})", cmd.Arguments.Count));
            }

            GameEngine.Instance.Scheduler.Tasks.Clear();
        }

        /// <summary>
        /// Runs a script. This uses the asset manager to cache file loads, and as such after editing a script an asset reload
        /// might be needed.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "include", Usage = "include <filename>", Help = "Run a script (uses the asset manager for caching)")]
        public static void Include(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for include (expected 1, got {0})", cmd.Arguments.Count));
            }

            string filename = cmd.Arguments[0].Value;

            CommandList commands = GameEngine.Instance.AssetManager.Load<CommandList>(filename);
            console.Execute(commands);
        }

        /// <summary>
        /// Captures a screenshot and saves it.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "capture", Usage = "capture <filename>", Help = "Take a screen shot and save it to a file (formats: bmp, png, tga and jpg)")]
        public static void Capture(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 1)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for capture (expected 1, got {0})", cmd.Arguments.Count));
            }

            SFML.Graphics.Image image = GameEngine.Instance.Window.Capture();
            if (!image.SaveToFile(cmd.Arguments[0].Value))
            {
                ConsoleManager.ConsoleLog.Warn(string.Format("Unable to save screen capture to {0}", cmd.Arguments[0].Value));
            }
        }
    }
}
