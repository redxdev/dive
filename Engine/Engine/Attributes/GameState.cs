namespace Dive.Engine.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Attribute for automatically registering a <see cref="Dive.Engine.IGameState"/> with a state manager.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameState : System.Attribute
    {
    }
}
