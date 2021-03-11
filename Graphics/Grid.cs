using System;

namespace GameEngine
{
   // Contains a 2D array of Tiles and various functions to manipulate it
   class Grid
   {
      // The 2d array of the grid of Tiles for the current level
      public Tile[,] TileGrid;

      public Grid(Tile[,] tileGrid)
      {
         if (tileGrid.GetLength(0) >= 260 || tileGrid.GetLength(1) >= 260)
         {
            Output.WriteLineTagged("Grid is too large!", Output.Tag.Error);
            return;
         }
         Tile[,] createdTiles = new Tile[tileGrid.GetLength(0), tileGrid.GetLength(1)];
         for (int y = 0; y < tileGrid.GetLength(1); y++)
         {
            for (int x = 0; x < tileGrid.GetLength(0); x++)
            {
               Floor oldFloor = tileGrid[x, y].Floor;
               Contents oldContents = tileGrid[x, y].Contents;
               if (oldContents == null)
               {
                  createdTiles[x, y] = new Tile(new Floor(oldFloor.VisualChar, oldFloor.Name), null, new Coord(x, y));
               }
               else
               {
                  createdTiles[x, y] = new Tile(new Floor(oldFloor.VisualChar, oldFloor.Name), new Contents(oldContents.Name, oldContents.VisualChar, oldContents.Transparent, oldContents.Durability, oldContents.Size, oldContents.Weight, oldContents.Container, oldContents.ContainerSpace, oldContents.Contained, oldContents.UseAction, oldContents.Behaviors), new Coord(x, y));
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
      public Grid(Tile[][] tileGrid)
      {
         if (tileGrid.Length >= 260 || tileGrid[0].Length >= 260)
         {
            Output.WriteLineTagged("Grid is too large!", Output.Tag.Error);
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
                  createdTiles[x, y] = new Tile(new Floor(oldFloor.VisualChar, oldFloor.Name), new Contents(oldContents.Name, oldContents.VisualChar, oldContents.Transparent, oldContents.Durability, oldContents.Size, oldContents.Weight, oldContents.Container, oldContents.ContainerSpace, oldContents.Contained, oldContents.UseAction, oldContents.Behaviors), new Coord(x, y));
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

      // Moves one contents to another tile
      public void MoveContents(Contents startingContents, Coord changedLoc, bool output = true)
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
               Level levelToWest;
               if (!World.WorldMap.GetLevelAtCoords(levelCoords, out levelToWest))
               {
                  return;
               }
               if (levelToWest == null)
               {
                  if (output)
                  {
                     Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
                  }
                  return;
               }
               if (levelToWest.EastEntry != null && levelToWest.Grid.GetTileAtCoords(levelToWest.EastEntry, out Tile WestEntryTile))
               {
                  if (WestEntryTile.Contents != null)
                  {
                     if (output)
                     {
                        Output.WriteLineTagged("Something is blocking this on the other side", Output.Tag.World);
                     }
                     return;
                  }
                  if (World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates, out Tile playerTile))
                  {
                     playerTile.Contents = null;
                  }
                  World.LoadedLevel = levelToWest;
                  startingContents.Coordinates = World.LoadedLevel.EastEntry;
                  WestEntryTile.Contents = startingContents;
               }
               else
               {
                  if (output)
                  {
                     Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
                  }
                  return;
               }
            }
            else
            {
               if (output)
               {
                  Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
               }
               return;
            }
         }
         else if (endingCoord.X >= World.LoadedLevel.Grid.TileGrid.GetLength(0))
         {
            // Changing levels to east
            Coord levelCoords = World.LoadedLevel.LevelCoord.Add(new Coord(1, 0));
            if (levelCoords.X < World.WorldMap.LevelMap.GetLength(0))
            {
               Level levelToEast;
               if (!World.WorldMap.GetLevelAtCoords(levelCoords, out levelToEast))
               {
                  return;
               }
               if (levelToEast == null)
               {
                  if (output)
                  {
                     Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
                  }
                  return;
               }
               if (levelToEast.WestEntry != null && levelToEast.Grid.GetTileAtCoords(levelToEast.WestEntry, out Tile eastEntryTile))
               {
                  if (eastEntryTile.Contents != null)
                  {
                     if (output)
                     {
                        Output.WriteLineTagged("Something is blocking this on the other side", Output.Tag.World);
                     }
                     return;
                  }
                  if (World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates, out Tile playerTile))
                  {
                     playerTile.Contents = null;
                  }
                  World.LoadedLevel = levelToEast;
                  startingContents.Coordinates = World.LoadedLevel.WestEntry;
                  eastEntryTile.Contents = startingContents;
               }
               else
               {
                  if (output)
                  {
                     Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
                  }
                  return;
               }
            }
            else
            {
               if (output)
               {
                  Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
               }
               return;
            }
         }
         else if (endingCoord.Y < 0)
         {
            // Changing levels to north
            Coord levelCoords = World.LoadedLevel.LevelCoord.Add(new Coord(0, -1));
            if (levelCoords.Y >= 0)
            {
               Level levelToNorth;
               if (!World.WorldMap.GetLevelAtCoords(levelCoords, out levelToNorth))
               {
                  return;
               }
               if (levelToNorth == null)
               {
                  if (output)
                  {
                     Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
                  }
                  return;
               }
               if (levelToNorth.SouthEntry != null && levelToNorth.Grid.GetTileAtCoords(levelToNorth.SouthEntry, out Tile northEntryTile))
               {
                  if (northEntryTile.Contents != null)
                  {
                     if (output)
                     {
                        Output.WriteLineTagged("Something is blocking this on the other side", Output.Tag.World);
                     }
                     return;
                  }
                  if (World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates, out Tile playerTile))
                  {
                     playerTile.Contents = null;
                  }
                  World.LoadedLevel = levelToNorth;
                  startingContents.Coordinates = World.LoadedLevel.SouthEntry;
                  northEntryTile.Contents = startingContents;
               }
               else
               {
                  if (output)
                  {
                     Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
                  }
                  return;
               }
            }
            else
            {
               if (output)
               {
                  Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
               }
               return;
            }
         }
         else if (endingCoord.Y >= World.LoadedLevel.Grid.TileGrid.GetLength(1))
         {
            // Changing levels to south
            Coord levelCoords = World.LoadedLevel.LevelCoord.Add(new Coord(0, 1));
            if (levelCoords.Y < World.WorldMap.LevelMap.GetLength(1))
            {
               Level levelToSouth;
               if (!World.WorldMap.GetLevelAtCoords(levelCoords, out levelToSouth))
               {
                  return;
               }
               if (levelToSouth == null)
               {
                  if (output)
                  {
                     Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
                  }
                  return;
               }
               if (levelToSouth.NorthEntry != null && levelToSouth.Grid.GetTileAtCoords(levelToSouth.NorthEntry, out Tile southEntryTile))
               {
                  if (southEntryTile.Contents != null)
                  {
                     if (output)
                     {
                        Output.WriteLineTagged("Something is blocking this on the other side", Output.Tag.World);
                     }
                     return;
                  }
                  if (World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates, out Tile playerTile))
                  {
                     playerTile.Contents = null;
                  }
                  World.LoadedLevel = levelToSouth;
                  startingContents.Coordinates = World.LoadedLevel.NorthEntry;
                  southEntryTile.Contents = startingContents;
               }
               else
               {
                  if (output)
                  {
                     Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
                  }
                  return;
               }
            }
            else
            {
               if (output)
               {
                  Output.WriteLineTagged("You cannot move here", Output.Tag.Error);
               }
               return;
            }
         }
         else
         {
            Coord startingCoord = new Coord(startingContents.Coordinates.X, startingContents.Coordinates.Y);
            Tile endingTile;
            if (!World.LoadedLevel.Grid.GetTileAtCoords(startingContents.Coordinates.Add(changedLoc), out endingTile))
            {
               return;
            }
            if (endingTile.Contents != null)
            {
               if (output)
               {
                  Output.WriteLineTagged("Tile is occupied", Output.Tag.Error);
               }
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

      // Gets the tile in this grid at the coordinates given
      public bool GetTileAtCoords(Coord coords, out Tile result, bool consolePrint = true)
      {
         if (coords.X < 0 || coords.X >= TileGrid.GetLength(0) || coords.Y < 0 || coords.Y >= TileGrid.GetLength(1))
         {
            if (consolePrint)
            {
               Output.WriteLineTagged("Coordinates given are out of range of the grid", Output.Tag.Error);
            }
            result = null;
            return false;
         }
         result = TileGrid[coords.X, coords.Y];
         return true;
      }

      // Sets the contents of a tile at the coordinates given
      public void SetContentsAtCoords(Coord coords, Contents contents)
      {
         TileGrid[coords.X, coords.Y].Contents = contents;
      }

      // Sets the tile at the coordinates given
      public void SetTileAtCoords(Coord coords, Tile tile)
      {
         if (coords.X < 0 || coords.X >= TileGrid.GetLength(0) || coords.Y < 0 || coords.Y >= TileGrid.GetLength(1))
         {
            return;
         }
         TileGrid[coords.X, coords.Y] = tile;
      }

      // The visual representation of the grid. Printed frequently to the console
      public string GraphicString(bool LOS = true)
      {
         string returnString = "    ";
         for (int repeat = 0; repeat < TileGrid.GetLength(0); repeat++)
         {
            returnString += ((repeat % 10 == 0) ? AlphabetIndex((repeat / 10)) : ' ') + Settings.Spacing;
         }
         returnString += "\n    ";
         for (int repeat = 0; repeat < TileGrid.GetLength(0); repeat++)
         {
            returnString += (repeat % 10) + Settings.Spacing;
         }
         returnString += "\n  +";
         int maxRepeat = (TileGrid.GetLength(0) * (Settings.Spacing.Length + 1) + 1);
         for (int repeat = 0; repeat < maxRepeat; repeat++)
         {
            returnString += (World.LoadedLevel.NorthEntry != null && repeat == maxRepeat / 2) ? "^" : "-";
         }
         returnString += "+\n";

         for (int y = 0; y < TileGrid.GetLength(1); y++)
         {
            returnString += ((y % 10 == 0) ? AlphabetIndex(y / 10) : ' ').ToString() + (y % 10);

            returnString += (World.LoadedLevel.WestEntry != null && y == (TileGrid.GetLength(1) / 2)) ? "< " : "| ";

            for (int x = 0; x < TileGrid.GetLength(0); x++)
            {
               TileGrid[x, y].UpdateVisual(LOS);
               returnString += TileGrid[x, y].VisualChar + Settings.Spacing;
            }
            returnString += (World.LoadedLevel.EastEntry != null && y == (TileGrid.GetLength(1) / 2)) ? ">" : "|";
            returnString += "\n";
         }
         returnString += "  +";
         for (int repeat = 0; repeat < maxRepeat; repeat++)
         {
            returnString += (World.LoadedLevel.SouthEntry != null && repeat == maxRepeat / 2) ? "v" : "-";
         }
         returnString += "+\n";
         return returnString;
      }
      
      // Child function of GraphicString
      private char AlphabetIndex(int index)
      {
         return (char)(65 + index);
      }

      // Finds the first instance of a contents in this grid
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

      // Line of Sight

      // Returns true if a line drawn from StartCoord to RelativeCoord along TileGrid does not cross over any Transparent=false tiles
      public bool VisibleAtLine(Coord startCoord, Coord relativeCoord)
      {
         double x = startCoord.X + 0.5;
         double y = startCoord.Y + 0.5;

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
               if (y == startCoord.Y + 0.5)
               {
                  continue;
               }
               if (!World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)x, (int)y), out Tile tileAtCoords, false))
               {
                  return false;
               }
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
               if (x == startCoord.X + 0.5)
               {
                  continue;
               }
               y = GetYTile(x - 0.1, (double)slope, startCoord);

               double tryY = GetYTile(x + 0.1, (double)slope, startCoord);
               Tile tileAtCoords;
               if (tryY != y)
               {
                  if (!World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)x, (int)tryY), out tileAtCoords, false))
                  {
                     return false;
                  }
                  if (tileAtCoords != null && tileAtCoords.Contents != null && !tileAtCoords.Contents.Transparent)
                  {
                     return false;
                  }
               }

               if (!World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)x, (int)y), out tileAtCoords, false))
               {
                  return false;
               }

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
               if (y == startCoord.Y + 0.5)
               {
                  continue;
               }
               x = GetXTile(y - 0.1, (double)slope, startCoord);

               double tryX = GetXTile(y + 0.1, (double)slope, startCoord);
               Tile tileAtCoords;
               if (tryX != x)
               {
                  if (!World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)tryX, (int)y), out tileAtCoords, false))
                  {
                     return false;
                  }
                  if (tileAtCoords != null && tileAtCoords.Contents != null && !tileAtCoords.Contents.Transparent)
                  {
                     return false;
                  }
               }
               
               if (!World.LoadedLevel.Grid.GetTileAtCoords(new Coord((int)x, (int)y), out tileAtCoords, false))
               {
                  return false;
               }

               if (tileAtCoords != null && tileAtCoords.Contents != null && !tileAtCoords.Contents.Transparent)
               {
                  return false;
               }
            }
         }
         return true;
      }
      
      // Child function of VisibleAtLine. Important math function
      private double CenterTile(double input)
      {
         return (Math.Floor(input) + 0.5f);
      }
      
      // Child function of VisibleAtLine. Important math function
      private double GetYTile(double x, double slope, Coord startCoord)
        {
            if (slope == 0)
            {
                return CenterTile(startCoord.Y);
            }
            return CenterTile(startCoord.Y + slope * (x + (0.5 / slope) - startCoord.X - 0.5));
        }
      
      // Child function of VisibleAtLine. Important math function
      private double GetXTile(double y, double slope, Coord startCoord)
      {
         return CenterTile(startCoord.X + ((y + (slope * 0.5) - startCoord.Y - 0.5) / slope));
      }
   }
}
