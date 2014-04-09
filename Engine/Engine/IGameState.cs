namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface for all game states managed by a <see cref="GameStateManager"/>.
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IGameState"/> is active.
        /// </summary>
        /// <value>The value indicating whether this <see cref="IGameState"/> is active.</value>
        bool IsActive
        {
            get;
        }

        /// <summary>
        /// Called when the <see cref="IGameState" /> is first added to the <see cref="GameStateManager" />.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called when this state is entered.
        /// </summary>
        /// <param name="previous">The previous state, or null if there is no previous state.</param>
        void Start(IGameState previous);

        /// <summary>
        /// Called when this state is being left.
        /// </summary>
        /// <param name="next">The next state, or null if there is no next state.</param>
        void Stop(IGameState next);

        /// <summary>
        /// Called when the <see cref="GameStateManager" /> is shut down.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Called when this game state is active and Update is called.
        /// </summary>
        void Update();

        /// <summary>
        /// Called when this game state is active and Draw is called.
        /// </summary>
        void Draw();

        /// <summary>
        /// Called when this game state is active and input is detected.
        /// </summary>
        /// <param name="action">The input action.</param>
        void OnInputAction(string action);
    }
}
