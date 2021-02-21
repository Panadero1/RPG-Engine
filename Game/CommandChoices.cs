using System;
using System.Collections.Generic;

namespace GameEngine
{
   class CommandChoices
   {
      public List<Command> _commandList;

      public CommandChoices(List<Command> commandList)
      {
         _commandList = commandList;
      }

      public bool TryFindCommand(string identifier, out Command command)
        {
            foreach (Command commandInList in _commandList)
            {
                if (commandInList._identifier.Equals(identifier, StringComparison.OrdinalIgnoreCase))
                {
                command = commandInList;
                return true;
                }
            }
            command = null;
            return false;
        }

        public string ListCommands()
        {
            string commandString = string.Empty;
            foreach (Command command in _commandList)
            {
                commandString += command._identifier + "\n";
            }
            return commandString;
        }
        public bool EvaluateCommand(string userCommand)
        {
            string[] splitCommand = userCommand.Split(' ');

            foreach (Command commandItem in _commandList)
            {
                if (splitCommand[0].Equals(commandItem._identifier, StringComparison.OrdinalIgnoreCase))
                {
                List<string> responses = new List<string>();
                int commandParameter;
                for (commandParameter = 1; commandParameter < splitCommand.Length; commandParameter++)
                {
                    responses.Add(splitCommand[commandParameter]);
                }
                for (int helpLineIndex = commandParameter; helpLineIndex < commandItem._helpLines.Length + 1; helpLineIndex++)
                {
                    string helpLine = commandItem._helpLines[helpLineIndex - 1];
                    responses.Add(CommandInterpretation.GetUserResponse(helpLine));
                }
                commandItem._customCommand(responses.ToArray());
                return commandItem._takesTime;
                }
            }
            return false;
        }
   }
}
