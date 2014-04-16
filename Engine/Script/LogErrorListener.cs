namespace Dive.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Antlr4.Runtime;
    using log4net;

    /// <summary>
    /// Listens for Antlr errors and logs them.
    /// </summary>
    public class LogErrorListener : BaseErrorListener
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LogErrorListener));

        /// <summary>
        /// Emits a syntax error.
        /// </summary>
        /// <param name="recognizer">The recognizer.</param>
        /// <param name="offendingSymbol">The offending symbol.</param>
        /// <param name="line">The line number.</param>
        /// <param name="charPositionInLine">The character position in the line.</param>
        /// <param name="msg">The error message.</param>
        /// <param name="e">The exception.</param>
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Log.Warn(string.Format("Syntax error at line {0}:{1} - {2}", line, charPositionInLine, msg));
        }
    }
}
