namespace Dive.Script.Arguments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine;
    using Dive.Entity;
    using log4net;

    /// <summary>
    /// Argument to a command that is backed by an entity id.
    /// </summary>
    public class EntityCommandArgument : BasicCommandArgument
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(VariableCommandArgument));

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        /// <exception cref="System.ArgumentException">Unknown entity.</exception>
        public override string Value
        {
            get
            {
                Entity entity = GameEngine.Instance.EntityManager.GetEntityByName(this.RawValue);
                if (entity == null)
                {
                    throw new ArgumentException(string.Format("Unknown entity \"{0}\"", this.RawValue));
                }

                return entity.Id.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("EntityCommandArgument{{ RawValue = \"{0}\" }}", this.RawValue);
        }
    }
}
