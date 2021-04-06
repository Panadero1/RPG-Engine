
namespace GameEngine
{
	// A simple class, but an integral part of every Tile
	class Floor
	{
		// Visual character that represents the tile (when there is no contents on it)
		public char VisualChar;

		// Name of the Floor
		public string Name;

		public Floor(char visualChar, string name)
		{
			VisualChar = visualChar;
			Name = name;
		}
	}
}
