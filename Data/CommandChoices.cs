using System;
using System.Collections.Generic;

namespace GameEngine
{
	// The CommandChoices class only consists of a List<Command>, but its use is in the ways it manipulates this List<Command>
	// A new CommandChoices must be defined in GameModeCommands for every new game mode (at the bottom of the file)
	class CommandChoices
	{
		// The list of commands..
		public List<Command> CommandList;

		public CommandChoices(List<Command> commandList)
		{
			CommandList = commandList;
		}

		// This is used to get a command, given a string
		public bool TryFindCommand(string identifier, out Command command)
		{
			foreach (Command commandInList in CommandList)
			{
				if (commandInList.Identifier.Equals(identifier, StringComparison.OrdinalIgnoreCase))
				{
					command = commandInList;
					return true;
				}
			}
			command = null;
			return false;
		}

		// Returns a list of all the Command.Identifiers in CommandList
		public string ListCommands()
		{
			string commandString = string.Empty;
			foreach (Command command in CommandList)
			{
				commandString += command.Identifier + "\n";
			}
			return commandString;
		}

		// This takes in a string as input and evaluates whether it matches any commands in CommandList.
		// If it does, it runs that command, passing in any parameters that were also entered
		public bool EvaluateCommand(string userCommand)
		{
			string[] splitCommand = userCommand.Split(' ');

			foreach (Command commandItem in CommandList)
			{
				if (splitCommand[0].Equals(commandItem.Identifier, StringComparison.OrdinalIgnoreCase))
				{
					List<string> responses = new List<string>();
					int commandParameter;
					for (commandParameter = 1; commandParameter < splitCommand.Length; commandParameter++)
					{
						responses.Add(splitCommand[commandParameter]);
					}
					for (int helpLineIndex = commandParameter; helpLineIndex < commandItem.HelpLines.Length + 1; helpLineIndex++)
					{
						string helpLine = commandItem.HelpLines[helpLineIndex - 1];
						Output.WriteLineToConsole(helpLine);
						responses.Add(CommandInterpretation.GetUserResponse());
					}
					commandItem.CustomCommand(responses.ToArray());
					return commandItem.TakesTime;
				}
			}
			return false;
		}
	}
}