using System;

namespace GameEngine
{
   class Coord
   {
      public int X;
      public int Y;

      public Coord(int x, int y)
      {
         X = x;
         Y = y;
      }
      public static bool FromAlphaNum(string alphaNumX, string alphaNumY, out Coord result)
      {
         alphaNumX = alphaNumX.ToUpper();
         alphaNumY = alphaNumY.ToUpper();
         #region tests
         if (alphaNumX.Length < 2)
         {
            Console.WriteLine("'x' was not in alphaNum format: <letter><number>");
            result = null;
            return false;
         }
         if (alphaNumY.Length < 2)
         {
            Console.WriteLine("'y' was not in alphaNum format: <letter><number>");
            result = null;
            return false;
         }
         if (!int.TryParse(alphaNumX[1].ToString(), out int x))
         {
            Console.WriteLine("'x' was not in alphaNum format: <letter><number>");
            result = null;
            return false;
         }
         if (!int.TryParse(alphaNumY[1].ToString(), out int y))
         {
            Console.WriteLine("'y' was not in alphaNum format: <letter><number>");
            result = null;
            return false;
         }

         if (int.TryParse(alphaNumX[0].ToString(), out _))
         {
            Console.WriteLine("This part of alphaNum format for 'x' should not be an integer: <letter><number>");
            result = null;
            return false;
         }
         if (int.TryParse(alphaNumY[0].ToString(), out _))
         {
            Console.WriteLine("This part of alphaNum format for 'y' should not be an integer: <letter><number>");
            result = null;
            return false;
         }

         #endregion
         
         result = new Coord(numFromAlphaNum(alphaNumX[0]) + x, numFromAlphaNum(alphaNumY[0]) + y);
         return true;
      }
      public string ToAlphaNum()
      {
         throw new NotImplementedException();
      }

      private static int numFromAlphaNum(char letter)
      {
         return (letter - 65);
      }

      public Coord Add(Coord coordsToAdd)
      {
         return new Coord(X + coordsToAdd.X, Y + coordsToAdd.Y);
      }
      public Coord Subtract(Coord coordsToSubtract)
      {
         return new Coord(X - coordsToSubtract.X, Y - coordsToSubtract.Y);
      }
   }
}
