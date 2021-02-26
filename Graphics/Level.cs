
namespace GameEngine
{
   class Level
   {
      public char VisualChar;
      public string Name;
      public Grid Grid;

      public Coord LevelCoord;

      public Coord NorthEntry;
      public Coord EastEntry;
      public Coord SouthEntry;
      public Coord WestEntry;

      public Level(string name, char visualChar, Grid grid, Coord levelCoord, Coord northEntry, Coord eastEntry, Coord southEntry, Coord westEntry)
      {
         Name = name;
         VisualChar = visualChar;
         Grid = grid;
         LevelCoord = levelCoord;

         NorthEntry = northEntry;
         EastEntry = eastEntry;
         SouthEntry = southEntry;
         WestEntry = westEntry;
      }
   }
}
