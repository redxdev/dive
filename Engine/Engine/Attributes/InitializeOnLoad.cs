namespace Dive.Engine.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Calls a method when the assembly is loaded. This is called at the end of GameEngine.ImportAssembly().
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class InitializeOnLoad : System.Attribute
    {
    }
}
