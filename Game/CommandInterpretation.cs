using System;
using System.Collections.Generic;

namespace GameEngine
{
   // The CommandInterpretation class is very important. Instead of asking for a multitude of parameters and plugging them in everytime you want it, these functions do that for you.
   // Feel free to expand it as you like.
   // TODO: merge some overloads together with optional params
   static class CommandInterpretation
   {
      // Same as Console.Readline, but makes it nice for message interception, if necessary
      public static string GetUserResponse(string message)
      {
         Output.WriteLineTagged(message, Output.Tag.Prompt);
         string response = GetUserResponse();
         return response.Trim();
      }

      // Overload: Prints no message
      public static string GetUserResponse()
      {
         string response = Console.ReadLine();
         Output.WriteLineToConsole("");
         return response.Trim();
      }

      // Prompts the user for an integer, then parses it and returns true if the parse was successful.
      public static bool InterpretInt(out int result)
      {
         string response = GetUserResponse("Enter an int");
         return InterpretInt(response, out result);
      }

      // Overload: response parameter, if the string to convert already exists (no prompting)
      public static bool InterpretInt(string response, out int result)
      {
         if (int.TryParse(response, out result))
         {
            return true;
         }
         return false;
      }

      // Overload: response parameter & integer range.
      public static bool InterpretInt(string response, int minValue, int maxValue, out int result)
      {
         if (maxValue < minValue)
         {
            result = -1;
            return false;
         }
         if (int.TryParse(response, out int responseInt) && (minValue <= responseInt) && (responseInt <= maxValue))
         {
            result = responseInt;
            return true;
         }
         else
         {
            if (AskYesNo("Invalid entry. Would you like to try again?"))
            {
               return InterpretInt(minValue, maxValue, out result);
            }
         }
         result = -1;
         return false;
      }

      // Overload: just integer range.
      public static bool InterpretInt(int minValue, int maxValue, out int result)
      {
         string response = GetUserResponse("Please enter an index between " + minValue + " and " + maxValue);

         if (int.TryParse(response, out int responseInt) && (minValue <= responseInt) && (responseInt <= maxValue))
         {
            result = responseInt;
            return true;
         }
         else
         {
            if (AskYesNo("Invalid entry. Would you like to try again?"))
            {
               return InterpretInt(minValue, maxValue, out result);
            }
         }
         result = -1;
         return false;
      }
      
      // Same as InterpretInt, but with floats
      public static bool InterpretFloat(string response, out float result)
      {
         if (float.TryParse(response, out result))
         {
            return true;
         }
         return false;
      }
      
      // Uses InterpretString to evaluate if the user has said a statement equivalent to "yes" or "no".
      // When it does not come back conclusive, default is "no"
      public static bool InterpretYesNo(string response)
      {
         string[] validYes = new string[] { "yes", "ok", "okay", "sure", "y", "alright", "yeah", "yep", "true", "t" };
         string[] validNo = new string[] { "no", "cancel", "nope", "nah", "n", "false", "f" };

         if (InterpretString(response, validYes, out _))
         {
            return true;
         }
         if (InterpretString(response, validNo, out _))
         {
            return false;
         }
         Output.WriteLineTagged("Message not understood. Default response: no", Output.Tag.Error);
         return false;
      }

      // Prompts the user with a response, then feeds it to InterpretYesNo
      public static bool AskYesNo(string message)
      {
         string response = GetUserResponse(message);
         return InterpretYesNo(response);
      }

      // Evaluates if a given string fits into a group of acceptable answers and returns the string
      public static bool InterpretString(string response, string[] acceptedAnswers, out string result)
      {
         foreach (string acceptedAnswer in acceptedAnswers)
         {
            if (response.Equals(acceptedAnswer, StringComparison.OrdinalIgnoreCase))
            {
               result = acceptedAnswer;
               return true;
            }
         }
         result = null;
         return false;
      }

      // Overload: prompts the user after listing all elements of the array
      public static bool InterpretString(string[] acceptedAnswers, out string result)
      {
         result = null;
         Output.WriteLineTagged("Here are your options:", Output.Tag.List);
         for (int answerIndex = 0; answerIndex < acceptedAnswers.Length; answerIndex++)
         {
            string answer = acceptedAnswers[answerIndex];
            Output.WriteLineToConsole(answerIndex + ". " + answer);
         }
         string response = GetUserResponse("Which do you choose?");
         if (int.TryParse(response, out int intIndex))
         {
            if (intIndex >= acceptedAnswers.Length)
            {
               return false;
            }
            result = acceptedAnswers[intIndex];
            return true;
         }
         return InterpretString(response, acceptedAnswers, out result);
      }

      // As long as the string is not empty, returns true along with the result
      public static bool InterpretString(string response, out string result)
      {
         if (response.Length < 1)
         {
            result = string.Empty;
            return false;
         }
         result = response;
         return true;
      }

      // Does the same thing as InterpretString(), but accepts and returns multiple responses.
      public static bool InterpretStringMC(string[] acceptedAnswers, out string[] result)
      {
         result = null;
         List<string> results = new List<string>();
         do
         {
            Output.WriteLineTagged("Here are your options:", Output.Tag.List);
            for (int answerIndex = 0; answerIndex < acceptedAnswers.Length; answerIndex++)
            {
               string answer = acceptedAnswers[answerIndex];
               Output.WriteLineToConsole(answerIndex + ". " + answer);
            }
            string response = GetUserResponse("Which do you choose?");
            if (InterpretInt(response, 0, acceptedAnswers.Length - 1, out int intIndex))
            {
               results.Add(acceptedAnswers[intIndex]);
            }
            else
            {
               if (!InterpretString(response, acceptedAnswers, out string stringResponse))
               {
                  return false;
               }
               results.Add(stringResponse);
            }
         } while (CommandInterpretation.AskYesNo("Would you like to add any more options?"));

         result = results.ToArray();
         return true;
      }

      // As long as the string is not empty, returns the first char of the string
      public static bool InterpretChar(string response, out char result)
      {
         if (response.Length < 1)
         {
            result = ' ';
            return false;
         }
         result = response[0];
         return true;
      }
      
      // Takes north, south, east, west and converts to (0, -1), (0, 1), (1, 0), (-1, 0), respectively
      public static bool InterpretDirection(string response, out Coord result)
      {
         if (InterpretString(response, new string[] { "north", "n" }, out _))
         {
            result = new Coord(0, -1);
         }
         else if (InterpretString(response, new string[] { "south", "s" }, out _))
         {
            result = new Coord(0, 1);
         }
         else if (InterpretString(response, new string[] { "west", "w" }, out _))
         {
            result = new Coord(-1, 0);
         }
         else if (InterpretString(response, new string[] { "east", "e" }, out _))
         {
            result = new Coord(1, 0);
         }
         else
         {
            if (AskYesNo("That is not a valid cardinal direction. Would you like to try again?"))
            {
               return InterpretDirection(GetUserResponse(), out result);
            }
            result = new Coord(0, 0);
            return false;
         }
         return true;
      }

      // Very similar to InterpretDirection, but takes two strings
      public static bool InterpretAlphaNum(string x, string y, out Coord result)
      {
         if (Coord.FromAlphaNum(x, y, out result))
         {
            return true;
         }
         else
         {
            if (AskYesNo("Coordinates incorrect. Would you like to try again?"))
            {
               return InterpretAlphaNum(GetUserResponse("Enter x coordinate in alphaNum"), GetUserResponse("Enter y coordinate in alphaNum"), out result);
            }
            else
            {
               return false;
            }
         }
      }
      
      // Overload: prompts user for x and y
      public static bool InterpretAlphaNum(out Coord result)
      {
         string x = GetUserResponse("Enter x coordinate");
         string y = GetUserResponse("Enter y coordiante");
         if (Coord.FromAlphaNum(x, y, out result))
         {
            return true;
         }
         else
         {
            if (AskYesNo("Coordinates incorrect. Would you like to try again?"))
            {
               return InterpretAlphaNum(GetUserResponse("Enter x coordinate in alphaNum"), GetUserResponse("Enter y coordinate in alphaNum"), out result);
            }
            else
            {
               return false;
            }
         }
      }

      // Used in InterpretTile(). Prompts user for all aspects of Contents
      public static bool InterpretFloor(out Floor result)
      {
         Output.WriteLineToConsole("\nFloor");
         result = null;

         if (!InterpretString(GetUserResponse("Name (unique identifier)<string>:"), out string name))
         {
            return false;
         }
         if (!InterpretChar(GetUserResponse("Visual character (to represent on the grid when no contents is on the tile)<character>:"), out char visualChar))
         {
            return false;
         }

         result = new Floor(visualChar, name);

         return true;
      }

      // Used in InterpretTile() and other cases. Prompts user for all aspects of Contents
      public static bool InterpretContents(out Contents result)
      {
         Output.WriteLineToConsole("\nContents");
         result = null;

         Dictionary<string, string> preContainerParamMap = new Dictionary<string, string>()
         {
            {"Name", GetUserResponse("Name (unique identifier)<string>:")},
            {"VisualChar", GetUserResponse("Visual character (to represent on the grid)<character>:")},
            {"Transparent", GetUserResponse("Would you like this tile to be transparent? (visible and targetable through)<boolean>:")},
            {"Durability", GetUserResponse("Durability (health points)<integer>:")},
            {"Size", GetUserResponse("Size (space it takes up in containers)<integer>:")},
            {"Weight", GetUserResponse("Weight (Depending on player strength, they may or may not be able to pick this up)<float>:\nCurrent player strength is " + World.Player.Strength)},
            {"Action", string.Empty},
            {"Behavior", string.Empty},
            {"Tags", string.Empty}
         };
         Output.WriteLineTagged("Choose an action that this contents takes", Output.Tag.Prompt);
         if (!CommandInterpretation.InterpretString(UseActions.GetIdentifiers(), out string actionString))
         {
            Output.WriteLineTagged("Action was not a valid response", Output.Tag.Error);
            return false;
         }
         preContainerParamMap["Action"] = actionString;

         Output.WriteLineTagged("Choose a behavior for this contents", Output.Tag.Prompt);
         do
         {
            if (!CommandInterpretation.InterpretString(Behavior.GetIdentifiers(), out string behaviorString))
            {
               Output.WriteLineTagged("Behavior was not a valid response", Output.Tag.Error);
               return false;
            }
            preContainerParamMap["Behavior"] += behaviorString + ",";
         } while (CommandInterpretation.AskYesNo("Would you like to add any more behavior?"));
         
         string tempBehavior = preContainerParamMap["Behavior"];

         tempBehavior = tempBehavior.Substring(0, tempBehavior.Length - 1);

         #region Checking params
         if (!InterpretInt(preContainerParamMap["Durability"], out int durability))
         {
            Output.WriteLineTagged("Durability was not in integer format", Output.Tag.Error);
            return false;
         }
         if (!InterpretInt(preContainerParamMap["Size"], out int size))
         {
            Output.WriteLineTagged("Size was not in integer format", Output.Tag.Error);
            return false;
         }
         if (!InterpretFloat(preContainerParamMap["Weight"], out float weight))
         {
            Output.WriteLineTagged("Weight was not in float format", Output.Tag.Error);
            return false;
         }

         if (!UseActions.TryGetAction(preContainerParamMap["Action"], out Action<string[], Contents> action))
         {
            Output.WriteLineTagged("Action was not found", Output.Tag.Error);
            return false;
         }
         int uniqueID = Contents.UniqueID();
         if (preContainerParamMap["Action"] == "Dialogue")
         {
            string dialogue = CommandInterpretation.GetUserResponse("Please enter the line of dialogue for this contents");
            World.Dialogue.Add(uniqueID, dialogue);
         }

         if (!Behavior.TryGetBehaviors(tempBehavior.Split(","), out Action<Contents>[] behavior))
         {
            Output.WriteLineTagged("Behavior was not found", Output.Tag.Error);
            return false;
         }

         Output.WriteLineTagged("Enter as many tags as you like.", Output.Tag.Prompt);
         string response;
         List<string> tags = new List<string>();
         while (true)
         {
            response = CommandInterpretation.GetUserResponse("Enter a tag here. Type \"Done\" when you are done.");
            if (response.Equals("done", StringComparison.OrdinalIgnoreCase))
            {
               break;
            }
            tags.Add(response);
         }
         #endregion

         result = new Contents(
            preContainerParamMap["Name"],
            uniqueID,
            preContainerParamMap["VisualChar"][0],
            InterpretYesNo(preContainerParamMap["Transparent"]),
            durability,
            size,
            weight,
            action,
            behavior
         );
         result.Tags = tags.ToArray();
         bool isContainer = AskYesNo("Will this contents be able to hold anything?");
         if (isContainer)
         {
            result.Container = true;
            if (!InterpretInt(GetUserResponse("Container space (volume the container can hold)<integer>"), out int containerSpace))
            {
               return false;
            }
            result.ContainerSpace = containerSpace;
            if (AskYesNo("Would you like this contents to contain anything?"))
            {
               do
               {
                  Output.WriteLineToConsole("Defining new contents\n");
                  if (!InterpretContents(out Contents contentsToAdd))
                  {
                     return false;
                  }
                  if (!InterpretInt(GetUserResponse("How many of these would you like to add?"), out int repeat))
                  {
                     return false;
                  }
                  for (; repeat > 0; repeat--)
                  {
                     result.Contained.Add((Contents)contentsToAdd.Clone());
                  }
               } while (AskYesNo("Would you like to add anything else?"));
            }
            else
            {
               return true;
            }
         }
         else
         {
            return true;
         }

         return true;
      }
      
      // Uses InterpretFloor() and InterpretContents() and patches them together
      public static bool InterpretTile(out Tile result)
      {
         result = null;
         Floor floor;
         Contents contents;
         while (true)
         {
            if (InterpretFloor(out floor))
            {
               break;
            }
            else
            {
               if (!AskYesNo("Would you like to try again?"))
               {
                  return false;
               }
            }
         }
         
         while (true)
         {
            if (InterpretContents(out contents))
            {
               break;
            }
            else
            {
               if (!AskYesNo("Would you like to try again?"))
               {
                  return false;
               }
            }
         }

         result = new Tile(floor, contents, new Coord(0, 0));
         return true;
      }
   }
}
