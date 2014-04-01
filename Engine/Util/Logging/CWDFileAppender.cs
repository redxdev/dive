namespace Dive.Util.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;
    using log4net.Appender;

    /// <summary>
    /// Extends the standard log4net FileAppender to write logging files based on the current working directory instead of the
    /// application's directory.
    /// </summary>
    public class CWDFileAppender : FileAppender
    {
        /// <summary>
        /// Sets the log file path.
        /// </summary>
        /// <value>The log file path.</value>
        public override string File
        {
            set
            {
                base.File = Path.Combine(Directory.GetCurrentDirectory(), value);
            }
        }
    }
}
