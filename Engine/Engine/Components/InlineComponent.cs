namespace Dive.Engine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Entity;
    using Dive.Entity.Attributes;

    /// <summary>
    /// Used to create inline components with lambda expressions, in case you don't want a reusable component. Due to the way the current entity framework is
    /// designed, you can only have a single InlineComponent on an Entity at a time.
    /// </summary>
    [EntityComponent(Name = "Engine.InlineComponent", ExecutionLayer = EngineLayers.UpdateGame)]
    public class InlineComponent : AbstractComponent
    {
        private Action<InlineComponent> updateAction = null;

        private Action<InlineComponent> drawAction = null;

        private Action<InlineComponent> clearAction = null;

        private Func<InlineComponent, string, object[], bool> onEventAction = null;

        private Action<InlineComponent, string> onInputAction = null;

        /// <summary>
        /// Gets or sets the update action.
        /// </summary>
        /// <value>
        /// The update action.
        /// </value>
        public Action<InlineComponent> UpdateAction
        {
            get
            {
                return this.updateAction;
            }

            set
            {
                this.updateAction = value;
            }
        }

        /// <summary>
        /// Gets or sets the draw action.
        /// </summary>
        /// <value>
        /// The draw action.
        /// </value>
        public Action<InlineComponent> DrawAction
        {
            get
            {
                return this.drawAction;
            }

            set
            {
                this.drawAction = value;
            }
        }

        /// <summary>
        /// Gets or sets the clear action.
        /// </summary>
        /// <value>
        /// The clear action.
        /// </value>
        public Action<InlineComponent> ClearAction
        {
            get
            {
                return this.clearAction;
            }

            set
            {
                this.clearAction = value;
            }
        }

        /// <summary>
        /// Gets or sets the on event action.
        /// </summary>
        /// <value>
        /// The on event action.
        /// </value>
        public Func<InlineComponent, string, object[], bool> OnEventAction
        {
            get
            {
                return this.onEventAction;
            }

            set
            {
                this.onEventAction = value;
            }
        }

        /// <summary>
        /// Gets or sets the on input action.
        /// </summary>
        /// <value>
        /// The on input action.
        /// </value>
        public Action<InlineComponent, string> OnInputAct
        {
            get
            {
                return this.onInputAction;
            }

            set
            {
                this.onInputAction = value;
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            if (this.UpdateAction == null)
            {
                return;
            }

            this.UpdateAction.Invoke(this);
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            if (this.DrawAction == null)
            {
                return;
            }

            this.DrawAction.Invoke(this);
        }

        /// <summary>
        /// Clears this instance. Called when the parent entity is removed from the world and
        /// is clearing itself.
        /// </summary>
        public override void Clear()
        {
            if (this.ClearAction == null)
            {
                return;
            }

            this.ClearAction.Invoke(this);
        }

        /// <summary>
        /// Called when [event].
        /// </summary>
        /// <param name="name">The event name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// Event specific.
        /// </returns>
        public override bool OnEvent(string name, params object[] args)
        {
            if (this.OnEventAction == null)
            {
                return base.OnEvent(name, args);
            }

            return this.OnEventAction.Invoke(this, name, args);
        }

        /// <summary>
        /// Called when [input action].
        /// </summary>
        /// <param name="action">The action.</param>
        public override void OnInputAction(string action)
        {
            if (this.OnInputAct == null)
            {
                return;
            }

            this.OnInputAct.Invoke(this, action);
        }
    }
}
