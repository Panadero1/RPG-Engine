﻿using System;
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
         Console.WriteLine(message);
         string response = Console.ReadLine();
         return response.Trim();
      }

      // Overload: Prints no message
      public static string GetUserResponse()
      {
         string response = Console.ReadLine();
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
         if (!CommandInterpretation.InterpretString(UseActions.GetIdentifiers(), out string actionString))
         {
            Console.WriteLine("Action was not a valid response");
            return false;
         }
         preContainerParamMap["Action"] = actionString;

         Console.WriteLine("Choose a behavior for this contents");
         if (!CommandInterpretation.InterpretString(Behavior.GetIdentifiers(), out string behaviorString))
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
