
namespace GameEngine
{
	// Represents the player
	class Player
	{
		// The contents that the player has reference to
		public Contents Contents;

		// The contents (if any) that the player is holding
		public Contents Holding;

		// Strength determines how heavy a thing the player can pick up
		public int Strength;

		public Player(Contents contents, Contents holding, int strength)
		{
			Contents = contents;
			Holding = holding;

			Strength = strength;
		}
		
		// Returns Contents.coordinates
		public Coord GetCoords()
		{
			return Contents.Coordinates;
		}
	}
}
