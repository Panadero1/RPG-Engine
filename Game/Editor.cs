using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
	// Variables for the level editor
	static class Editor
	{
		public static string[] AvailableTags = new string[]
		{
			"invulnerable",
			"explode",
			"nopick",
			"nodestroy",

		};

		// Whether the user is looking at the map or a specific level
		public enum State 
		{
			Map,
			Level
		}

		// ^
		public static State EditorState;

		// A reference to a Tile. Used for drawing
		public static Tile Brush;

	}
}