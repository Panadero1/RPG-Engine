using System;

namespace GameEngine
{
   class Map
   {
      public Level[,] LevelMap;
      public string Name;

      public Map(Level[,] gridMap, string name)
      {
         LevelMap = gridMap;
         Name = name;
      }

      public bool GetLevelAtCoords(Coord coords, out Level result)
      {
         if (coords.X < 0 || coords.X >= LevelMap.GetLength(0) || coords.Y < 0 || coords.Y >= LevelMap.GetLength(1))
         {
            Console.WriteLine("Level is out of bounds of the map.");
            result = null;
            return false;
         }
         result = LevelMap[coords.X, coords.Y];
         return true;
      }

      public string GraphicString()
      {
         string returnString = "   ";
         for (int repeat = 0; repeat < LevelMap.GetLength(0); repeat++)
         {
            returnString += ((repeat % 10 == 0) ? AlphabetIndex((repeat / 10)) : ' ') + (Settings.Spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < LevelMap.GetLength(0); repeat++)
         {
            returnString += (repeat % 10) + (Settings.Spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < LevelMap.GetLength(0) + (Settings.Spaced ? LevelMap.GetLength(0) - 1 : 0); repeat++)
         {
            returnString += "-";
         }
         returnString += "\n";

         for (int y = 0; y < LevelMap.GetLength(1); y++)
         {
            returnString += ((y % 10 == 0) ? AlphabetIndex(y / 10) : ' ') + (y % 10 + "|");

            for (int x = 0; x < LevelMap.GetLength(0); x++)
            {
               Level currentLevel = LevelMap[x, y];
               if (currentLevel == null)
               {
                  returnString += " " + (Settings.Spaced ? " " : "");
               }
               else
               {
                  returnString += ((currentLevel.Equals(World.LoadedLevel)) ? World.Player.Contents.VisualChar : currentLevel.VisualChar) + (Settings.Spaced ? " " : "");
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
