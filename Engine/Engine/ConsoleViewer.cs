#pragma warning disable 0429

namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Script;
    using Dive.Util;
    using SFML.Graphics;
    using SFML.Window;

    /// <summary>
    /// Handles drawing, input, and buffering for the in-game console.
    /// </summary>
    public class ConsoleViewer
    {
        /// <summary>
        /// The rate that the pipe blinks while editing text.
        /// </summary>
        public const double BlinkRate = 0.6;

        /// <summary>
        /// The maximum size for the console buffer.
        /// </summary>
        public const int MaxConsoleBufferSize = -1;

        private Font consoleFont = null;

        private LinkedList<ConsoleMessage> consoleBuffer = new LinkedList<ConsoleMessage>();
        private int bufferPos = 0;

        private LinkedList<string> inputBuffer = new LinkedList<string>();
        private LinkedListNode<string> lastInput = null;

        private int currentPos = 0;
        private string currentInput = string.Empty;

        private double nextBlink = BlinkRate;
        private bool blinkOn = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleViewer" /> class.
        /// </summary>
        public ConsoleViewer()
        {
            this.consoleFont = GameEngine.Instance.AssetManager.Load<Font>("content/fonts/DroidSansMono.ttf");
            this.Enabled = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the console is currently open/enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Handles input for the console.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        public void OnInput(KeyEventArgs e)
        {
            if (!this.Enabled)
            {
                if (e.Code == Keyboard.Key.Tilde)
                {
                    this.Enabled = true;
                    return;
                }
                else
                {
                    return;
                }
            }

            switch (e.Code)
            {
                case Keyboard.Key.Return:
                    this.SubmitInput();
                    break;

                case Keyboard.Key.Tilde:
                    this.Enabled = false;
                    break;

                case Keyboard.Key.Back:
                    if (this.currentInput.Length > 0 && this.currentPos > 0)
                    {
                        this.currentInput = this.currentInput.Remove(this.currentPos - 1, 1);
                        this.currentPos--;
                    }

                    break;

                case Keyboard.Key.Left:
                    this.currentPos--;
                    break;

                case Keyboard.Key.Right:
                    this.currentPos++;
                    break;

                case Keyboard.Key.End:
                    this.currentPos = this.currentInput.Length;
                    break;

                case Keyboard.Key.Home:
                    this.currentPos = 0;
                    break;

                case Keyboard.Key.Up:
                    if (this.lastInput == null)
                    {
                        if (this.inputBuffer.Count == 0)
                        {
                            break;
                        }

                        this.lastInput = this.inputBuffer.First;
                    }
                    else
                    {
                        if (this.lastInput.Next == null)
                        {
                            break;
                        }

                        this.lastInput = this.lastInput.Next;
                    }

                    if (this.currentPos == this.currentInput.Length)
                    {
                        this.currentPos = this.lastInput.Value.Length;
                    }
                    else
                    {
                        this.currentPos = MathHelper.Clamp(this.currentPos, 0, this.lastInput.Value.Length);
                    }

                    this.currentInput = this.lastInput.Value;
                    break;

                case Keyboard.Key.Down:
                    if (this.lastInput == null)
                    {
                        break;
                    }
                    else
                    {
                        this.lastInput = this.lastInput.Previous;
                        if (this.lastInput == null)
                        {
                            this.currentInput = string.Empty;
                            break;
                        }
                    }

                    if (this.currentPos == this.currentInput.Length)
                    {
                        this.currentPos = this.lastInput.Value.Length;
                    }
                    else
                    {
                        this.currentPos = MathHelper.Clamp(this.currentPos, 0, this.lastInput.Value.Length);
                    }

                    this.currentInput = this.lastInput.Value;
                    break;

                case Keyboard.Key.Delete:
                    if (this.currentInput.Length > this.currentPos)
                    {
                        this.currentInput = this.currentInput.Remove(this.currentPos, 1);
                    }

                    break;

                case Keyboard.Key.PageUp:
                    this.bufferPos++;
                    if (this.bufferPos > this.consoleBuffer.Count)
                    {
                        this.bufferPos = this.consoleBuffer.Count;
                    }

                    break;

                case Keyboard.Key.PageDown:
                    this.bufferPos--;
                    if (this.bufferPos < 0)
                    {
                        this.bufferPos = 0;
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles text entry for the console.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/> instance containing the event data.</param>
        public void OnInput(TextEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (char.IsControl(e.Unicode[0]))
            {
                return;
            }

            if (e.Unicode.Equals("`"))
            {
                return;
            }

            this.currentInput = this.currentInput.Insert(this.currentPos, e.Unicode);
            this.currentPos += e.Unicode.Length;
        }

        /// <summary>
        /// Handles mouse wheel movement for the console.
        /// </summary>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        public void OnInput(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                this.bufferPos++;
            }
            else if (e.Delta < 0)
            {
                this.bufferPos--;
            }

            if (this.bufferPos < 0)
            {
                this.bufferPos = 0;
            }
            else if (this.bufferPos > this.consoleBuffer.Count)
            {
                this.bufferPos = this.consoleBuffer.Count;
            }
        }

        /// <summary>
        /// Prints the to the console buffer.
        /// </summary>
        /// <param name="input">The input.</param>
        public void Print(string input)
        {
            this.Print(input, Color.White);
        }

        /// <summary>
        /// Prints to the console buffer. This will split lines before printing.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="color">The color.</param>
        public void Print(string input, Color color)
        {
            if (GameEngine.Instance.Window == null)
            {
                return;
            }

            int maxLineLength = (int)(GameEngine.Instance.Window.Size.X / 8.1f);

            if (input.Length > maxLineLength)
            {
                this.AddToBuffer(input.Substring(0, maxLineLength), color);
                this.Print(input.Substring(maxLineLength), color);
            }
            else
            {
                this.AddToBuffer(input, color);
            }
        }

        /// <summary>
        /// Adds directly to the console buffer. This will not split lines before printing.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="color">The color.</param>
        public void AddToBuffer(string input, Color color)
        {
            this.consoleBuffer.AddFirst(new ConsoleMessage() { Message = input, PrintColor = color });
            if (MaxConsoleBufferSize > -1 && this.consoleBuffer.Count > MaxConsoleBufferSize)
            {
                this.consoleBuffer.RemoveLast();
            }

            this.bufferPos = 0;
        }

        /// <summary>
        /// Clears the console buffer.
        /// </summary>
        public void Clear()
        {
            this.consoleBuffer.Clear();
        }

        /// <summary>
        /// Submits and executes the current input.
        /// </summary>
        public void SubmitInput()
        {
            if (string.Empty.Equals(this.currentInput))
            {
                return;
            }

            this.lastInput = null;
            ConsoleManager.ConsoleLog.Info("> " + this.currentInput);
            string input = this.currentInput;
            this.currentInput = string.Empty;

            if (this.inputBuffer.First == null || !this.inputBuffer.First.Value.Equals(input))
            {
                this.inputBuffer.AddFirst(input);
            }

            try
            {
                CommandList commands = ScriptUtilities.ParseString(input);
                GameEngine.Instance.Console.Execute(commands);
            }
            catch (Exception e)
            {
                ConsoleManager.ConsoleLog.Warn("Unable to execute command", e);
            }
        }

        /// <summary>
        /// Draws the console.
        /// </summary>
        public void Draw()
        {
            this.nextBlink -= GameEngine.Instance.Delta;
            if (this.nextBlink <= 0)
            {
                this.blinkOn = !this.blinkOn;
                this.nextBlink += BlinkRate;
            }

            if (!this.Enabled)
            {
                return;
            }

            if (this.currentPos > this.currentInput.Length)
            {
                this.currentPos = this.currentInput.Length;
            }
            else if (this.currentPos < 0)
            {
                this.currentPos = 0;
            }

            RectangleShape background = new RectangleShape(new Vector2f(GameEngine.Instance.Window.Size.X, (GameEngine.Instance.Window.Size.Y / 3) + 20));
            background.OutlineColor = new Color(200, 200, 200, 150);
            background.FillColor = new Color(70, 70, 70, 150);
            background.Position = GameEngine.Instance.Window.MapPixelToCoords(new Vector2i(0, (int)((GameEngine.Instance.Window.Size.Y / 3) * 2) - 20));
            background.OutlineThickness = 3f;
            GameEngine.Instance.Window.Draw(background);

            Text inputLine = new Text("> " + this.currentInput, this.consoleFont, 14);
            inputLine.Color = Color.White;
            inputLine.Position = GameEngine.Instance.Window.MapPixelToCoords(new Vector2i(10, (int)GameEngine.Instance.Window.Size.Y - 24));
            GameEngine.Instance.Window.Draw(inputLine);

            LinkedListNode<ConsoleMessage> node = this.consoleBuffer.First;
            for (int i = 0; i < this.bufferPos; i++)
            {
                if (node == null)
                {
                    break;
                }

                node = node.Next;
            }

            int lineCount = 1;
            int maxLineCount = (int)(GameEngine.Instance.Window.Size.Y / 3) / 14;
            while (node != null && lineCount <= maxLineCount)
            {
                Text outputLine = new Text(node.Value.Message, this.consoleFont, 14);
                outputLine.Color = node.Value.PrintColor;
                outputLine.Position = GameEngine.Instance.Window.MapPixelToCoords(new Vector2i(10, (int)(GameEngine.Instance.Window.Size.Y - 24) - (lineCount * 14)));
                GameEngine.Instance.Window.Draw(outputLine);

                node = node.Next;
                lineCount++;
            }

            if (this.blinkOn)
            {
                Text pipeText = new Text(new string(' ', this.currentPos + 1) + '|', this.consoleFont, 14);
                pipeText.Color = Color.White;
                pipeText.Position = GameEngine.Instance.Window.MapPixelToCoords(new Vector2i(14, (int)GameEngine.Instance.Window.Size.Y - 24));
                GameEngine.Instance.Window.Draw(pipeText);
            }
        }

        private class ConsoleMessage
        {
            public Color PrintColor
            {
                get;
                set;
            }

            public string Message
            {
                get;
                set;
            }
        }
    }
}
