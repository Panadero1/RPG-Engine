using System;

namespace GameEngine
{
   // Highest level data class
   class Map
   {
      // 2D array of all levels
      public Level[,] LevelMap;

      // Name of the map
      public string Name;

      public Map(Level[,] gridMap, string name)
      {
         LevelMap = gridMap;
         Name = name;
      }

      // Gets the level at the given coords
      public bool GetLevelAtCoords(Coord coords, out Level result, bool printResponse = true)
      {
         if (coords.X < 0 || coords.X >= LevelMap.GetLength(0) || coords.Y < 0 || coords.Y >= LevelMap.GetLength(1))
         {
            if (printResponse)
            {
               Output.WriteLineTagged("Level is out of bounds of the map.", Output.tag.Error);
            }
            result = null;
            return false;
         }
         result = LevelMap[coords.X, coords.Y];
         return true;
      }

      // String to return that represents LevelMap
      public string GraphicString()
      {
         string returnString = "   ";
         for (int repeat = 0; repeat < LevelMap.GetLength(0); repeat++)
         {
            returnString += ((repeat % 10 == 0) ? AlphabetIndex((repeat / 10)) : ' ') + Settings.Spacing;
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < LevelMap.GetLength(0); repeat++)
         {
            returnString += (repeat % 10) + Settings.Spacing;
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < (LevelMap.GetLength(0) * (Settings.Spacing.Length + 1)) - Settings.Spacing.Length; repeat++)
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
                  returnString += " " + Settings.Spacing;
               }
               else
               {
                  returnString += ((currentLevel.Equals(World.LoadedLevel)) ? World.Player.Contents.VisualChar : currentLevel.VisualChar) + Settings.Spacing;
               }
            }
            returnString += "\n";
         }
         return returnString;
      }

      // Child function of GraphicString()
      private char AlphabetIndex(int index)
      {
         return (char)(65 + index);
      }

      // Not implemented yet
      public Map GenerateMap()
      {
         throw new NotImplementedException();
      }
   }
}
