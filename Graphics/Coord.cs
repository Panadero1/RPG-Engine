using System;

namespace A
{
   class Coord
   {
      public int _x;
      public int _y;

      public Coord(int x, int y)
      {
         _x = x;
         _y = y;
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
         return new Coord(_x + coordsToAdd._x, _y + coordsToAdd._y);
      }
      public Coord Subtract(Coord coordsToSubtract)
      {
         return new Coord(_x - coordsToSubtract._x, _y - coordsToSubtract._y);
      }
   }
}
