namespace Dive.Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine;
    using Dive.Script;
    using Dive.Script.Attributes;
    using Dive.Util;

    /// <summary>
    /// Asset console commands.
    /// </summary>
    public static class AssetCommands
    {
        /// <summary>
        /// Loads the specified asset.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">
        /// Wrong number of arguments
        /// or
        /// Unknown type.
        /// </exception>
        [CommandDef(Name = "asset_load", Usage = "asset_load <type> <key>", Help = "Loads an asset into memory")]
        public static void Load(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for asset_load (expected 2, got {0})", cmd.Arguments.Count));
            }

            string typeName = cmd.Arguments[0].Value;
            string key = cmd.Arguments[1].Value;

            Type type = TypeUtilities.GetGlobalType(typeName);

            if (type == null)
            {
                throw new ArgumentException(string.Format("Unknown type \"{0}\"", typeName));
            }

            GameEngine.Instance.AssetManager.Load(type, key);
        }

        /// <summary>
        /// Reloads the specified asset.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">
        /// Wrong number of arguments
        /// or
        /// Unknown type.
        /// </exception>
        [CommandDef(Name = "asset_reload", Usage = "asset_reload <type> <key>", Help = "Force reload an asset in memory (or load for first time use)")]
        public static void Reload(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for asset_load (expected 2, got {0})", cmd.Arguments.Count));
            }

            string typeName = cmd.Arguments[0].Value;
            string key = cmd.Arguments[1].Value;

            Type type = TypeUtilities.GetGlobalType(typeName);

            if (type == null)
            {
                throw new ArgumentException(string.Format("Unknown type \"{0}\"", typeName));
            }

            GameEngine.Instance.AssetManager.Reload(type, key);
        }

        /// <summary>
        /// Runs an asset cleanup.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">Wrong number of arguments.</exception>
        [CommandDef(Name = "asset_cleanup", Usage = "asset_cleanup", Help = "Tells the asset manager to clean up unused assets")]
        public static void Cleanup(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 0)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for asset_load (expected 0, got {0})", cmd.Arguments.Count));
            }

            GameEngine.Instance.AssetManager.Clean();
        }

        /// <summary>
        /// Gets the status of an asset.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.ArgumentException">
        /// Wrong number of arguments
        /// or
        /// Unknown type.
        /// </exception>
        [CommandDef(Name = "asset_status", Usage = "asset_status <type> <key>", Help = "Get the status of an asset")]
        public static void Status(ConsoleManager console, ExecutableCommand cmd)
        {
            if (cmd.Arguments.Count != 2)
            {
                throw new ArgumentException(string.Format("Wrong number of arguments for asset_load (expected 2, got {0})", cmd.Arguments.Count));
            }

            string typeName = cmd.Arguments[0].Value;
            string key = cmd.Arguments[1].Value;

            Type type = TypeUtilities.GetGlobalType(typeName);

            if (type == null)
            {
                throw new ArgumentException(string.Format("Unknown type \"{0}\"", typeName));
            }

            if (GameEngine.Instance.AssetManager.IsLoaded(type, key))
            {
                ConsoleManager.ConsoleLog.Info("loaded");
            }
            else
            {
                ConsoleManager.ConsoleLog.Info("unloaded");
            }
        }
    }
}
