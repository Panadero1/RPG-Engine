using System;
using System.Collections.Generic;
using System.Text;

namespace A
{
   class Tile : ICloneable
   {
      public Floor _floor;
      public Contents _contents = null;
      public char _visualChar;
      public Coord _coordinates;

      public Tile(Floor floor, Contents contents, Coord coordinates)
      {
         _floor = floor;
         _contents = contents;
         _coordinates = coordinates;
      }
      public void UpdateVisual()
      {
         Coord playerCoords = World._player.GetCoords();
         if (!World._loadedLevel._grid.VisibleAtLine(new Coord(_coordinates._x - playerCoords._x, _coordinates._y - playerCoords._y)))
         {
            _visualChar = ' ';
         }
         else
         {
            _visualChar = _contents != null ? _contents._visualChar : _floor._visualChar;
         }
      }
      public object Clone()
      {
         if (_contents == null)
         {
            return new Tile(_floor, null, _coordinates);
         }
         return new Tile(_floor, (Contents)_contents.Clone(), _coordinates);
      }
   }
}
