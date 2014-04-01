namespace Dive.Engine.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Use on an <see cref="IGameState"/> that has the <see cref="GameState"/> attribute to make the engine start with it.
    /// <para>
    /// This attribute is semi-obsolete, and it is preferred that scripts/init.ds is used instead with the "changestate" command.
    /// </para>
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StartupGameState : System.Attribute
    {
    }
}
