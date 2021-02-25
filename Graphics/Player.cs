
namespace GameEngine
{
   class Player
   {
      public Contents Contents;
      public Contents Holding;

      public int Strength;

      public Player(Contents contents, Contents holding, int strength)
      {
         Contents = contents;
         Holding = holding;

         Strength = strength;
      }
      public Coord GetCoords()
      {
         return Contents.Coordinates;
      }
   }
}
