using System;

namespace GameEngine
{
   class Command
   {
      public string _helpText;

      public string _identifier;

      public string[] _helpLines;
      public Action<string[]> _customCommand;

      public bool _takesTime;

      public Command(string identifier, string helpText, string[] helpLines, Action<string[]> customCommand, bool takesTime)
      {
         _identifier = identifier;
         _helpText = helpText;
         _helpLines = helpLines;
         _customCommand = customCommand;
         _takesTime = takesTime;
      }
   }
}
