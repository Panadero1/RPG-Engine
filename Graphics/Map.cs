using System;

namespace GameEngine
{
   class Map
   {
      public Level[][] _levelMap;
      public string _name;
      public int _width;
      public int _height;

      public Map(Level[][] gridMap, string name)
      {
         _levelMap = gridMap;
         _name = name;

         _width = gridMap.Length;
         _height = gridMap[0].Length;
      }

      public Level GetLevelAtCoords(Coord coords)
      {
         if (coords._x < 0 || coords._x >= _levelMap.Length || coords._y < 0 || coords._y >= _levelMap[0].Length)
         {
            Console.WriteLine("Level is out of bounds of the map.");
            return null;
         }
         return _levelMap[coords._x][coords._y];
      }

      public string GraphicString()
      {
         string returnString = "   ";
         for (int repeat = 0; repeat < _levelMap.Length; repeat++)
         {
            returnString += ((repeat % 10 == 0) ? AlphabetIndex((repeat / 10)) : ' ') + (Settings._spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < _levelMap.Length; repeat++)
         {
            returnString += (repeat % 10) + (Settings._spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < _levelMap.Length + (Settings._spaced ? _levelMap.Length - 1 : 0); repeat++)
         {
            returnString += "-";
         }
         returnString += "\n";

         for (int y = 0; y < _levelMap[0].Length; y++)
         {
            returnString += ((y % 10 == 0) ? AlphabetIndex(y / 10) : ' ') + (y % 10 + "|");

            for (int x = 0; x < _levelMap.Length; x++)
            {
               Level currentLevel = _levelMap[x][y];
               if (currentLevel == null)
               {
                  returnString += " " + (Settings._spaced ? " " : "");
               }
               else
               {
                  returnString += ((currentLevel.Equals(World._loadedLevel)) ? World._player._contents._visualChar : currentLevel._visualChar) + (Settings._spaced ? " " : "");
               }
            }
            returnString += "\n";
         }
         return returnString;
      }

      private char AlphabetIndex(int index)
      {
         return (char)(65 + index);
      }

      public Map GenerateMap()
      {
         throw new NotImplementedException();
      }
   }
}
