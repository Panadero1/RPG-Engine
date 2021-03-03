using System;
using System.Collections.Generic;

namespace GameEngine
{
   static class CommandInterpretation
   {
      public static string GetUserResponse(string message)
      {
         Console.WriteLine(message);
         string response = Console.ReadLine();
         return response.Trim();
      }

      public static string GetUserResponse()
      {
         string response = Console.ReadLine();
         return response.Trim();
      }

      public static bool InterpretInt(out int result)
      {
         string response = GetUserResponse("Enter an int");
         return InterpretInt(response, out result);
      }

      public static bool InterpretInt(string response, out int result)
      {
         if (int.TryParse(response, out result))
         {
            return true;
         }
         return false;
      }

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
      
      public static bool InterpretFloat(string response, out float result)
      {
         if (float.TryParse(response, out result))
         {
            return true;
         }
         return false;
      }
      
      public static bool InterpretYesNo(string response)
      {
         string[] validYes = new string[] { "yes", "ok", "okay", "sure", "y", "alright", "yeah", "yep", "true" };
         string[] validNo = new string[] { "no", "cancel", "nope", "nah", "n", "false" };

         if (InterpretString(response, validYes, out _))
         {
            return true;
         }
         if (InterpretString(response, validNo, out _))
         {
            return false;
         }
         Console.WriteLine("Message not understood. Default response: no");
         return false;
      }

      public static bool AskYesNo(string message)
      {
         string response = GetUserResponse(message);
         return InterpretYesNo(response);
      }

      public static bool InterpretString(string response, string[] acceptedAnswers, out string result)
      {
         foreach (string acceptedAnswer in acceptedAnswers)
         {
            if (response.Equals(acceptedAnswer, StringComparison.OrdinalIgnoreCase))
            {
               result = response;
               return true;
            }
         }
         result = null;
         return false;
      }

      public static bool InterpretString(string[] acceptedAnswers, out string result)
      {
         Console.WriteLine("Here are your options:");
         for (int answerIndex = 0; answerIndex < acceptedAnswers.Length; answerIndex++)
         {
            string answer = acceptedAnswers[answerIndex];
            Console.WriteLine(answerIndex + ". " + answer);
         }
         string response = GetUserResponse("Which do you choose?");
         if (int.TryParse(response, out int intIndex))
         {
            result = acceptedAnswers[intIndex];
            return true;
         }
         return InterpretString(response, acceptedAnswers, out result);
      }

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

      public static bool InterpretFloor(out Floor result)
      {
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

      public static bool InterpretContents(out Contents result)
      {
         result = null;
         bool isContainer = AskYesNo("Will this contents be able to hold anything?");

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
         };

         Console.WriteLine("Choose an action that this contents takes");
         if (!CommandInterpretation.InterpretString(UseActions.Identifiers, out string actionString))
         {
            Console.WriteLine("Action was not a valid response");
            return false;
         }
         preContainerParamMap["Action"] = actionString;

         Console.WriteLine("Choose a behavior for this contents");
         if (!CommandInterpretation.InterpretString(Behavior.Identifiers, out string behaviorString))
         {
            Console.WriteLine("Behavior was not a valid response");
            return false;
         }
         preContainerParamMap["Behavior"] = behaviorString;


         #region Checking params
         if (!InterpretInt(preContainerParamMap["Durability"], out int durability))
         {
            Console.WriteLine("Durability was not in integer format");
            return false;
         }
         if (!InterpretInt(preContainerParamMap["Size"], out int size))
         {
            Console.WriteLine("Size was not in integer format");
            return false;
         }
         if (!InterpretFloat(preContainerParamMap["Weight"], out float weight))
         {
            Console.WriteLine("Weight was not in float format");
            return false;
         }
         if (!UseActions.TryGetAction(actionString, out Action<string[], Contents> action))
         {
            Console.WriteLine("Action was not found");
            return false;
         }
         if (actionString == "Dialogue")
         {
            if (World.Dialogue.TryGetValue(preContainerParamMap["Name"], out string dialogue))
            {
               Console.WriteLine("There is already a dialogue line for this content's name (give it a unique name to give it a unique dialogue)");
               Console.WriteLine(dialogue);
            }
            else
            {
               Console.WriteLine("There is no current dialogue for this content's name. Please define it below");
               dialogue = Console.ReadLine();
               World.Dialogue.Add(preContainerParamMap["Name"], dialogue);
            }
         }
         if (!Behavior.TryGetBehavior(behaviorString, out Action<Contents> behavior))
         {
            Console.WriteLine("Behavior was not found");
            return false;
         }
         #endregion

         result = new Contents(
            preContainerParamMap["Name"],
            preContainerParamMap["VisualChar"][0],
            InterpretYesNo(preContainerParamMap["Transparent"]),
            durability,
            size,
            weight,
            action,
            behavior
         );
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
                  Console.WriteLine("Defining new contents\n");
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
