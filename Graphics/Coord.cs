using System;

namespace GameEngine
{
	// Coordinate class. For determining position on the grid
	class Coord
	{
		// Horizontal position
		public int X;

		// Vertical position
		public int Y;

		public Coord(int x, int y)
		{
			X = x;
			Y = y;
		}

		// Interprets string representation of coordinates and reterns a Coord
		public static bool FromAlphaNum(string alphaNumX, string alphaNumY, out Coord result)
		{
			alphaNumX = alphaNumX.ToUpper();
			alphaNumY = alphaNumY.ToUpper();
			#region tests
			if (alphaNumX.Length < 2)
			{
				Output.WriteLineTagged("'x' was not in alphaNum format: <letter><number>", Output.Tag.Error);
				result = null;
				return false;
			}
			if (alphaNumY.Length < 2)
			{
				Output.WriteLineTagged("'y' was not in alphaNum format: <letter><number>", Output.Tag.Error);
				result = null;
				return false;
			}
			if (!int.TryParse(alphaNumX[1].ToString(), out int x))
			{
				Output.WriteLineTagged("'x' was not in alphaNum format: <letter><number>", Output.Tag.Error);
				result = null;
				return false;
			}
			if (!int.TryParse(alphaNumY[1].ToString(), out int y))
			{
				Output.WriteLineTagged("'y' was not in alphaNum format: <letter><number>", Output.Tag.Error);
				result = null;
				return false;
			}

			if (int.TryParse(alphaNumX[0].ToString(), out _))
			{
				Output.WriteLineTagged("This part of alphaNum format for 'x' should not be an integer: <letter><number>", Output.Tag.Error);
				result = null;
				return false;
			}
			if (int.TryParse(alphaNumY[0].ToString(), out _))
			{
				Output.WriteLineTagged("This part of alphaNum format for 'y' should not be an integer: <letter><number>", Output.Tag.Error);
				result = null;
				return false;
			}

			#endregion
			
			result = new Coord(numFromAlphaNum(alphaNumX[0])*10 + x, numFromAlphaNum(alphaNumY[0])*10 + y);
			return true;
		}
		
		// Converts from Coord to string representation
		public string ToAlphaNum()
		{
			return (char)((X / 10) + 65) + "" + (X % 10) + " " + (char)((Y / 10) + 65) + "" + (Y % 10);
		}

		// Child function of FromAlphaNum()
		private static int numFromAlphaNum(char letter)
		{
			return (letter - 65);
		}
		
		// Finds the distance between two coordinates
		public double Distance(Coord coordsToCompare)
		{
			Coord distCoord = Subtract(coordsToCompare);
			return Math.Sqrt(Math.Pow(distCoord.X, 2) + Math.Pow(distCoord.Y, 2));
		}
		
		// Adds two coordinates
		public Coord Add(Coord coordsToAdd)
		{
			return new Coord(X + coordsToAdd.X, Y + coordsToAdd.Y);
		}
		
		// Subtracts the given coordinates from this coordinate
		public Coord Subtract(Coord coordsToSubtract)
		{
			return new Coord(X - coordsToSubtract.X, Y - coordsToSubtract.Y);
		}
	}
}
