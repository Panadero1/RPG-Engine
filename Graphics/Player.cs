
namespace GameEngine
{
   class Player
   {
      public Contents _contents;
      public Contents _holding;

      public int _strength;

      public Player(Contents contents, Contents holding, int strength)
      {
         _contents = contents;
         _holding = holding;

         _strength = strength;
      }
      public Coord GetCoords()
      {
         return _contents._coordinates;
      }
   }
}
