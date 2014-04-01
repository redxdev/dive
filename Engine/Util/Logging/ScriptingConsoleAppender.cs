namespace Dive.Util.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine;
    using log4net.Appender;
    using log4net.Core;
    using SFML.Graphics;

    /// <summary>
    /// Log appender for the in-game console.
    /// </summary>
    public class ScriptingConsoleAppender : AppenderSkeleton
    {
        private Color debugColor = new Color(0, 200, 0);
        private Color infoColor = Color.White;
        private Color warnColor = Color.Yellow;
        private Color errorColor = Color.Red;
        private Color fatalColor = Color.Red;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingConsoleAppender"/> class.
        /// </summary>
        public ScriptingConsoleAppender()
        {
            this.ConsoleViewer = null;
        }

        /// <summary>
        /// Gets or sets the console viewer.
        /// </summary>
        /// <value>
        /// The console viewer.
        /// </value>
        public ConsoleViewer ConsoleViewer
        {
            get;
            set;
        }

        /// <summary>
        /// Appends the specified logging event.
        /// </summary>
        /// <param name="loggingEvent">The logging event.</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (this.ConsoleViewer != null)
            {
                Color color = this.infoColor;
                switch (loggingEvent.Level.Name)
                {
                    default:
                        color = Color.Black;
                        break;

                    case "DEBUG":
                        color = this.debugColor;
                        break;

                    case "INFO":
                        color = this.infoColor;
                        break;

                    case "WARN":
                        color = this.warnColor;
                        break;

                    case "ERROR":
                        color = this.errorColor;
                        break;

                    case "FATAL":
                        color = this.fatalColor;
                        break;
                }

                string[] message = loggingEvent.RenderedMessage.Split('\n');
                foreach (string line in message)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        this.ConsoleViewer.Print(line, color);
                    }
                }

                message = loggingEvent.GetExceptionString().Split('\n');
                foreach (string line in message)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        this.ConsoleViewer.Print(line, color);
                    }
                }
            }
        }
    }
}
