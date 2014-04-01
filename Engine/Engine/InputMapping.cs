namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SFML.Window;

    /// <summary>
    /// Holds data about an input mapping for keyboard, mouse, or mouse wheel.
    /// </summary>
    public class InputMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputMapping"/> class.
        /// </summary>
        public InputMapping()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputMapping"/> class.
        /// </summary>
        /// <param name="key">The keyboard key.</param>
        /// <param name="keyDown">Whether the key is down or not.</param>
        public InputMapping(Keyboard.Key key, bool keyDown)
        {
            this.Type = InputType.Keyboard;
            this.KeyDown = keyDown;
            this.Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputMapping"/> class.
        /// </summary>
        /// <param name="button">The mouse button.</param>
        /// <param name="keyDown">Whether the button is down or not.</param>
        public InputMapping(Mouse.Button button, bool keyDown)
        {
            this.Type = InputType.Mouse;
            this.KeyDown = keyDown;
            this.Button = button;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputMapping"/> class.
        /// </summary>
        /// <param name="wheelDown">Whether the mouse wheel moves down or not.</param>
        public InputMapping(bool wheelDown)
        {
            this.Type = InputType.MouseWheel;
            this.KeyDown = wheelDown;
        }

        /// <summary>
        /// Types of inputs.
        /// </summary>
        public enum InputType
        {
            /// <summary>
            /// Keyboard input type.
            /// </summary>
            Keyboard,

            /// <summary>
            /// Mouse input type.
            /// </summary>
            Mouse,

            /// <summary>
            /// Mouse wheel input type.
            /// </summary>
            MouseWheel
        }

        /// <summary>
        /// Gets or sets the <see cref="InputType"/>.
        /// </summary>
        /// <value>The <see cref="InputType"/>.</value>
        public InputType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this input mapping is for a key down or key up event.
        /// </summary>
        /// <value>True if this is a key down binding, or false if it s a key up binding.</value>
        public bool KeyDown
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the keyboard key.
        /// </summary>
        /// <value>The keyboard key for this binding.</value>
        public Keyboard.Key Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mouse button.
        /// </summary>
        /// <value>The mouse button for this binding.</value>
        public Mouse.Button Button
        {
            get;
            set;
        }

        /// <summary>
        /// Parse an input string to an <see cref="InputMapping"/>.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns>The <see cref="InputMapping"/> parsed from the string.</returns>
        public static InputMapping Parse(string str)
        {
            if (str.Length < 2)
            {
                throw new InputMappingException("Invalid mapping \"" + str + "\" (too short)");
            }

            InputMapping im = new InputMapping();
            switch (str[0])
            {
                case '+':
                    im.KeyDown = true;
                    break;

                case '-':
                    im.KeyDown = false;
                    break;

                default:
                    throw new InputMappingException("Invalid mapping \"" + str + "\" (invalid key state specifier)");
            }

            string keyName = str.Substring(1);

            Keyboard.Key key;
            if (Enum.TryParse<Keyboard.Key>(keyName, out key))
            {
                im.Key = key;
                im.Type = InputType.Keyboard;
                return im;
            }
            else
            {
                switch (keyName)
                {
                    case "mouse1":
                        im.Button = Mouse.Button.Left;
                        im.Type = InputType.Mouse;
                        break;

                    case "mouse2":
                        im.Button = Mouse.Button.Right;
                        im.Type = InputType.Mouse;
                        break;

                    case "mouse3":
                        im.Button = Mouse.Button.Middle;
                        im.Type = InputType.Mouse;
                        break;

                    case "mouse4":
                        im.Button = Mouse.Button.XButton1;
                        im.Type = InputType.Mouse;
                        break;

                    case "mouse5":
                        im.Button = Mouse.Button.XButton2;
                        im.Type = InputType.Mouse;
                        break;

                    case "wheel":
                        im.Type = InputType.MouseWheel;
                        break;

                    default:
                        throw new InputMappingException("Invalid input name");
                }

                return im;
            }
        }

        /// <summary>
        /// Compares an object against this object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the objects are equivalent, false if not.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is InputMapping))
            {
                return false;
            }

            InputMapping other = (InputMapping)obj;

            if (this.Type != other.Type || this.KeyDown != other.KeyDown)
            {
                return false;
            }

            switch (this.Type)
            {
                case InputType.Keyboard:
                    return this.Key == other.Key;

                case InputType.Mouse:
                    return this.Button == other.Button;

                case InputType.MouseWheel:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get the hash code for this object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            int code = this.Type.GetHashCode() ^ this.KeyDown.GetHashCode();

            switch (this.Type)
            {
                case InputType.Keyboard:
                    return code ^ this.Key.GetHashCode();

                case InputType.Mouse:
                    return code ^ this.Button.GetHashCode();

                case InputType.MouseWheel:
                    return code;
            }

            return code;
        }
    }
}
