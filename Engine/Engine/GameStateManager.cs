namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;

    /// <summary>
    /// Manages <see cref="IGameState"/>s.
    /// </summary>
    public class GameStateManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameStateManager));

        private Dictionary<Type, IGameState> gameStates = null;

        private IGameState currentState = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameStateManager"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="autoAddGameStates">Whether to automatically add <see cref="IGameState"/>s with the <see cref="Dive.Engine.Attributes.GameState"/> attribute.</param>
        public GameStateManager(bool autoAddGameStates = true)
        {
            this.gameStates = new Dictionary<Type, IGameState>();

            if (autoAddGameStates)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!typeof(IGameState).IsAssignableFrom(type))
                        {
                            continue;
                        }

                        foreach (Attributes.GameState attribute in type.GetCustomAttributes<Attributes.GameState>(false))
                        {
                            this.AddState((IGameState)Activator.CreateInstance(type));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the dictionary of <see cref="IGameState"/>s.
        /// </summary>
        /// <value>The dictionary of <see cref="IGameState"/>s.</value>
        public Dictionary<Type, IGameState> GameStates
        {
            get
            {
                return this.gameStates;
            }
        }

        /// <summary>
        /// Gets the current <see cref="IGameState"/> instance.
        /// </summary>
        /// <value>The current <see cref="IGameState"/> instance.</value>
        public IGameState CurrentState
        {
            get
            {
                return this.currentState;
            }
        }

        /// <summary>
        /// Register a <see cref="IGameState"/> with this <see cref="GameStateManager"/>.
        /// </summary>
        /// <param name="state">The game state.</param>
        public void AddState(IGameState state)
        {
            if (this.gameStates.ContainsKey(state.GetType()))
            {
                throw new InvalidOperationException("Game state of type \"" + state.GetType().FullName + "\" already registered");
            }

            Log.Debug("Adding game state to manager: " + state.GetType().FullName);

            this.gameStates.Add(state.GetType(), state);
        }

        /// <summary>
        /// Remove a <see cref="IGameState"/> from this <see cref="GameStateManager"/>. This will deactivate the state if it is active.
        /// </summary>
        /// <param name="state">The game state.</param>
        public void RemoveState(IGameState state)
        {
            if (!this.gameStates.ContainsKey(state.GetType()))
            {
                throw new InvalidOperationException("Game state of type \"" + state.GetType().FullName + "\" not registered");
            }

            Log.Debug("Removing game state from manager: " + state.GetType().FullName);

            this.gameStates.Remove(state.GetType());

            if (this.IsStateActive(state))
            {
                state.Stop(null);
                this.currentState = null;
            }
        }

        /// <summary>
        /// Initialize the game state manager and all <see cref="IGameState"/>s.
        /// </summary>
        public void Initialize()
        {
            GameEngine.Instance.Input.ActionEvent += this.OnInputAction;

            Log.Debug("Initializing game states");

            foreach (KeyValuePair<Type, IGameState> state in this.GameStates)
            {
                state.Value.Initialize();
            }

            Type startupType = null;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (!typeof(IGameState).IsAssignableFrom(type))
                    {
                        continue;
                    }

                    if (type.GetCustomAttributes(typeof(Attributes.StartupGameState), false).Length > 0)
                    {
                        Log.Debug("StartupGameState was set on " + type.FullName);
                        startupType = type;
                    }
                }
            }

            if (startupType != null)
            {
                this.ChangeState(startupType);
            }
        }

        /// <summary>
        /// Shut down the game state manager and all <see cref="IGameState"/>s.
        /// </summary>
        public void Shutdown()
        {
            Log.Debug("Shutting down game states");

            if (this.CurrentState != null)
            {
                this.CurrentState.Stop(null);
                this.currentState = null;
            }

            foreach (KeyValuePair<Type, IGameState> state in this.GameStates)
            {
                state.Value.Shutdown();
            }
        }

        /// <summary>
        /// Call update on the current game state.
        /// </summary>
        public void Update()
        {
            if (this.CurrentState != null)
            {
                this.CurrentState.Update();
            }
        }

        /// <summary>
        /// Call draw on the current game state.
        /// </summary>
        public void Draw()
        {
            if (this.CurrentState != null)
            {
                this.CurrentState.Draw();
            }
        }

        /// <summary>
        /// Does this manager contain a state.
        /// </summary>
        /// <param name="t">The state type.</param>
        /// <returns>Whether this manager contains the specified state.</returns>
        public bool HasState(Type t)
        {
            return this.gameStates.ContainsKey(t);
        }

        /// <summary>
        /// Does this manager contain a state.
        /// </summary>
        /// <typeparam name="T">The state type.</typeparam>
        /// <returns>Whether this manager contains the specified state.</returns>
        public bool HasState<T>()
        {
            return this.HasState(typeof(T));
        }

        /// <summary>
        /// Does this manager contain a specific instance of a state.
        /// </summary>
        /// <param name="state">The state instance.</param>
        /// <returns>Whether this manager contains the specified instance of a state.</returns>
        public bool HasState(IGameState state)
        {
            return this.GameStates.ContainsValue(state);
        }

        /// <summary>
        /// Change the current game state.
        /// </summary>
        /// <param name="t">The type to change the state to.</param>
        public void ChangeState(Type t)
        {
            IGameState nextState = null;
            if (!this.GameStates.TryGetValue(t, out nextState))
            {
                throw new InvalidOperationException("Game state of type \"" + t.FullName + "\" not registered");
            }

            if (this.CurrentState != null)
            {
                Log.Debug("Moving game state from " + this.CurrentState.GetType().FullName + " to " + t.FullName);

                this.CurrentState.Stop(nextState);
            }
            else
            {
                Log.Debug("Moving game state from null to " + t.FullName);
            }

            IGameState oldState = this.CurrentState;
            this.currentState = nextState;
            this.CurrentState.Start(oldState);
        }

        /// <summary>
        /// Change the current game state.
        /// </summary>
        /// <typeparam name="T">The type to change the state to.</typeparam>
        public void ChangeState<T>()
        {
            this.ChangeState(typeof(T));
        }

        /// <summary>
        /// Is the specified instance of a state active.
        /// </summary>
        /// <param name="state">The instance to check.</param>
        /// <returns>Whether the specified instance of a state is active.</returns>
        public bool IsStateActive(IGameState state)
        {
            return this.CurrentState == state;
        }

        /// <summary>
        /// Is the state active.
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <returns>Whether the specified state type is active.</returns>
        public bool IsStateActive(Type t)
        {
            return this.CurrentState.GetType() == t;
        }

        /// <summary>
        /// Is the state active.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>Whether the specified state type is active.</returns>
        public bool IsStateActive<T>()
        {
            return this.IsStateActive(typeof(T));
        }

        /// <summary>
        /// Called when an input action is received.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="action">The input action.</param>
        protected void OnInputAction(object sender, string action)
        {
            if (this.CurrentState != null)
            {
                this.CurrentState.OnInputAction(action);
            }
        }
    }
}
