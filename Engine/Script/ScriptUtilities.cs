namespace Dive.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Antlr4.Runtime;
    using Dive.Script.Language;

    /// <summary>
    /// Utility functions for parsing and manipulating DScript.
    /// </summary>
    public static class ScriptUtilities
    {
        /// <summary>
        /// Parses a string into a list of ExecutableCommands.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A list of ExecutableCommands.</returns>
        public static CommandList ParseString(string input)
        {
            try
            {
                return Parse(new AntlrInputStream(input));
            }
            catch (ParseException)
            {
                return new CommandList()
                {
                    Commands = new List<ExecutableCommand>()
                };
            }
        }

        /// <summary>
        /// Parses a file into a list of ExecutableCommands.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>A list of ExecutableCommands.</returns>
        public static CommandList ParseFile(string filename)
        {
            try
            {
                return Parse(new AntlrFileStream(filename));
            }
            catch (ParseException)
            {
                return new CommandList()
                {
                    Commands = new List<ExecutableCommand>()
                };
            }
        }

        /// <summary>
        /// Parses the specified input stream.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A list of ExecutableCommands.</returns>
        public static CommandList Parse(ICharStream input)
        {
            DScriptLexer lexer = new DScriptLexer(input);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);

            DScriptParser parser = new DScriptParser(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new LogErrorListener());

            List<ExecutableCommand> commands = parser.compileUnit().finalCommands;

            if (parser.NumberOfSyntaxErrors > 0)
            {
                throw new ParseException("Parser finished with syntax errors");
            }

            return new CommandList(commands);
        }
    }
}
