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
        /// <param name="console">The console.</param>
        public static void Build(ConsoleManager console)
        {
            console.RegisterVariable(
                "e_running",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return GameEngine.Instance.IsRunning.ToString();
                        }
                });

            console.RegisterVariable(
                "e_time_delta",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return GameEngine.Instance.Delta.ToString();
                        }
                });

            console.RegisterVariable(
                "e_timer",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return GameEngine.Instance.Timer.Elapsed.TotalSeconds.ToString();
                        }
                });

            console.RegisterVariable(
                "e_state",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            if (GameEngine.Instance.StateManager.CurrentState == null)
                            {
                                return "null";
                            }

                            return GameEngine.Instance.StateManager.CurrentState.GetType().ToString();
                        }
                });

            console.RegisterVariable(
                "e_fps",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return GameEngine.Instance.FPS.ToString();
                        }
                });

            console.RegisterVariable(
                "e_frameskip",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return GameEngine.Instance.FrameSkip.ToString();
                        }
                });

            console.RegisterVariable(
                "e_clearcolor",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return GameEngine.Instance.ClearColor.R.ToString() + ", " + GameEngine.Instance.ClearColor.G.ToString() + ", " + GameEngine.Instance.ClearColor.B.ToString();
                        }
                });

            console.RegisterVariable(
                "e_clearcolor_r",
                new DelegateConVar()
                {
                    GetFunc = () =>
                        {
                            return GameEngine.Instance.ClearColor.R.ToString();
                        },

                    SetFunc = (value) =>
                        {
                            GameEngine.Instance.ClearColor = new SFML.Graphics.Color(byte.Parse(value), GameEngine.Instance.ClearColor.G, GameEngine.Instance.ClearColor.B, GameEngine.Instance.ClearColor.A);
                        }
                });

            console.RegisterVariable(
                "e_clearcolor_g",
                new DelegateConVar()
                {
                    GetFunc = () =>
                    {
                        return GameEngine.Instance.ClearColor.G.ToString();
                    },

                    SetFunc = (value) =>
                    {
                        GameEngine.Instance.ClearColor = new SFML.Graphics.Color(GameEngine.Instance.ClearColor.R, byte.Parse(value), GameEngine.Instance.ClearColor.B, GameEngine.Instance.ClearColor.A);
                    }
                });

            console.RegisterVariable(
                "e_clearcolor_b",
                new DelegateConVar()
                {
                    GetFunc = () =>
                    {
                        return GameEngine.Instance.ClearColor.B.ToString();
                    },

                    SetFunc = (value) =>
                    {
                        GameEngine.Instance.ClearColor = new SFML.Graphics.Color(GameEngine.Instance.ClearColor.R, GameEngine.Instance.ClearColor.G, byte.Parse(value), GameEngine.Instance.ClearColor.A);
                    }
                });

            console.RegisterVariable(
                "e_task_count",
                new DelegateConVar()
                {
                    GetFunc = () =>
                    {
                        return GameEngine.Instance.Scheduler.Tasks.Count.ToString();
                    }
                });
        }
    }
}
