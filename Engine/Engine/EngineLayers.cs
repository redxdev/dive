namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Holds consants for engine layer ids.
    /// </summary>
    public static class EngineLayers
    {
        // Drawing layers

        /// <summary>
        /// The layer for background drawing.
        /// </summary>
        public const int DrawBackground = 0;

        /// <summary>
        /// The layer for game drawing.
        /// </summary>
        public const int DrawGame = 5;

        /// <summary>
        /// The layer for gui drawing.
        /// </summary>
        public const int DrawGui = 10;

        // Update layers

        /// <summary>
        /// The layer for pre-update debug.
        /// </summary>
        public const int UpdatePreDebug = 0;

        /// <summary>
        /// The layer for input updates.
        /// </summary>
        public const int UpdateInput = 5;

        /// <summary>
        /// The layer for physics updates (changes in the physics
        /// engine won't take place until the next update).
        /// Transforms will be overwritten by the physics system.
        /// </summary>
        public const int UpdatePhysics = 10;

        /// <summary>
        /// The layer for debug updates.
        /// </summary>
        public const int UpdateDebug = 15;

        /// <summary>
        /// The layer for game logic updates.
        /// </summary>
        public const int UpdateGame = 20;

        /// <summary>
        /// The layer for updating physics positions.
        /// Transforms will be copied into the physics engine.
        /// </summary>
        public const int UpdatePhysicsPositions = 25;

        /// <summary>
        /// The layer for final update logic.
        /// </summary>
        public const int UpdateFinal = 30;

        /// <summary>
        /// The layer for post-update debug.
        /// </summary>
        public const int UpdatePostDebug = 35;
    }
}
