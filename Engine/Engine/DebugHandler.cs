namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine.Extensions;
    using Dive.Entity;
    using FarseerPhysics;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Dynamics;
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
        /// Gets or sets a value indicating whether to draw physics bodies.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [draw physics]; otherwise, <c>false</c>.
        /// </value>
        public bool DrawPhysics
        {
            get;
            set;
        }

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
            bool drawPhysics = false;
            bool.TryParse(GameEngine.Instance.Configuration["debug"]["draw-physics"], out drawPhysics);
            this.DrawPhysics = drawPhysics;

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
            if (this.DrawPhysics)
            {
                foreach (Body body in GameEngine.Instance.PhysicsWorld.BodyList)
                {
                    SFML.Window.Vector2f pos = VectorExtensions.FromXnaVector(ConvertUnits.ToDisplayUnits(body.Position));

                    foreach (Fixture fixture in body.FixtureList)
                    {
                        switch (fixture.Shape.ShapeType)
                        {
                            case ShapeType.Polygon:
                                {
                                    PolygonShape shape = (PolygonShape)fixture.Shape;
                                    ConvexShape convex = new ConvexShape((uint)shape.Vertices.Count);
                                    for (int i = 0; i < shape.Vertices.Count; i++)
                                    {
                                        convex.SetPoint((uint)i, VectorExtensions.FromXnaVector(ConvertUnits.ToDisplayUnits(shape.Vertices[i])));
                                    }

                                    convex.FillColor = new Color(0, 0, 0, 0);
                                    convex.OutlineColor = new Color(255, 0, 0, 255);
                                    convex.OutlineThickness = 1;

                                    convex.Position = pos;
                                    convex.Rotation = (float)(body.Rotation * (180 / Math.PI));

                                    GameEngine.Instance.Window.Draw(convex);

                                    break;
                                }

                            case ShapeType.Chain:
                                {
                                    ChainShape shape = (ChainShape)fixture.Shape;
                                    VertexArray vertices = new VertexArray(PrimitiveType.LinesStrip);
                                    for (int i = 0; i < shape.Vertices.Count; i++)
                                    {
                                        vertices.Append(
                                            new Vertex(
                                                VectorExtensions.FromXnaVector(ConvertUnits.ToDisplayUnits(shape.Vertices[i])) + pos,
                                                new Color(255, 0, 0, 255)));
                                    }

                                    GameEngine.Instance.Window.Draw(vertices);

                                    break;
                                }
                        }
                    }
                }
            }

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
                    "Inactive: {11}\n" +
                    "Bodies: {12}\n",
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
                    inactive,
                    GameEngine.Instance.PhysicsWorld.BodyList.Count);

                this.debugText.Position = GameEngine.Instance.Window.MapPixelToCoords(new SFML.Window.Vector2i(10, 10));
                this.debugText.Rotation = -GameEngine.Instance.Window.GetView().Rotation;
                GameEngine.Instance.Window.Draw(this.debugText);
            }
        }
    }
}
