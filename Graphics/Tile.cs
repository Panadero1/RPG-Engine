using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
   class Tile : ICloneable
   {
      public Floor Floor;
      public Contents Contents = null;
      public char VisualChar;
      public Coord Coordinates;

      public Tile(Floor floor, Contents contents, Coord coordinates)
      {
         Floor = floor;
         Contents = contents;
         Coordinates = coordinates;
      }
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
