using System;

namespace GameEngine
{
   class Command
   {
      public string HelpText;

      public string Identifier;

      public string[] HelpLines;
      public Action<string[]> CustomCommand;

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
