
namespace GameEngine
{
   class Level
   {
      public char _visualChar;
      public string _name;
      public Grid _grid;
      public int _width;
      public int _height;

      public Coord _levelCoord;

      public Coord _northEntry;
      public Coord _eastEntry;
      public Coord _southEntry;
      public Coord _westEntry;

      public Level(string name, char visualChar, Grid grid, Coord levelCoord, Coord northEntry, Coord eastEntry, Coord southEntry, Coord westEntry)
      {
         _name = name;
         _visualChar = visualChar;
         _grid = grid;
         _width = grid._tileGrid.GetLength(0);
         _height = grid._tileGrid.GetLength(1);
         _levelCoord = levelCoord;

         _northEntry = northEntry;
         _eastEntry = eastEntry;
         _southEntry = southEntry;
         _westEntry = westEntry;
      }
   }
}
