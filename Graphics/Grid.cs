using System;

namespace GameEngine
{
   class Grid
   {
      public Tile[,] _tileGrid;

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
               Floor oldFloor = tileGrid[y][x]._floor;
               Contents oldContents = tileGrid[y][x]._contents;
               if (oldContents == null)
               {
                  createdTiles[x, y] = new Tile(new Floor(oldFloor._visualChar, oldFloor._name), null, new Coord(x, y));
               }
               else
               {
                  createdTiles[x, y] = new Tile(new Floor(oldFloor._visualChar, oldFloor._name), new Contents(oldContents._name, oldContents._visualChar, oldContents._temperature, oldContents._meltingpoint, oldContents._transparent, oldContents._durability, oldContents._size, oldContents._weight, oldContents._container, oldContents._containerSpace, oldContents._contained, oldContents._useAction, oldContents._behavior), new Coord(x, y));
                  createdTiles[x, y]._contents._coordinates = new Coord(x, y);
                  if (oldContents._name == World._player._contents._name)
                  {
                     World._player._contents = createdTiles[x, y]._contents;
                  }
               }
            }
         }
         _tileGrid = createdTiles;
      }

      public void MoveContents(Contents startingContents, Coord changedLoc)
      {
         if (changedLoc._x == 0 && changedLoc._y == 0)
         {
            return;
         }
         Coord endingCoord = startingContents._coordinates.Add(changedLoc);


         // This chain of 'if's in combination with the 'else' statments DOES NOT WORK WITH DIAGONAL MOVEMENT. This is intentional as of 2/11/2021
         if (endingCoord._x < 0)
         {
            // Changing levels to the west
            Coord levelCoords = World._loadedLevel._levelCoord.Add(new Coord(-1, 0));
            if (levelCoords._x >= 0)
            {
               Level levelToWest = World._worldMap.GetLevelAtCoords(levelCoords);
               if (levelToWest == null)
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
               if (levelToWest._eastEntry != null && levelToWest._grid.GetTileAtCoords(levelToWest._eastEntry)._contents == null)
               {
                  World._loadedLevel._grid.GetTileAtCoords(startingContents._coordinates)._contents = null;
                  World._loadedLevel = levelToWest;
                  startingContents._coordinates = World._loadedLevel._eastEntry;
                  World._loadedLevel._grid.GetTileAtCoords(World._loadedLevel._eastEntry)._contents = startingContents;
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
         else if (endingCoord._x >= World._loadedLevel._width)
         {
            // Changing levels to east
            Coord levelCoords = World._loadedLevel._levelCoord.Add(new Coord(1, 0));
            if (levelCoords._x < World._worldMap._width)
            {
               Level levelToEast = World._worldMap.GetLevelAtCoords(levelCoords);
               if (levelToEast == null)
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
               if (levelToEast._westEntry != null && levelToEast._grid.GetTileAtCoords(levelToEast._westEntry)._contents == null)
               {
                  World._loadedLevel._grid.GetTileAtCoords(startingContents._coordinates)._contents = null;
                  World._loadedLevel = levelToEast;
                  startingContents._coordinates = World._loadedLevel._westEntry;
                  World._loadedLevel._grid.GetTileAtCoords(World._loadedLevel._westEntry)._contents = startingContents;
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
         else if (endingCoord._y < 0)
         {
            // Changing levels to north
            Coord levelCoords = World._loadedLevel._levelCoord.Add(new Coord(0, -1));
            if (levelCoords._y >= 0)
            {
               Level levelToNorth = World._worldMap.GetLevelAtCoords(levelCoords);
               if (levelToNorth == null)
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
               if (levelToNorth._southEntry != null && levelToNorth._grid.GetTileAtCoords(levelToNorth._southEntry)._contents == null)
               {
                  World._loadedLevel._grid.GetTileAtCoords(startingContents._coordinates)._contents = null;
                  World._loadedLevel = levelToNorth;
                  startingContents._coordinates = World._loadedLevel._southEntry;
                  World._loadedLevel._grid.GetTileAtCoords(World._loadedLevel._southEntry)._contents = startingContents;
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
         else if (endingCoord._y >= World._loadedLevel._height)
         {
            // Changing levels to south
            Coord levelCoords = World._loadedLevel._levelCoord.Add(new Coord(0, 1));
            if (levelCoords._y < World._worldMap._height)
            {
               Level levelToSouth = World._worldMap.GetLevelAtCoords(levelCoords);
               if (levelToSouth == null)
               {
                  Console.WriteLine("You cannot move here");
                  return;
               }
               if (levelToSouth._northEntry != null && levelToSouth._grid.GetTileAtCoords(levelToSouth._northEntry)._contents == null)
               {
                  World._loadedLevel._grid.GetTileAtCoords(startingContents._coordinates)._contents = null;
                  World._loadedLevel = levelToSouth;
                  startingContents._coordinates = World._loadedLevel._northEntry;
                  World._loadedLevel._grid.GetTileAtCoords(World._loadedLevel._northEntry)._contents = startingContents;
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
            Coord startingCoord = new Coord(startingContents._coordinates._x, startingContents._coordinates._y);
            Tile endingTile = World._loadedLevel._grid.GetTileAtCoords(startingContents._coordinates.Add(changedLoc));
            if (endingTile == null)
            {
               return;
            }
            if (endingTile._contents != null)
            {
               Console.WriteLine("Tile is occupied");
               return;
            }
            Contents removedContents = startingContents;
            Coord newCoord = startingContents._coordinates.Add(changedLoc);
            removedContents._coordinates = newCoord;
            endingTile._contents = removedContents;
            if (removedContents._name == World._player._contents._name)
            {
               World._player._contents = removedContents;
            }
            World._loadedLevel._grid._tileGrid[startingCoord._x, startingCoord._y]._contents = null;
         }

      }

      public Tile GetTileAtCoords(Coord coords, bool consolePrint = true)
      {
         if (coords._x < 0 || coords._x >= _tileGrid.GetLength(0) || coords._y < 0 || coords._y >= _tileGrid.GetLength(1))
         {
            if (consolePrint)
            {
               Console.WriteLine("Coordinates given are out of range of the grid");
            }
            return null;
         }
         return _tileGrid[coords._x, coords._y];
      }

      public void SetContentsAtCoords(Coord coords, Contents contents)
      {
         _tileGrid[coords._x, coords._y]._contents = contents;
      }

      public string GraphicString()
      {
         string returnString = "   ";
         for (int repeat = 0; repeat < _tileGrid.GetLength(0); repeat++)
         {
            returnString += ((repeat % 10 == 0) ? AlphabetIndex((repeat / 10)) : ' ') + (Settings._spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < _tileGrid.GetLength(0); repeat++)
         {
            returnString += (repeat % 10) + (Settings._spaced ? " " : "");
         }
         returnString += "\n   ";
         for (int repeat = 0; repeat < _tileGrid.GetLength(0) + (Settings._spaced ? _tileGrid.GetLength(0) - 1 : 0); repeat++)
         {
            returnString += "-";
         }
         returnString += "\n";

         for (int y = 0; y < _tileGrid.GetLength(1); y++)
         {
            returnString += ((y % 10 == 0) ? AlphabetIndex(y / 10) : ' ') + (y % 10 + "|");

            for (int x = 0; x < _tileGrid.GetLength(0); x++)
            {
               _tileGrid[x, y].UpdateVisual();
               returnString += _tileGrid[x, y]._visualChar + (Settings._spaced ? " " : "");
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
         for (int y = 0; y < _tileGrid.GetLength(1); y++)
         {
            for (int x = 0; x < _tileGrid.GetLength(0); x++)
            {
               if (_tileGrid[x, y]._contents == null)
               {
                  continue;
               }
               if (_tileGrid[x, y]._contents._name == compareContents._name)
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
         Coord playerCoords = World._player.GetCoords();

         double x = playerCoords._x + 0.5;
         double y = playerCoords._y + 0.5;

         double endingX = x + relativeCoord._x;
         double endingY = y + relativeCoord._y;

         int xSign = relativeCoord._x == 0 ? 0 : (relativeCoord._x / Math.Abs(relativeCoord._x));
         int ySign = relativeCoord._y == 0 ? 0 : (relativeCoord._y / Math.Abs(relativeCoord._y));


         double? slope;
         if (relativeCoord._x != 0)
         {
            slope = (double)relativeCoord._y / (double)relativeCoord._x;
         }
         else
         {
            for (; y != endingY; y += ySign)
            {
               Tile tileAtCoords = World._loadedLevel._grid.GetTileAtCoords(new Coord((int)x, (int)y), false);
               if (tileAtCoords != null && tileAtCoords._contents != null && !tileAtCoords._contents._transparent)
               {
                  return false;
               }
            }
            return true;
         }
         bool xMove = Math.Abs(relativeCoord._x) > Math.Abs(relativeCoord._y);

         if (xMove)
         {
            for (; x != endingX; x += xSign)
            {
               y = GetYTile(x - 0.1, (double)slope);

               double tryY = GetYTile(x + 0.1, (double)slope);
               Tile tileAtCoords;
               if (tryY != y)
               {
                  tileAtCoords = World._loadedLevel._grid.GetTileAtCoords(new Coord((int)x, (int)tryY), false);
                  if (tileAtCoords != null && tileAtCoords._contents != null && !tileAtCoords._contents._transparent)
                  {
                     return false;
                  }
               }

               tileAtCoords = World._loadedLevel._grid.GetTileAtCoords(new Coord((int)x, (int)y), false);

               if (tileAtCoords != null && tileAtCoords._contents != null && !tileAtCoords._contents._transparent)
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
                  tileAtCoords = World._loadedLevel._grid.GetTileAtCoords(new Coord((int)tryX, (int)y), false);
                  if (tileAtCoords != null && tileAtCoords._contents != null && !tileAtCoords._contents._transparent)
                  {
                     return false;
                  }
               }

               tileAtCoords = World._loadedLevel._grid.GetTileAtCoords(new Coord((int)x, (int)y), false);

               if (tileAtCoords != null && tileAtCoords._contents != null && !tileAtCoords._contents._transparent)
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
         Coord playerCoord = World._player.GetCoords();
         if (slope == 0)
         {
            return CenterTile(playerCoord._y);
         }
         return CenterTile(playerCoord._y + slope * (x + (0.5 / slope) - playerCoord._x - 0.5));
      }
      private double GetXTile(double y, double slope)
      {
         Coord playerCoord = World._player.GetCoords();
         return CenterTile(playerCoord._x + ((y + (slope * 0.5) - playerCoord._y - 0.5) / slope));
      }

      // </LOS>

      public void RunBehavior()
      {
         foreach (Tile tile in _tileGrid)
         {
            tile._contents._behavior(tile._contents);
         }
      }
   }
}
