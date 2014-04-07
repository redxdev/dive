namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine.Extensions;
    using Dive.Entity;
    using SFML.Graphics;

    /// <summary>
    /// Basic implementation of a debug handler. Replaces the old system of all debug methods acting
    /// inside the Engine class.
    /// </summary>
    public class DebugHandler
    {
        private Text debugText = null;

        private double avgFps = 0;

        private double displayFps = 0;

        private double avgTps = 0;

        private double displayTps = 0;

        private float timeToUpdate = 0.25f;

        /// <summary>
        /// Gets or sets a value indicating whether to draw the debug overlay.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [debug overlay]; otherwise, <c>false</c>.
        /// </value>
        public bool DrawOverlay
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes this debug handler.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public virtual void Initialize()
        {
            bool drawOverlay = false;
            bool.TryParse(GameEngine.Instance.Configuration["debug"]["draw-overlay"], out drawOverlay);
            this.DrawOverlay = drawOverlay;

            this.debugText = new Text("INIT", GameEngine.Instance.AssetManager.Load<Font>("content/fonts/DroidSansMono.ttf"), 14);
            this.debugText.Color = Color.White;
        }

        /// <summary>
        /// Draws debug information. This is always called after all engine drawing routines, and as such draw calls will appear on top of
        /// all objects on the screen. This should not use engine drawing methods, and instead directly draw to the engine.Window object.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public virtual void Draw()
        {
            if (this.DrawOverlay)
            {
                uint active = 0;
                uint inactive = 0;
                foreach (Entity ent in GameEngine.Instance.EntityManager.Entities.Values)
                {
                    if (ent.IsActive)
                    {
                        active++;
                    }
                    else
                    {
                        inactive++;
                    }
                }

                this.avgFps += GameEngine.Instance.FPS;
                this.avgFps /= 2;

                this.avgTps += GameEngine.Instance.TPS;
                this.avgTps /= 2;

                if (this.timeToUpdate <= 0)
                {
                    this.displayFps = this.avgFps;
                    this.displayTps = this.avgTps;
                    this.timeToUpdate = 0.25f;
                }

                this.timeToUpdate -= (float)GameEngine.Instance.Delta;

                this.debugText.DisplayedString = string.Format(
                    "FPS: {0:0.0}  Timer: {1:0.000000}\n" +
                    "TPS: {2:0.0}  Tasks: {3}\n" +
                    "FrameSkip: {4}  VR: {5}x{6}\n" +
                    "Assets: {7}   WR: {8}x{9}\n" +
                    "Active: {10}\n" +
                    "Inactive: {11}\n",
                    this.displayFps,
                    GameEngine.Instance.Timer.Elapsed.TotalSeconds,
                    this.displayTps,
                    GameEngine.Instance.Scheduler.Tasks.Count,
                    GameEngine.Instance.FrameSkip,
                    GameEngine.Instance.Window.GetView().Size.X,
                    GameEngine.Instance.Window.GetView().Size.Y,
                    GameEngine.Instance.AssetManager.GetAssetCount(),
                    GameEngine.Instance.Window.Size.X,
                    GameEngine.Instance.Window.Size.Y,
                    active,
                    inactive);

                this.debugText.Position = GameEngine.Instance.Window.MapPixelToCoords(new SFML.Window.Vector2i(10, 10));
                this.debugText.Rotation = -GameEngine.Instance.Window.GetView().Rotation;
                GameEngine.Instance.Window.Draw(this.debugText);
            }
        }
    }
}
