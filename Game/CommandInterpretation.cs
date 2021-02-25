using System;

namespace GameEngine
{
   static class CommandInterpretation
   {
      public static string GetUserResponse(string message)
      {
         Console.WriteLine(message);
         string response = Console.ReadLine();
         if (response == "exit")
         {
            Game.Execute = false;
            return string.Empty;
         }
         else
         {
            return response;
         }
      }

      public static string GetUserResponse()
      {
         string response = Console.ReadLine();
         if (response == "exit")
         {
            Game.Execute = false;
            return string.Empty;
         }
         else
         {
            return response;
         }
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
            if (InterpretYesNo("Invalid entry. Would you like to try again?"))
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
            if (InterpretYesNo("Invalid entry. Would you like to try again?"))
            {
               return InterpretInt(minValue, maxValue, out result);
            }
         }
         result = -1;
         return false;
      }
      
      public static bool InterpretYesNo(string message, bool fromHere = false)
      {

         string[] validYes = new string[] { "yes", "ok", "okay", "sure", "y", "alright", "yeah", "yep" };
         string[] validNo = new string[] { "no", "cancel", "nope", "nah", "n" };

         string response = GetUserResponse(message);
         if (InterpretString(response, validYes, out _))
         {
            return true;
         }
         if (InterpretString(response, validNo, out _))
         {
            return false;
         }
         if (!fromHere && InterpretYesNo("Message not recognized. Would you like to try again?", true))
         {
            return InterpretYesNo(message);
         }
         return false;
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
            if (InterpretYesNo("That is not a valid cardinal direction. Would you like to try again?"))
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
            if (InterpretYesNo("Coordinates incorrect. Would you like to try again?"))
            {
               return InterpretAlphaNum(GetUserResponse("Enter x coordinate in alphaNum"), GetUserResponse("Enter y coordinate in alphaNum"), out result);
            }
            else
            {
               return false;
            }
         }
      }
   }
}
