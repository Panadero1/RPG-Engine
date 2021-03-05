
namespace GameEngine
{
   // Data class that has many important variables
   class Level
   {
      // Visual representation of the level on the map
      public char VisualChar;

      // Identifier of the level. Not currently used for anything
      public string Name;

      // The current Grid of the level
      public Grid Grid;

      // The coordinates of the Level on the map
      public Coord LevelCoord;

      // The coordinates where the player starts if they came from the north
      public Coord NorthEntry;

      // The coordinates where the player starts if they came from the east
      public Coord EastEntry;

      // The coordinates where the player starts if they came from the south
      public Coord SouthEntry;

      // The coordinates where the player starts if they came from the west
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
