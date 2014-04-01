namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Script;
    using Dive.Script.ConVars;

    /// <summary>
    /// Interfaces the engine and the console.
    /// </summary>
    public static class EngineScriptInterface
    {
        /// <summary>
        /// Build the console variables for interfacing the engine and the console.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="console">The console.</param>
        public static void Build(Engine engine, ConsoleManager console)
        {
            console.RegisterVariable(
                "e_running",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return engine.IsRunning.ToString();
                        }
                });

            console.RegisterVariable(
                "e_time_delta",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return engine.Delta.ToString();
                        }
                });

            console.RegisterVariable(
                "e_timer",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return engine.Timer.Elapsed.TotalSeconds.ToString();
                        }
                });

            console.RegisterVariable(
                "e_state",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            if (engine.StateManager.CurrentState == null)
                            {
                                return "null";
                            }

                            return engine.StateManager.CurrentState.GetType().ToString();
                        }
                });

            console.RegisterVariable(
                "e_fps",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return engine.FPS.ToString();
                        }
                });

            console.RegisterVariable(
                "e_frameskip",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return engine.FrameSkip.ToString();
                        }
                });

            console.RegisterVariable(
                "e_clearcolor",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return engine.ClearColor.R.ToString() + ", " + engine.ClearColor.G.ToString() + ", " + engine.ClearColor.B.ToString();
                        }
                });

            console.RegisterVariable(
                "e_clearcolor_r",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return engine.ClearColor.R.ToString();
                        },

                    SetFunc = (value) =>
                        {
                            engine.ClearColor = new SFML.Graphics.Color(byte.Parse(value), engine.ClearColor.G, engine.ClearColor.B, engine.ClearColor.A);
                        }
                });

            console.RegisterVariable(
                "e_clearcolor_g",
                new DelegateConVar()
                {
                    GetFunc = () =>
                    {
                        return engine.ClearColor.G.ToString();
                    },

                    SetFunc = (value) =>
                    {
                        engine.ClearColor = new SFML.Graphics.Color(engine.ClearColor.R, byte.Parse(value), engine.ClearColor.B, engine.ClearColor.A);
                    }
                });

            console.RegisterVariable(
                "e_clearcolor_b",
                new DelegateConVar()
                {
                    GetFunc = () =>
                    {
                        return engine.ClearColor.B.ToString();
                    },

                    SetFunc = (value) =>
                    {
                        engine.ClearColor = new SFML.Graphics.Color(engine.ClearColor.R, engine.ClearColor.G, byte.Parse(value), engine.ClearColor.A);
                    }
                });

            console.RegisterVariable(
                "e_task_count",
                new DelegateConVar()
                {
                    GetFunc = () =>
                    {
                        return engine.Scheduler.Tasks.Count.ToString();
                    }
                });
        }
    }
}
