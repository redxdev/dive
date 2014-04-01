namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;
    using SFML.Window;

    /// <summary>
    /// Input action event handler.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="action">Input action.</param>
    public delegate void ActionEventHandler(object sender, string action);

    /// <summary>
    /// Manages input and input bindings. Joystick bindings are not currently supported, but support might be
    /// added in the future.
    /// </summary>
    public class InputManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InputManager));

        private Dictionary<InputMapping, string> inputMappings;

        private Dictionary<string, InputMapping> actionMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public InputManager()
        {
            this.inputMappings = new Dictionary<InputMapping, string>();
            this.actionMappings = new Dictionary<string, InputMapping>();
        }

        /// <summary>
        /// Called when an input action has been received.
        /// </summary>
        public event ActionEventHandler ActionEvent;

        /// <summary>
        /// Initialize this input manager. This will read the configuration mappings.
        /// </summary>
        public void Initialize()
        {
            this.ReadConfigurationMappings();

            GameEngine.Instance.Window.KeyPressed += this.OnKeyPressed;
            GameEngine.Instance.Window.KeyReleased += this.OnKeyReleased;
            GameEngine.Instance.Window.MouseButtonPressed += this.OnMouseButtonPressed;
            GameEngine.Instance.Window.MouseButtonReleased += this.OnMouseButtonReleased;
            GameEngine.Instance.Window.MouseWheelMoved += this.OnMouseWheelMoved;
            GameEngine.Instance.Window.TextEntered += this.OnTextEntered;
        }

        /// <summary>
        /// Register an input to action mapping.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="action">The action.</param>
        public void AddMapping(InputMapping input, string action)
        {
            this.inputMappings.Add(input, action);
            this.actionMappings.Add(action, input);
        }

        /// <summary>
        /// Remove an input mapping.
        /// </summary>
        /// <param name="input">The input to remove.</param>
        public void RemoveMapping(InputMapping input)
        {
            string action = this.inputMappings[input];
            this.inputMappings.Remove(input);
            this.actionMappings.Remove(action);
        }
        
        /// <summary>
        /// Remove an action mapping.
        /// </summary>
        /// <param name="action">The action.</param>
        public void RemoveMapping(string action)
        {
            InputMapping input = this.actionMappings[action];
            this.inputMappings.Remove(input);
            this.actionMappings.Remove(action);
        }

        /// <summary>
        /// Load and read the input configuration file.
        /// </summary>
        public void ReadConfigurationMappings()
        {
            IniParser.IniData config = GameEngine.Instance.AssetManager.Load<IniParser.IniData>("config/input.ini");

            foreach (IniParser.SectionData section in config.Sections)
            {
                string action = section.SectionName;
                string map = section.Keys["map"];
                InputMapping input = null;
                try
                {
                    input = InputMapping.Parse(map);
                }
                catch (InvalidOperationException e)
                {
                    Log.Warn("Invalid input mapping (" + action + " -> " + map + ")", e);
                    continue;
                }

                this.AddMapping(input, action);
            }
        }

        /// <summary>
        /// Get the action mapped to an input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The action.</returns>
        public string GetAction(InputMapping input)
        {
            return this.inputMappings[input];
        }

        /// <summary>
        /// Get the input mapped to an action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The input.</returns>
        public InputMapping GetInput(string action)
        {
            return this.actionMappings[action];
        }

        /// <summary>
        /// Get the mouse position, relative to the render window. If you need absolute mouse position, use SFML.net functions.
        /// </summary>
        /// <returns>The mouse position.</returns>
        public Vector2i GetMousePosition()
        {
            return Mouse.GetPosition(GameEngine.Instance.Window);
        }

        /// <summary>
        /// Set the mouse position, relative to the render window. If you need absolute mouse position, use SFML.net functions.
        /// </summary>
        /// <param name="pos">The mouse position.</param>
        public void SetMousePosition(Vector2i pos)
        {
            Mouse.SetPosition(pos, GameEngine.Instance.Window);
        }

        /// <summary>
        /// Get a value indicating whether the specified action is active. Mouse wheel actions are
        /// always inactive by nature.
        /// </summary>
        /// <param name="action">The input action.</param>
        /// <returns>True if the action is active, false if not.</returns>
        public bool IsActionActive(string action)
        {
            if (GameEngine.Instance.ConsoleViewer.Enabled)
            {
                return false;
            }

            InputMapping input;
            if (!this.actionMappings.TryGetValue(action, out input))
            {
                return false;
            }

            switch (input.Type)
            {
                case InputMapping.InputType.Keyboard:
                    return Keyboard.IsKeyPressed(input.Key) && input.KeyDown;

                case InputMapping.InputType.Mouse:
                    return Mouse.IsButtonPressed(input.Button) && input.KeyDown;

                case InputMapping.InputType.MouseWheel:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Called when text is entered.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextEventArgs"/> instance containing the event data.</param>
        public void OnTextEntered(object sender, TextEventArgs e)
        {
            GameEngine.Instance.ConsoleViewer.OnInput(e);
        }

        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void OnKeyPressed(object sender, KeyEventArgs e)
        {
            GameEngine.Instance.ConsoleViewer.OnInput(e);

            if (GameEngine.Instance.ConsoleViewer.Enabled)
            {
                return;
            }

            InputMapping input = new InputMapping(e.Code, true);
            string action;
            if (!this.inputMappings.TryGetValue(input, out action))
            {
                return;
            }

            if (this.ActionEvent != null)
            {
                this.ActionEvent(this, action);
            }
        }

        /// <summary>
        /// Called when a key is released.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void OnKeyReleased(object sender, KeyEventArgs e)
        {
            if (GameEngine.Instance.ConsoleViewer.Enabled)
            {
                return;
            }

            InputMapping input = new InputMapping(e.Code, false);
            string action;
            if (!this.inputMappings.TryGetValue(input, out action))
            {
                return;
            }

            if (this.ActionEvent != null)
            {
                this.ActionEvent(this, action);
            }
        }

        /// <summary>
        /// Called when a mouse button is pressed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (GameEngine.Instance.ConsoleViewer.Enabled)
            {
                return;
            }

            InputMapping input = new InputMapping(e.Button, true);
            string action;
            if (!this.inputMappings.TryGetValue(input, out action))
            {
                return;
            }

            if (this.ActionEvent != null)
            {
                this.ActionEvent(this, action);
            }
        }

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (GameEngine.Instance.ConsoleViewer.Enabled)
            {
                return;
            }

            InputMapping input = new InputMapping(e.Button, false);
            string action;
            if (!this.inputMappings.TryGetValue(input, out action))
            {
                return;
            }

            if (this.ActionEvent != null)
            {
                this.ActionEvent(this, action);
            }
        }

        /// <summary>
        /// Called when the mouse wheel is moved.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void OnMouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            GameEngine.Instance.ConsoleViewer.OnInput(e);

            if (GameEngine.Instance.ConsoleViewer.Enabled)
            {
                return;
            }

            InputMapping input = new InputMapping(e.Delta < 0);
            string action;
            if (!this.inputMappings.TryGetValue(input, out action))
            {
                return;
            }

            if (this.ActionEvent != null)
            {
                this.ActionEvent(this, action);
            }
        }
    }
}
