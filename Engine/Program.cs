namespace Dive
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using log4net;
    using log4net.Config;

    /// <summary>
    /// Holds the Main method.
    /// </summary>
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static Engine.Engine engine = null;

        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            if (!Directory.Exists("config") ||
                !Directory.Exists("content") ||
                !Directory.Exists("scripts"))
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("FATAL ERROR: The working directory seems to be incorrect (directories missing).");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            if (!File.Exists("config/logging.xml") ||
                !File.Exists("config/engine.ini") ||
                !File.Exists("config/input.ini"))
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("FATAL ERROR: Missing configuration files.");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            XmlConfigurator.Configure(new System.IO.FileInfo("config/logging.xml"));

            Log.Info("New session started.");
#if DEBUG
            Log.Info("Running in DEBUG mode");
#else
            Log.Info("Running in RELEASE mode");
#endif

            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelHandler);
            Log.Debug("Registered ConsoleCancelEvent handler");

            engine = new Engine.Engine();

#if !DEBUG
            try
            {
#endif
                if (engine.Run())
                {
                    Log.Info("Session ended normally.");
                }
                else
                {
                    Log.Warn("Session ended with errors.");
#if !DEBUG
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
#endif
                }
#if !DEBUG
            }
            catch (Exception e)
            {
                Log.Fatal("Session ended with exception.", e);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
#endif
        }

        /// <summary>
        /// Handles ConsoleCancel events.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">Event arguments.</param>
        protected static void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            if (!args.Cancel && engine != null && engine.IsRunning)
            {
                Log.Warn("Stopping engine due to ConsoleCancelEvent");
                engine.Stop();
                args.Cancel = true;
            }
        }
    }
}
