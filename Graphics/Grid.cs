﻿using System;

namespace GameEngine
{
   class Grid
   {
      public Tile[,] TileGrid;

      public Grid(Tile[][] tileGrid)
      {
         if (tileGrid.Length >= 260 || tileGrid[0].Length >= 260)
         {
            Console.WriteLine("Grid is too large!");
            return;
         }
         Tile[,] createdTiles = new Tile[tileGrid[0].Length, tileGrid.Length];
         for (int y = 0; y < tileGrid.Length; y++)
         {
            for (int x = 0; x < tileGrid[0].Length; x++)
            {
               Floor oldFloor = tileGrid[y][x].Floor;
               Contents oldContents = tileGrid[y][x].Contents;
               if (oldContents == null)
               {
                  createdTiles[x, y] = new Tile(new Floor(oldFloor.VisualChar, oldFloor.Name), null, new Coord(x, y));
               }
               else
               {
                  createdTiles[x, y] = new Tile(new Floor(oldFloor.VisualChar, oldFloor.Name), new Contents(oldContents.Name, oldContents.VisualChar, oldContents.Temperature, oldContents.MeltingPoint, oldContents.Transparent, oldContents.Durability, oldContents.Size, oldContents.Weight, oldContents.Container, oldContents.ContainerSpace, oldContents.Contained, oldContents.UseAction, oldContents.Behavior), new Coord(x, y));
                  createdTiles[x, y].Contents.Coordinates = new Coord(x, y);
                  if (oldContents.Name == World.Player.Contents.Name)
                  {
                     World.Player.Contents = createdTiles[x, y].Contents;
                  }
               }
            }
         }
         TileGrid = createdTiles;
      }

      public void MoveContents(Contents startingContents, Coord changedLoc)
      {
         if (changedLoc.X == 0 && changedLoc.Y == 0)
         {
            return;
         }
         Coord endingCoord = startingContents.Coordinates.Add(changedLoc);


         // This chain of 'if's in combination with the 'else' statments DOES NOT WORK WITH DIAGONAL MOVEMENT. This is intentional as of 2/11/2021
         if (endingCoord.X < 0)
         {
            // Changing levels to the west
            Coord levelCoords = World.LoadedLevel.LevelCoord.Add(new Coord(-1, 0));
            if (levelCoords.X >= 0)
            {
               Level levelToWest = World.WorldMap.GetLevelAtCoords(levelCoords);
               if (levelToWest == null)
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
               if (levelToWest.EastEntry != null && levelToWest.Grid.GetTileAtCoords(levelToWest.EastEntry).Contents == null)
               {
                  World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates).Contents = null;
                  World.LoadedLevel = levelToWest;
                  startingContents.Coordinates = World.LoadedLevel.EastEntry;
                  World.LoadedLevel.Grid.GetTileAtCoords(World.LoadedLevel.EastEntry).Contents = startingContents;
               }
               else
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
            }
            else
            {
               Console.WriteLine("You cannot move here");
               return;
            }
         }
         else if (endingCoord.X >= World.LoadedLevel.Width)
         {
            // Changing levels to east
            Coord levelCoords = World.LoadedLevel.LevelCoord.Add(new Coord(1, 0));
            if (levelCoords.X < World.WorldMap.Width)
            {
               Level levelToEast = World.WorldMap.GetLevelAtCoords(levelCoords);
               if (levelToEast == null)
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
               if (levelToEast.WestEntry != null && levelToEast.Grid.GetTileAtCoords(levelToEast.WestEntry).Contents == null)
               {
                  World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates).Contents = null;
                  World.LoadedLevel = levelToEast;
                  startingContents.Coordinates = World.LoadedLevel.WestEntry;
                  World.LoadedLevel.Grid.GetTileAtCoords(World.LoadedLevel.WestEntry).Contents = startingContents;
               }
               else
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
            }
            else
            {
               Console.WriteLine("You cannot move here");
               return;
            }
         }
         else if (endingCoord.Y < 0)
         {
            // Changing levels to north
            Coord levelCoords = World.LoadedLevel.LevelCoord.Add(new Coord(0, -1));
            if (levelCoords.Y >= 0)
            {
               Level levelToNorth = World.WorldMap.GetLevelAtCoords(levelCoords);
               if (levelToNorth == null)
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
               if (levelToNorth.SouthEntry != null && levelToNorth.Grid.GetTileAtCoords(levelToNorth.SouthEntry).Contents == null)
               {
                  World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates).Contents = null;
                  World.LoadedLevel = levelToNorth;
                  startingContents.Coordinates = World.LoadedLevel.SouthEntry;
                  World.LoadedLevel.Grid.GetTileAtCoords(World.LoadedLevel.SouthEntry).Contents = startingContents;
               }
               else
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
            }
            else
            {
               Console.WriteLine("You cannot move here");
               return;
            }
         }
         else if (endingCoord.Y >= World.LoadedLevel.Height)
         {
            // Changing levels to south
            Coord levelCoords = World.LoadedLevel.LevelCoord.Add(new Coord(0, 1));
            if (levelCoords.Y < World.WorldMap.Height)
            {
               Level levelToSouth = World.WorldMap.GetLevelAtCoords(levelCoords);
               if (levelToSouth == null)
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
               if (levelToSouth.NorthEntry != null && levelToSouth.Grid.GetTileAtCoords(levelToSouth.NorthEntry).Contents == null)
               {
                  World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates).Contents = null;
                  World.LoadedLevel = levelToSouth;
                  startingContents.Coordinates = World.LoadedLevel.NorthEntry;
                  World.LoadedLevel.Grid.GetTileAtCoords(World.LoadedLevel.NorthEntry).Contents = startingContents;
               }
               else
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
            }
            else
            {
               Console.WriteLine("You cannot move here");
               return;
            }
         }
         else
         {
            Coord startingCoord = new Coord(startingContents.Coordinates.X, startingContents.Coordinates.Y);
            Tile endingTile = World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates.Add(changedLoc));
            if (endingTile == null)
            {
               return;
            }
            if (endingTile.Contents != null)
            {
               Console.WriteLine("Tile is occupied");
               return;
            }
            Contents removedContents = startingContents;
            Coord newCoord = startingContents.Coordinates.Add(changedLoc);
            removedContents.Coordinates = newCoord;
            endingTile.Contents = removedContents;
            if (removedContents.Name == World.Player.Contents.Name)
            {
               World.Player.Contents = removedContents;
            }
            World.LoadedLevel.Grid.TileGrid[startingCoord.X, startingCoord.Y].Contents = null;
         }

      }

      public Tile GetTileAtCoords(Coord coords, bool consolePrint = true)
      {
         if (coords.X < 0 || coords.X >= TileGrid.GetLength(0) || coords.Y < 0 || coords.Y >= TileGrid.GetLength(1))
         {
            if (consolePrint)
            {
               Console.WriteLine("Coordinates given are out of range of the grid");
            }
            return null;
         }
         return TileGrid[coords.X, coords.Y];
      }

      public void SetContentsAtCoords(Coord coords, Contents contents)
      {
         TileGrid[coords.X, coords.Y].Contents = contents;
      }

      public string GraphicString()
      {
         string returnString = "   ";
         for (int repeat = 0; repeat < TileGrid.GetLength(0); repeat++)
         {
            returnString += ((repeat % 10 == 0) ? AlphabetIndex((repeat / 10)) : ' ') + (Settings.Spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < TileGrid.GetLength(0); repeat++)
         {
            returnString += (repeat % 10) + (Settings.Spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < TileGrid.GetLength(0) + (Settings.Spaced ? TileGrid.GetLength(0) - 1 : 0); repeat++)
         {
            returnString += "-";
         }
         returnString += "\n";

         for (int y = 0; y < TileGrid.GetLength(1); y++)
         {
            returnString += ((y % 10 == 0) ? AlphabetIndex(y / 10) : ' ') + (y % 10 + "|");

            for (int x = 0; x < TileGrid.GetLength(0); x++)
            {
               TileGrid[x, y].UpdateVisual();
               returnString += TileGrid[x, y].VisualChar + (Settings.Spaced ? " " : "");
            }
            returnString += "\n";
         }
         return returnString;
      }
      private char AlphabetIndex(int index)
      {
         return (char)(65 + index);
      }

      /// <summary>
      /// Finds the first instance of a particular contents
      /// </summary>
      /// <returns></returns>
      public bool TryFindContents(Contents compareContents, out Coord coords)
      {
         for (int y = 0; y < TileGrid.GetLength(1); y++)
         {
            for (int x = 0; x < TileGrid.GetLength(0); x++)
            {
               if (TileGrid[x, y].Contents == null)
               {
                  continue;
               }
               if (TileGrid[x, y].Contents.Name == compareContents.Name)
               {
                  coords = new Coord(x, y);
                  return true;
               }
            }
         }
         coords = null;
         return false;
      }

      // <LOS>

      public bool VisibleAtLine(Coord relativeCoord)
      {
         Coord playerCoords = World.Player.GetCoords();

         double x = playerCoords.X + 0.5;
         double y = playerCoords.Y + 0.5;

         double endingX = x + relativeCoord.X;
         double endingY = y + relativeCoord.Y;

         int xSign = relativeCoord.X == 0 ? 0 : (relativeCoord.X / Math.Abs(relativeCoord.X));
         int ySign = relativeCoord.Y == 0 ? 0 : (relativeCoord.Y / Math.Abs(relativeCoord.Y));


         double? slope;
         if (relativeCoord.X != 0)
         {
            slope = (double)relativeCoord.Y / (double)relativeCoord.X;
         }
         else
         {
            for (; y != endingY; y += ySign)
            {
               Tile tileAtCoords = World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)x, (int)y), false);
               if (tileAtCoords != null && tileAtCoords.Contents != null && !tileAtCoords.Contents.Transparent)
               {
                  return false;
               }
            }
            return true;
         }
         bool xMove = Math.Abs(relativeCoord.X) > Math.Abs(relativeCoord.Y);

         if (xMove)
         {
            for (; x != endingX; x += xSign)
            {
               y = GetYTile(x - 0.1, (double)slope);

               double tryY = GetYTile(x + 0.1, (double)slope);
               Tile tileAtCoords;
               if (tryY != y)
               {
                  tileAtCoords = World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)x, (int)tryY), false);
                  if (tileAtCoords != null && tileAtCoords.Contents != null && !tileAtCoords.Contents.Transparent)
                  {
                     return false;
                  }
               }

               tileAtCoords = World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)x, (int)y), false);

               if (tileAtCoords != null && tileAtCoords.Contents != null && !tileAtCoords.Contents.Transparent)
               {
                  return false;
               }
            }
         }
         else
         {
            for (; y != endingY; y += ySign)
            {
               x = GetXTile(y - 0.1, (double)slope);

               double tryX = GetXTile(y + 0.1, (double)slope);
               Tile tileAtCoords;
               if (tryX != x)
               {
                  tileAtCoords = World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)tryX, (int)y), false);
                  if (tileAtCoords != null && tileAtCoords.Contents != null && !tileAtCoords.Contents.Transparent)
                  {
                     return false;
                  }
               }

               tileAtCoords = World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)x, (int)y), false);

               if (tileAtCoords != null && tileAtCoords.Contents != null && !tileAtCoords.Contents.Transparent)
               {
                  return false;
               }
            }
         }
         return true;
      }
      private double CenterTile(double input)
      {
         return (Math.Floor(input) + 0.5f);
      }
      private double GetYTile(double x, double slope)
      {
         Coord playerCoord = World.Player.GetCoords();
         if (slope == 0)
         {
            return CenterTile(playerCoord.Y);
         }
         return CenterTile(playerCoord.Y + slope * (x + (0.5 / slope) - playerCoord.X - 0.5));
      }
      private double GetXTile(double y, double slope)
      {
         Coord playerCoord = World.Player.GetCoords();
         return CenterTile(playerCoord.X + ((y + (slope * 0.5) - playerCoord.Y - 0.5) / slope));
      }

      // </LOS>

      public void RunBehavior()
      {
         foreach (Tile tile in TileGrid)
         {
            tile.Contents.Behavior(tile.Contents);
         }
      }
   }
}
