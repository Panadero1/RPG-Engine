using System;
using System.Collections.Generic;

namespace GameEngine
{
   class CommandChoices
   {
      public List<Command> CommandList;

      public CommandChoices(List<Command> commandList)
      {
         CommandList = commandList;
      }

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

        public string ListCommands()
        {
            string commandString = string.Empty;
            foreach (Command command in CommandList)
            {
                commandString += command.Identifier + "\n";
            }
            return commandString;
        }
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
                    responses.Add(CommandInterpretation.GetUserResponse(helpLine));
                }
                commandItem.CustomCommand(responses.ToArray());
                return commandItem.TakesTime;
                }
            }
            return false;
        }
   }
}