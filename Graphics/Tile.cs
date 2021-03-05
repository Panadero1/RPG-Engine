using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
   // Most used data class is the Tile. It contains a Contents and a Floor
   class Tile : ICloneable
   {
      // The floor of the tile
      public Floor Floor;

      // The Contents of the tile
      public Contents Contents = null;

      // The visual character of the tile
      public char VisualChar;

      // The coordinates of the tile
      public Coord Coordinates;

      public Tile(Floor floor, Contents contents, Coord coordinates)
      {
         Floor = floor;
         Contents = contents;
         Coordinates = coordinates;
      }
      
      // Updates VisualChar depending on whether there is a contents above it (default is Floor.VisualChar)
      public void UpdateVisual(bool LOS)
      {
         Coord playerCoords = World.Player.GetCoords();
         if (LOS && !World.LoadedLevel.Grid.VisibleAtLine(playerCoords, new Coord(Coordinates.X - playerCoords.X, Coordinates.Y - playerCoords.Y)))
         {
            VisualChar = ' ';
         }
         else
         {
            VisualChar = Contents != null ? Contents.VisualChar : Floor.VisualChar;
         }
      }
      public object Clone()
      {
         if (Contents == null)
         {
            return new Tile(Floor, null, Coordinates);
         }
         return new Tile(Floor, (Contents)Contents.Clone(), Coordinates);
      }
   }
}
