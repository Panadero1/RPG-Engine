using System;

namespace GameEngine
{
   class Map
   {
      public Level[][] LevelMap;
      public string Name;
      public int Width;
      public int Height;

      public Map(Level[][] gridMap, string name)
      {
         LevelMap = gridMap;
         Name = name;

         Width = gridMap.Length;
         Height = gridMap[0].Length;
      }

      public Level GetLevelAtCoords(Coord coords)
      {
         if (coords.X < 0 || coords.X >= LevelMap.Length || coords.Y < 0 || coords.Y >= LevelMap[0].Length)
         {
            Console.WriteLine("Level is out of bounds of the map.");
            return null;
         }
         return LevelMap[coords.X][coords.Y];
      }

      public string GraphicString()
      {
         string returnString = "   ";
         for (int repeat = 0; repeat < LevelMap.Length; repeat++)
         {
            returnString += ((repeat % 10 == 0) ? AlphabetIndex((repeat / 10)) : ' ') + (Settings.Spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < LevelMap.Length; repeat++)
         {
            returnString += (repeat % 10) + (Settings.Spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < LevelMap.Length + (Settings.Spaced ? LevelMap.Length - 1 : 0); repeat++)
         {
            returnString += "-";
         }
         returnString += "\n";

         for (int y = 0; y < LevelMap[0].Length; y++)
         {
            returnString += ((y % 10 == 0) ? AlphabetIndex(y / 10) : ' ') + (y % 10 + "|");

            for (int x = 0; x < LevelMap.Length; x++)
            {
               Level currentLevel = LevelMap[x][y];
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
