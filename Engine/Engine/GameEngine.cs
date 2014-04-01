namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine.Extensions;
    using Dive.Engine.Scheduler;
    using Dive.Entity;
    using Dive.Script;
    using FarseerPhysics;
    using FarseerPhysics.Dynamics;
    using log4net;
    using SFML.Graphics;

    /// <summary>
    /// Handles the main game loop.
    /// </summary>
    public class GameEngine
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameEngine));

        private static GameEngine instance = null;

        private Assets.AssetManager assetManager = null;

        private IniParser.IniData configuration = null;

        private bool isRunning = false;

        private Stopwatch timer = null;

        private EntityManager entityManager = null;

        private GameStateManager stateManager = null;

        private RenderWindow window = null;

        private InputManager input = null;

        private List<EngineDrawable> drawList = new List<EngineDrawable>();

        private World physicsWorld = null;

        private DiveScheduler scheduler = new DiveScheduler();

        private ConsoleManager console = null;

        private ConsoleViewer consoleViewer = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngine" /> class.
        /// </summary>
        public GameEngine()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("Engine instance already exists");
            }

            instance = this;

            Log.Debug("Initializing Engine");

            this.ClearColor = ColorConstants.CornflowerBlue;
            this.LargestDrawLayer = int.MinValue;

            this.console = new ConsoleManager(true);
            this.entityManager = new EntityManager();
            this.assetManager = new Assets.AssetManager(true);
            this.stateManager = new GameStateManager(true);
            this.input = new InputManager();
            this.physicsWorld = new World(new Microsoft.Xna.Framework.Vector2(0f, 0f));
            this.consoleViewer = new ConsoleViewer();

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            this.Input.ActionEvent += this.OnInputAction;

            Log.Debug("Initialized Engine");
        }

        public static GameEngine Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the largest draw layer.
        /// </summary>
        /// <value>
        /// The largest draw layer.
        /// </value>
        public int LargestDrawLayer
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the engine's <see cref="Dive.Assets.AssetManager" /> instance.
        /// </summary>
        /// <value>The engine's <see cref="Dive.Assets.AssetManager" /> instance.</value>
        public Assets.AssetManager AssetManager
        {
            get
            {
                return this.assetManager;
            }
        }

        /// <summary>
        /// Gets the engine's main configuration.
        /// </summary>
        /// <value>The engine's main configuration.</value>
        public IniParser.IniData Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the engine is running.
        /// </summary>
        /// <value>True if the engine is running, false if not.</value>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        /// <summary>
        /// Gets the engine's timer.
        /// </summary>
        /// <value>The engine's timer.</value>
        public Stopwatch Timer
        {
            get
            {
                return this.timer;
            }
        }

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <value>The world.</value>
        public EntityManager EntityManager
        {
            get
            {
                return this.entityManager;
            }
        }

        /// <summary>
        /// Gets the <see cref="GameStateManager"/>.
        /// </summary>
        /// <value>The <see cref="GameStateManager"/>.</value>
        public GameStateManager StateManager
        {
            get
            {
                return this.stateManager;
            }
        }

        /// <summary>
        /// Gets the engine's RenderWindow.
        /// </summary>
        /// <value>The engine's RenderWindow.</value>
        public RenderWindow Window
        {
            get
            {
                return this.window;
            }
        }

        /// <summary>
        /// Gets the <see cref="InputManager" />.
        /// </summary>
        /// <value>The <see cref="InputManager" /></value>
        public InputManager Input
        {
            get
            {
                return this.input;
            }
        }

        /// <summary>
        /// Gets the physics world.
        /// </summary>
        /// <value>
        /// The physics world.
        /// </value>
        public World PhysicsWorld
        {
            get
            {
                return this.physicsWorld;
            }
        }

        /// <summary>
        /// Gets the console.
        /// </summary>
        /// <value>
        /// The console.
        /// </value>
        public ConsoleManager Console
        {
            get
            {
                return this.console;
            }
        }

        /// <summary>
        /// Gets the console viewer.
        /// </summary>
        /// <value>
        /// The console viewer.
        /// </value>
        public ConsoleViewer ConsoleViewer
        {
            get
            {
                return this.consoleViewer;
            }
        }

        /// <summary>
        /// Gets the task scheduler.
        /// <para>
        /// Synchronous tasks are always run at the end of each Update tick. Additionally, due to the way the scheduler is
        /// written, all expiring tasks will run, no matter how long they take. As a result, only very short operations
        /// should be scheduled synchronously.
        /// </para>
        /// </summary>
        /// <value>
        /// The task scheduler.
        /// </value>
        public DiveScheduler Scheduler
        {
            get
            {
                return this.scheduler;
            }
        }

        /// <summary>
        /// Gets or sets the number of ticks per second.
        /// </summary>
        /// <value>Ticks per second.</value>
        public double TPS
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the number of frames per second.
        /// </summary>
        /// <value>Frames per second.</value>
        public double FPS
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the number of frames skipped.
        /// </summary>
        /// <value>The number of frames skipped.</value>
        public int FrameSkip
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the delta time in seconds.
        /// </summary>
        /// <value>Delta time in seconds.</value>
        public double Delta
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the clear color.
        /// </summary>
        /// <value>
        /// The clear color.
        /// </value>
        public Color ClearColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the debug handler.
        /// </summary>
        /// <value>
        /// The debug handler.
        /// </value>
        public DebugHandler Debug
        {
            get;
            protected set;
        }

        /// <summary>
        /// Sets the current engine instance as inactive.
        /// </summary>
        public static void CleanupInstance()
        {
            instance = null;
        }

        /// <summary>
        /// Tell the engine to stop execution at the end of the current loop.
        /// </summary>
        public void Stop()
        {
            if (!this.IsRunning)
            {
                throw new InvalidOperationException("Called Stop() on an engine that isn't running");
            }

            this.isRunning = false;

            Log.Debug("Set running flag to false. Will halt execution on next loop.");
        }

        /// <summary>
        /// Start the engine's game loop. This method will block for as long as the game loop is running.
        /// </summary>
        /// <returns>True if the game exits normally, false if there was a problem starting the engine.</returns>
        public bool Run()
        {
            Log.Debug("Starting game loop...");

            try
            {
                this.configuration = this.AssetManager.Load<IniParser.IniData>("config/engine.ini");
            }
            catch (Assets.AssetException e)
            {
                Log.Fatal("Unable to load engine configuration", e);
                Log.Fatal("Unable to initialize engine");
                return false;
            }
            
            double configTickRate = 0;
            if (!double.TryParse(this.Configuration["engine"]["tickrate"], out configTickRate) || configTickRate <= 0)
            {
                Log.Fatal("Configuration: Invalid input for engine.tickrate");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            double tickrate = 1 / configTickRate;

            Log.Info("engine.tickrate = " + tickrate);

            double configDrawRate = 0;
            if (!double.TryParse(this.Configuration["engine"]["drawrate"], out configDrawRate) || configDrawRate <= 0)
            {
                Log.Fatal("Configuration: Invalid input for engine.drawrate");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            double drawrate = 1 / configDrawRate;

            Log.Info("engine.drawrate = " + drawrate);

            int maxSkippedFrames = 0;
            if (!int.TryParse(this.configuration["engine"]["max-skipped-frames"], out maxSkippedFrames) || maxSkippedFrames < 0)
            {
                Log.Fatal("Configuration: Invalid input for engine.max-skipped-frames");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            double cleanAssetsAfter = 0;
            if (!double.TryParse(this.Configuration["assets"]["clean-after"], out cleanAssetsAfter))
            {
                Log.Fatal("Configuration: Invalid input for assets.clean-after");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            Log.Info("assets.clean-after = " + cleanAssetsAfter);

            int skippedFrames = 0;
            double maxTimeDiff = 0.5;
            double nextTick = tickrate;
            double nextDraw = drawrate;
            double lastTick = 0;
            double cleanTime = 0;
            double lastDraw = 0;

            if (!this.Startup())
            {
                Log.Fatal("Engine startup failed");
                Log.Fatal("Unable to initialize engine");

                if (this.Window != null)
                {
                    this.Window.Close();
                }

                return false;
            }

            Log.Info("Started game loop");

            this.isRunning = true;

            this.timer = new Stopwatch();
            this.Timer.Start();
            while (this.IsRunning)
            {
                if (!this.Window.IsOpen())
                {
                    Log.Info("RenderWindow closed; stopping engine.");
                    this.Stop();
                    continue;
                }

                double currentTime = this.Timer.Elapsed.TotalSeconds;

                this.Delta = currentTime - lastTick;

                this.FrameSkip = skippedFrames;

                if (cleanAssetsAfter >= 0)
                {
                    cleanTime -= this.Delta;
                    if (cleanTime <= 0)
                    {
                        cleanTime = cleanAssetsAfter;
                        this.AssetManager.Clean();
                    }
                }

                if ((currentTime - nextTick) > maxTimeDiff)
                {
                    nextTick = currentTime;
                }

                bool didWork = false;

                if (currentTime >= nextTick)
                {
                    if (currentTime - lastTick != 0)
                    {
                        this.TPS = 1 / this.Delta;
                    }

                    lastTick = currentTime;

                    nextTick += tickrate;
                    this.Update(nextTick);
                    didWork = true;
                }

                if (currentTime >= nextDraw)
                {
                    if (currentTime < nextTick || skippedFrames >= maxSkippedFrames)
                    {
                        if (currentTime - lastDraw != 0)
                        {
                            this.FPS = 1 / (currentTime - lastDraw);
                        }

                        lastDraw = currentTime;

                        this.Draw();

                        skippedFrames = 0;
                        didWork = true;
                        nextDraw += drawrate;
                    }
                    else
                    {
                        skippedFrames++;
                    }
                }

                if (!didWork)
                {
                    int sleepTime = (int)(1000.0 * ((nextTick < nextDraw ? nextTick : nextDraw) - currentTime));
                    this.Timer.Restart();
                    nextTick -= currentTime;
                    nextDraw -= currentTime;
                    lastTick -= currentTime;
                    lastDraw -= currentTime;
                    if (sleepTime > 0)
                    {
                        System.Threading.Thread.Sleep(sleepTime);
                    }
                }
            }

            Log.Info("Stopping game loop...");

            this.Timer.Stop();

            this.Shutdown();

            Log.Info("Game loop stopped");

            return true;
        }

        /// <summary>
        /// Adds to render queue.
        /// </summary>
        /// <param name="drawable">The drawable.</param>
        public void AddToRenderQueue(Drawable drawable)
        {
            this.AddToRenderQueue(drawable, this.LargestDrawLayer + 1);
        }

        /// <summary>
        /// Adds to render queue.
        /// </summary>
        /// <param name="drawable">The drawable.</param>
        /// <param name="layer">The layer.</param>
        public void AddToRenderQueue(Drawable drawable, int layer)
        {
            this.drawList.Add(new EngineDrawable()
                {
                    Drawable = drawable,
                    Layer = layer
                });
            if (this.LargestDrawLayer < layer)
            {
                this.LargestDrawLayer = layer;
            }
        }

        /// <summary>
        /// Called before starting the game loop.
        /// </summary>
        /// <returns>True on success, false if the engine shouldn't start.</returns>
        protected virtual bool Startup()
        {
            uint resW = 0;
            if (!uint.TryParse(this.Configuration["window"]["resW"], out resW))
            {
                Log.Fatal("Configuration: Invalid input for window.resW");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            Log.Info("window.resW = " + resW);

            uint resH = 0;
            if (!uint.TryParse(this.Configuration["window"]["resH"], out resH))
            {
                Log.Fatal("Configuration: Invalid input for window.resH");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            Log.Info("window.resH = " + resH);

            uint bpp = 0;
            if (!uint.TryParse(this.Configuration["window"]["bpp"], out bpp))
            {
                Log.Fatal("Configuration: Invalid input for window.bpp");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            Log.Info("window.bpp = " + bpp);

            bool fullscreen = false;
            if (!bool.TryParse(this.Configuration["window"]["fullscreen"], out fullscreen))
            {
                Log.Fatal("Configuration: Invalid input for window.fullscreen");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            Log.Info("window.fullscreen = " + fullscreen);

            bool vsync = false;
            if (!bool.TryParse(this.Configuration["window"]["vsync"], out vsync))
            {
                Log.Fatal("Configuration: Invalid input for window.vsync");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            Log.Info("window.vsync = " + vsync);

            System.Console.Title = this.Configuration["engine"]["title"] + " | CONSOLE";
            this.window = new RenderWindow(
                new SFML.Window.VideoMode(
                    resW,
                    resH,
                    bpp),
                this.Configuration["engine"]["title"],
                fullscreen ? SFML.Window.Styles.Fullscreen : SFML.Window.Styles.Default);

            this.Window.SetVerticalSyncEnabled(vsync);

            this.Window.Closed += this.OnWindowClosed;
            this.Window.Resized += this.OnWindowResized;

            this.Input.Initialize();
            this.StateManager.Initialize();

            bool debugEnabled = false;
            if (!bool.TryParse(this.Configuration["debug"]["enabled"], out debugEnabled))
            {
                Log.Fatal("Configuration: Invalid input for debug.enabled");
                Log.Fatal("Unable to initialize engine");
                return false;
            }

            if (debugEnabled)
            {
                Type debugHandlerType = Type.GetType(this.Configuration["debug"]["handler"]);
                if (debugHandlerType == null)
                {
                    Log.Fatal("Configuration: Unknown debug handler type \"" + this.Configuration["debug"]["handler"] + "\"");
                    Log.Fatal("Unable to initialize engine");
                    return false;
                }

                if (!typeof(DebugHandler).IsAssignableFrom(debugHandlerType))
                {
                    Log.Fatal("Configuration: Specified debug handler is not a valid subclass of DebugHandler");
                    Log.Fatal("Unable to initialize engine");
                    return false;
                }

                Log.Debug("Using debug handler " + debugHandlerType.Name);

                this.Debug = (DebugHandler)Activator.CreateInstance(debugHandlerType);
                this.Debug.Initialize();
            }
            else
            {
                this.Debug = null;
            }

            EngineScriptInterface.Build(this.Console);

            try
            {
                CommandList initScript = this.AssetManager.Load<CommandList>("scripts/init.ds");
                this.Console.Execute(initScript);
            }
            catch (Exception e)
            {
                ConsoleManager.ConsoleLog.Warn("Unable to execute scripts/init.ds", e);
            }

            return true;
        }

        /// <summary>
        /// Called when the render window has it's close button pressed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void OnWindowClosed(object sender, EventArgs e)
        {
            this.Window.Close();
        }

        /// <summary>
        /// Called when the render window is resized.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void OnWindowResized(object sender, SFML.Window.SizeEventArgs e)
        {
            View view = this.Window.GetView();
            view.Size = new SFML.Window.Vector2f(e.Width, e.Height);
            this.Window.SetView(view);
        }

        /// <summary>
        /// Called whenever the engine requests a logic update.
        /// </summary>
        /// <param name="nextTick">The time of the next tick.</param>
        protected virtual void Update(double nextTick)
        {
            this.Window.DispatchEvents();

            this.PhysicsWorld.Step((float)this.Delta);
            this.EntityManager.Update();
            this.StateManager.Update();

            this.Scheduler.RunTasks((float)this.Delta, this.Timer, (float)nextTick);
        }

        /// <summary>
        /// Called whenever the engine requests rendering to take place.
        /// </summary>
        protected virtual void Draw()
        {
            this.Window.Clear(this.ClearColor);

            this.EntityManager.Draw();
            this.StateManager.Draw();

            this.drawList.Sort();
            foreach (EngineDrawable drawable in this.drawList)
            {
                this.Window.Draw(drawable.Drawable);
            }

            this.LargestDrawLayer = int.MinValue;
            this.drawList.Clear();

            this.ConsoleViewer.Draw();

            if (this.Debug != null)
            {
                this.Debug.Draw();
            }

            this.Window.Display();
        }

        /// <summary>
        /// Called after the game loop ends (but only if the game loop ended normally).
        /// </summary>
        protected virtual void Shutdown()
        {
            this.StateManager.Shutdown();

            if (this.Window.IsOpen())
            {
                this.Window.Close();
            }
        }

        /// <summary>
        /// Called when [input action].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="action">The action.</param>
        protected void OnInputAction(object sender, string action)
        {
            this.EntityManager.OnInputAction(action);
        }

        /// <summary>
        /// Handles the layer for drawables.
        /// </summary>
        private class EngineDrawable : IComparable<EngineDrawable>
        {
            /// <summary>
            /// Gets or sets the drawable.
            /// </summary>
            /// <value>
            /// The drawable.
            /// </value>
            public Drawable Drawable
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the layer.
            /// </summary>
            /// <value>
            /// The layer.
            /// </value>
            public int Layer
            {
                get;
                set;
            }

            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.
            /// </returns>
            public int CompareTo(EngineDrawable other)
            {
                return this.Layer - other.Layer;
            }
        }
    }
}
