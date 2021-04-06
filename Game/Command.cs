using System;

namespace GameEngine
{
	// A Command has a few important features for streamlining the command creation process
	class Command
	{
		// **Important** All commands to be used in the game are to be defined in GameMode Commands

		// This is a line describing what the command does
		public string HelpText;

		// This is a string representing the function. It's necessary for saving & loading. Make it unique!
		public string Identifier;

		// This array contains a string to output for each parameter that will be needed for the function. Parameter interpretation should happen within the function
		public string[] HelpLines;

		// This is a reference to a function defined in GameModeCommands
		public Action<string[]> CustomCommand;

		// Whether or not the world should update upon the completion of this command
		public bool TakesTime;

		public Command(string identifier, string helpText, string[] helpLines, Action<string[]> customCommand, bool takesTime)
		{
			Identifier = identifier;
			HelpText = helpText;
			HelpLines = helpLines;
			CustomCommand = customCommand;
			TakesTime = takesTime;
		}
	}
}
