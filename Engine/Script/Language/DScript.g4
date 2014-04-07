grammar DScript;

@parser::header
{
#pragma warning disable 3021

using Dive.Script.Arguments;
}

@parser::members
{
	protected const int EOF = Eof;
}

@lexer::header
{
#pragma warning disable 3021
}

@lexer::members
{
	protected const int EOF = Eof;
	protected const int HIDDEN = Hidden;
}

/*
 * Parser Rules
 */

compileUnit returns [List<ExecutableCommand> finalCommands]
	:
		(
			cmds=commands
			{
				$finalCommands = $cmds.cmds;
			}
		)?
		EOF
	;

commands returns [List<ExecutableCommand> cmds]
	:
		{
			$cmds = new List<ExecutableCommand>();
		}
		firstCmd=command
		{
			$cmds.Add($firstCmd.cmd);
		}
		(
			cmd=command
			{
				$cmds.Add($cmd.cmd);
			}
		)*
	;

command returns [ExecutableCommand cmd]
	:	
		{
			$cmd = new ExecutableCommand();
		}
		name=IDENT
		(
			(
				arg=argument
				{
					$cmd.Arguments = new List<ICommandArgument>();
					$cmd.Arguments.Add($arg.arg);
				}
			)
		|	(
				args=arguments
				{
					$cmd.Arguments = $args.argList;
				}
			)
		)?
		{
			$cmd.Name = $name.text;
			if($cmd.Arguments == null)
				$cmd.Arguments = new List<ICommandArgument>();
		}
	;

arguments returns [List<ICommandArgument> argList]
	:	{
			$argList = new List<ICommandArgument>();
		}

		GROUP_START

		(
			firstArg=argument
			{
				$argList.Add($firstArg.arg);
			}

			(
				ARGUMENT_SEPARATOR
				arg=argument
				{
					$argList.Add($arg.arg);
				}
			)*?
		)?

		GROUP_END
	;

argument returns [ICommandArgument arg]
	:	str=IDENT
		{
			$arg = new BasicCommandArgument() { RawValue = $str.text };
		}
	|	str=STRING
		{
			$arg = new BasicCommandArgument() { RawValue = $str.text };
		}
	|	str=STRING_EXT
		{
			$arg = new BasicCommandArgument() { RawValue = $str.text };
		}
	|	num=NUMBER
		{
			$arg = new BasicCommandArgument() { RawValue = $num.text };
		}
	|	VAR_SPEC var=IDENT
		{
			$arg = new VariableCommandArgument() { RawValue = $var.text };
		}
	|	ENTITY_SPEC ent=IDENT
		{
			$arg = new EntityCommandArgument() { RawValue = $ent.text };
		}
	;

/*
 * Lexer Rules
 */

fragment ESCAPE_SEQUENCE
	:	'\\'
		(
			'\\'
		|	'"'
		)
	;

STRING
	:	'"' (( ESCAPE_SEQUENCE | . )*?) '"'
		{
			Text = Text.Substring(1, Text.Length - 2)
				.Replace("\\\\", "\\")
				.Replace("\\\"", "\"");
		}
	;

STRING_EXT
	:	'[[' .*? ']]'
		{
			Text = Text.Substring(2, Text.Length - 4);
		}
	;

NUMBER
	: '-'? [0-9] '.' [0-9]+
	| '-'? '.' [0-9]+
	| '-'? [0-9]+
	;

ARGUMENT_SEPARATOR
	:	','
	;

GROUP_START
	:	'('
	;

GROUP_END
	:	')'
	;

VAR_SPEC
	:	'$'
	;

ENTITY_SPEC
	:	'@'
	;

IDENT
	:	[a-zA-Z] ([0-9a-zA-Z] | '.' | '-' | '_' | '/')*
	;

WS
	:	[ \n\t\r] -> channel(HIDDEN)
	;

COMMENT
	:	'#' ~[\r\n]* -> channel(HIDDEN)
	;