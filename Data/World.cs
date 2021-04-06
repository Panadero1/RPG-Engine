using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace GameEngine
{
	// Where all world data is stored.
	static class World
	{
		// Coordinates are loaded into each tile & contents when they run through the Grid() constructor, so this is just used as a filler
		private static readonly Coord _zeroZero = new Coord(0, 0);

		// The player is the controllable character. It is set as a default only for the purpose of satisfying a default player for levelEditor
		public static Player Player = new Player(new Contents("Player", 0, 'A', true, 10, 10, 50, true, 100, new List<Contents>() { }, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing }), null, 50);

		// Tile templates used to make the demo.txt file.. some are used in the tutorial. Not used for level editing whatsoever
		#region tile definitions

		public static Floor Ground = new Floor('.', "ground");

		public static Tile Empty = new Tile(Ground, null, _zeroZero);

		public static Tile Wall = new Tile(Ground, new Contents("wall", 0, '#', false, 40, 10000, 400000, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile Fence = new Tile(Ground, new Contents("fence", 0, '+', true, 5, 10000, 60, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile Hog = new Tile(new Floor('~', "mudpit"), new Contents("hog", 0, 'H', true, 4, 40, 20, UseActions.DoesNothing, new Action<Contents>[] { Behavior.AttackClose, Behavior.Wander, Behavior.MoveTowardsPlayer }), _zeroZero);

		public static Tile Plant = new Tile(new Floor('Y', "plant"), null, _zeroZero);

		public static Tile Chicken = new Tile(Ground, new Contents("chicken", 0, '>', true, 1, 2, 5, UseActions.DoesNothing, new Action<Contents>[] { Behavior.Wander }), _zeroZero);

		public static Tile Person = new Tile(Ground, new Contents("person", 0, 'p', true, 1, 1, 1000, UseActions.Rude, new Action<Contents>[] { Behavior.Wander }), _zeroZero);

		public static Tile Rock = new Tile(Ground, new Contents("rock", 0, 'o', true, 4, 1, 3, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile Lever = new Tile(Ground, new Contents("lever", 0, 'L', true, 10, 234, 5151, UseActions.Lever, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile Target = new Tile(Ground, new Contents("target", 0, '@', true, 2, 1000, 1000, UseActions.DoesNothing, new Action<Contents>[] { Behavior.Target }), _zeroZero);

		public static Tile GunTile = new Tile(Ground, new Contents("gun", 0, 'r', true, 4, 4, 5, UseActions.Gun, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile Sign = new Tile(Ground, new Contents("sign", 130, 'S', true, 4, 10000, 100000, UseActions.Dialogue, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile GateOpen = new Tile(new Floor('.', "retractedGate"), null, _zeroZero);

		public static Tile GateClosed = new Tile(Ground, new Contents("gate", 0, '-', true, 2000, 10, 12390, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);
		#endregion

		public static Dictionary<string, Tile> Palette = new Dictionary<string, Tile>()
		{
			{ "Empty", Empty },
			{ "Wall", Wall },
			{ "Fence", Fence },
			{ "Hog", Hog },
			{ "Plant", Plant },
			{ "Chicken", Chicken },
			{ "Person", Person },
			{ "Rock", Rock },
			{ "Lever", Lever },
			{ "Target", Target },
			{ "Gun", GunTile},
			{ "Gate(open)", GateOpen},
			{ "Gate(closed)", GateClosed }
		};


		// A dictionary mapping between content names and their respective dialogue lines
		// This means that two contents with Dialogue as the UseAction
		public static Dictionary<int, string> Dialogue = new Dictionary<int, string>();

		// WorldMap is referenced as the current Map in play. This gets defined in LoadFromFile
		public static Map WorldMap;

		// Solidly defined within the program as the tutorial level. Playable without any world files. This also means it's not editable
		public static Map TutorialLevel = new Map(new Level[,]
		{
			// column 1
			{
				new Level(
					"Field",
					'F',
					new Grid(
						new Tile[][]
						{
							// row 1
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Fence.Clone(),
								(Tile)Target.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Fence.Clone(),
								(Tile)GateClosed.Clone()
							},
							// row 2
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)GunTile.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Fence.Clone(),
								(Tile)Fence.Clone(),
								(Tile)Fence.Clone(),
								(Tile)Fence.Clone(),
								(Tile)GateClosed.Clone()
							},
							// row 3 
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								new Tile(Ground, Player.Contents, _zeroZero),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)GateClosed.Clone()
							},
							// row 4
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)GateClosed.Clone()
							}
						}
					),
					new Coord(0, 0),
					null,
					new Coord(7, 2),
					null,
					null
				)
			},
			// column 2
			{
				new Level(
					"Trap",
					'T',
					new Grid
					(
						new Tile[][]
						{
							// row 1
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Lever.Clone(),
								(Tile)Wall.Clone(),
								(Tile)Lever.Clone(),
								new Tile(Ground, new Contents(
									"chest",
									Contents.UniqueID(),
									'C',
									true,
									10,
									1000,
									1000,
									true,
									100,
									new List<Contents>()
									{
										new Contents("coin", Contents.UniqueID(), 'c', true, 3, 1, 1, UseActions.DoesNothing, new Action<Contents>[] {Behavior.DoesNothing}),
										new Contents("coin", Contents.UniqueID(), 'c', true, 3, 1, 1, UseActions.DoesNothing, new Action<Contents>[] {Behavior.DoesNothing}),
										new Contents("coin", Contents.UniqueID(), 'c', true, 3, 1, 1, UseActions.DoesNothing, new Action<Contents>[] {Behavior.DoesNothing})
									},
									UseActions.DoesNothing,
									new Action<Contents>[] {Behavior.DoesNothing}), _zeroZero),
							},
							// row 2
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)GateClosed.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
							},
							// row 3
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)GateClosed.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
							},
							// row 4
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Sign.Clone(),
								(Tile)Wall.Clone(),
								(Tile)GateOpen.Clone(),
								(Tile)GateOpen.Clone(),
							},
							// row 5
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Wall.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
							},
							// row 6
							new Tile[]
							{
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Wall.Clone(),
								(Tile)Empty.Clone(),
								(Tile)Hog.Clone(),
							}
						}
					),
					new Coord(1, 0),
					null,
					null,
					null,
					new Coord(0, 2)
				)
			}
		}, "tutorial");

		public static Level LoadedLevel;

		// This is what is stored for when the 'index' command is typed. It is stored in each world file and generates as the player looks at more things
		public static List<Contents> ContentsIndex = new List<Contents>();

		// This tries to open the 'maps' folder in the directory. It will safely exit the program upon not finding it.
		public static bool TryGetMapsFolder(out string result)
		{
			string tryFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\maps\";

			if (Directory.Exists(tryFolder))
			{
				result = tryFolder;
				return true;
			}
			else
			{
				Output.WriteLineTagged("Your 'maps' folder may be missing. We're making one now. Be sure to put all your world files in the folder named 'maps' within the directory of the executable", Output.Tag.Error);
				Directory.CreateDirectory(tryFolder);
				result = null;
				return false;
			}
		}

		// This does none of the loading; it just lists all the loadable files and prompts the user to pick one. It then redirects to the function below.
		public static bool LoadFromFile()
		{
			string fileMouth;
			if (!TryGetMapsFolder(out fileMouth))
			{
				return false;
			}

			DirectoryInfo directory = new DirectoryInfo(fileMouth);

			Dictionary<string, FileInfo> files = new Dictionary<string, FileInfo>();
			foreach (FileInfo file in directory.GetFiles("*.txt"))
			{
				files.Add(file.Name, file);
			}

			if (files.Count == 0)
			{
				Output.WriteLineTagged("It appears there are no files to load. Either download a world file, or try designing one in the level editor", Output.Tag.Error);
				return false;
			}

			string[] fileNames = files.Keys.ToArray();

			Output.WriteLineTagged("Choose a file from the options below.", Output.Tag.Prompt);
			if (CommandInterpretation.InterpretString(fileNames, out string result))
			{
				Game.FilePath = files[result].FullName;
				LoadFromFile(files[result].FullName);
			}
			else
			{
				if (CommandInterpretation.AskYesNo("No file found with that name. Would you like to try again?"))
				{
					return LoadFromFile();
				}
				return false;
			}
			return true;
		}

		// This is where all world file interpretation takes place
		// <worldFile>.txt -> WorldMap
		public static void LoadFromFile(string filePath)
		{
			Output.WriteLineToConsole("Loading file " + filePath + "...");
			StreamReader sr;
			try
			{
				sr = new StreamReader(filePath);
			}
			catch
			{
				Output.WriteLineTagged("File path is incorrect", Output.Tag.Error);
				return;
			}

			string[] splitLine = SplitNextLine(sr);
			string mapName = splitLine[1];

			List<Level[]> levelGrid = new List<Level[]>();
			// each row of levels v
			for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
			{
				List<Level> levels = new List<Level>();
				// each level within the given row v
				for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
				{
					if (splitLine[0] == "null")
					{
						levels.Add(null);
						continue;
					}
					List<Tile[]> tileGrid = new List<Tile[]>();
					// each row of tiles within the given level v
					for (string[] tileRowLine = SplitNextLine(sr); tileRowLine[0] != "}"; tileRowLine = SplitNextLine(sr))
					{
						List<Tile> tiles = new List<Tile>();
						// each tile within the given row v
						for (tileRowLine = SplitNextLine(sr); tileRowLine[0] != "}"; tileRowLine = SplitNextLine(sr))
						{
							tileRowLine = SplitNextLine(sr);
							Floor floor = new Floor(tileRowLine[2][0], tileRowLine[1]);
							Contents contents = GetAllContents(sr)[0];
							tileRowLine = SplitNextLine(sr);
							Coord coordinates = new Coord(int.Parse(tileRowLine[1]), int.Parse(tileRowLine[2]));
							if (contents != null)
							{
								contents.Coordinates = coordinates;
							}

							tiles.Add(new Tile(floor, contents, coordinates));
							sr.ReadLine();
						}
						tileGrid.Add(tiles.ToArray());
					}

					// Parsing Coord from comma-delimited storage on file
					Coord levelCoord = new Coord(int.Parse(splitLine[4].Split(",")[0]), int.Parse(splitLine[4].Split(",")[1]));
					Coord northEntry;
					Coord eastEntry;
					Coord southEntry;
					Coord westEntry;
					if (splitLine[5] == "null")
					{
						northEntry = null;
					}
					else
					{
						northEntry = new Coord(int.Parse(splitLine[5].Split(",")[0]), int.Parse(splitLine[5].Split(",")[1]));
					}

					if (splitLine[6] == "null")
					{
						eastEntry = null;
					}
					else
					{
						eastEntry = new Coord(int.Parse(splitLine[6].Split(",")[0]), int.Parse(splitLine[6].Split(",")[1]));
					}

					if (splitLine[7] == "null")
					{
						southEntry = null;
					}
					else
					{
						southEntry = new Coord(int.Parse(splitLine[7].Split(",")[0]), int.Parse(splitLine[7].Split(",")[1]));
					}

					if (splitLine[8] == "null")
					{
						westEntry = null;
					}
					else
					{
						westEntry = new Coord(int.Parse(splitLine[8].Split(",")[0]), int.Parse(splitLine[8].Split(",")[1]));
					}

					Level levelToAdd = new Level(splitLine[1], splitLine[2][0], new Grid(tileGrid.ToArray()), levelCoord, northEntry, eastEntry, southEntry, westEntry);
					levels.Add(levelToAdd);
					if (bool.Parse(splitLine[3]))
					{
						LoadedLevel = levelToAdd;
					}
				}
				levelGrid.Add(levels.ToArray());
			}
			WorldMap = new Map(new Level[levelGrid[0].Length, levelGrid.Count], mapName);
			for (int y = 0; y < WorldMap.LevelMap.GetLength(1); y++)
			{
				for (int x = 0; x < WorldMap.LevelMap.GetLength(0); x++)
				{
					WorldMap.LevelMap[x, y] = levelGrid[y][x];
				}
			}

			sr.ReadLine();

			splitLine = SplitNextLine(sr);

			Coord playerCoords = new Coord(int.Parse(splitLine[0]), int.Parse(splitLine[1]));

			List<Contents> allContents = GetAllContents(sr);

			if (!World.LoadedLevel.Grid.GetTileAtCoords(playerCoords, out Tile tile, false))
			{
				return;
			}

			Player = new Player(tile.Contents, allContents[0], int.Parse(sr.ReadLine()));

			Player.Contents.Coordinates = playerCoords;

			sr.ReadLine();

			ContentsIndex = GetAllContents(sr);

			sr.ReadLine();

			for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
			{
				string restOfLine = "";
				for (int splitLineIndex = 1; splitLineIndex < splitLine.Length; splitLineIndex++)
				{
					restOfLine += splitLine[splitLineIndex] + " ";
				}
				restOfLine.Trim();
				int id = int.Parse(splitLine[0]);
				Dialogue.Add(id, restOfLine);

				if (id > Contents.uniqueIndex)
				{
					Contents.uniqueIndex = id;
				}
			}
			sr.ReadLine();

			for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
			{
				string currentEvent = splitLine[0];
				for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
				{
					EventHandler.IdentifierEventMapping[currentEvent].ConnectionList.Add(new Connection(int.Parse(splitLine[0]), int.Parse(splitLine[1]), splitLine[2], string.Join(' ', splitLine, 3, splitLine.Length - 3)));
				}
			}

			sr.Close();
		}

		// This is a child function of the one above. It recursively gets all contents, since a contents can contain a contents. 
		// This is the only reasonable way to get all contents (thus, the name)
		private static List<Contents> GetAllContents(StreamReader sr)
		{
			sr.ReadLine();
			List<Contents> contentsList = new List<Contents>();
			for (string[] splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
			{
				if (splitLine[0] == "null")
				{
					contentsList.Add(null);
					continue;
				}
				int id = int.Parse(splitLine[1]);
				if (id > Contents.uniqueIndex)
				{
					Contents.uniqueIndex = id;
				}
				bool isContainer = bool.Parse(splitLine[7]);
				if (UseActions.TryGetAction(splitLine[9], out Action<string[], Contents> actionResult) && Behavior.TryGetBehaviors(splitLine[10].Split(","), out Action<Contents>[] behaviorResult))
				{
					contentsList.Add(new Contents(
					name: splitLine[0],
					id: id,
					visualChar: splitLine[2][0],
					transparent: bool.Parse(splitLine[3]),
					durability: int.Parse(splitLine[4]),
					size: int.Parse(splitLine[5]),
					weight: float.Parse(splitLine[6]),
					container: isContainer,
					containerSpace: int.Parse(splitLine[8]),
					useAction: actionResult,
					behaviors: behaviorResult,
					contained: (isContainer ? GetAllContents(sr) : null)
					));
					if (!isContainer)
					{
						sr.ReadLine();
						sr.ReadLine();
						sr.ReadLine();
					}
					List<string> contentTags = new List<string>();
					sr.ReadLine();
					for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
					{
						contentTags.Add(splitLine[0]);
					}
					contentsList[contentsList.Count - 1].Tags = contentTags.ToArray();
				}
			}
			return contentsList;
		}

		// Another child function of LoadFromFile. This one just splits the next line returned into an array delimited by spaces
		// TODO: add custom string?
		private static string[] SplitNextLine(StreamReader sr)
		{
			return sr.ReadLine().Split(" ");
		}

		// This saves the world map into a file
		// WorldMap -> <worldFile>.txt
		public static void SaveToFile(string filePath)
		{
			StreamWriter sw;
			try
			{
				sw = new StreamWriter(filePath);
			}
			catch
			{
				if (CommandInterpretation.AskYesNo("File path is invalid. Would you like to try again?"))
				{
					SaveToFile(CommandInterpretation.GetUserResponse("Enter file path"));
				}
				return;
			}
			Level[,] levelMap = WorldMap.LevelMap;
			sw.WriteLine("map " + WorldMap.Name + " {");
			for (int levelY = 0; levelY < levelMap.GetLength(1); levelY++)
			{
				sw.WriteLine("row " + levelY + " {");
				for (int levelX = 0; levelX < levelMap.GetLength(0); levelX++)
				{
					Level level = levelMap[levelX, levelY];
					if (level == null)
					{
						sw.WriteLine("null");
						continue;
					}
					// v This is on one line
					sw.WriteLine(
						"level " + level.Name + " " + level.VisualChar + " " + (level.Equals(LoadedLevel)) +
						" " + level.LevelCoord.X + "," + level.LevelCoord.Y +
						" " + (level.NorthEntry == null ? "null" : (level.NorthEntry.X + "," + level.NorthEntry.Y)) +
						" " + (level.EastEntry == null ? "null" : (level.EastEntry.X + "," + level.EastEntry.Y)) +
						" " + (level.SouthEntry == null ? "null" : (level.SouthEntry.X + "," + level.SouthEntry.Y)) +
						" " + (level.WestEntry == null ? "null" : (level.WestEntry.X + "," + level.WestEntry.Y)) +
						" " + "grid {");
					for (int y = 0; y < level.Grid.TileGrid.GetLength(1); y++)
					{
						sw.WriteLine("row " + y + " {");
						for (int x = 0; x < level.Grid.TileGrid.GetLength(0); x++)
						{
							Tile tileAtCoords = level.Grid.TileGrid[x, y];
							sw.WriteLine("tile {");

							sw.WriteLine("floor " + tileAtCoords.Floor.Name + " " + tileAtCoords.Floor.VisualChar);
							sw.WriteLine("contents {");
							ListAllContents(tileAtCoords.Contents, sw);
							sw.WriteLine("}");
							sw.WriteLine("coordinates " + x + " " + y);

							sw.WriteLine("}");
						}
						sw.WriteLine("}");
					}
					sw.WriteLine("}");
				}
				sw.WriteLine("}");
			}
			sw.WriteLine("}");

			sw.WriteLine("player {");
			sw.WriteLine(Player.Contents.Coordinates.X + " " + Player.Contents.Coordinates.Y);
			sw.WriteLine("holding {");
			ListAllContents(Player.Holding, sw);
			sw.WriteLine("}");
			sw.WriteLine(Player.Strength);
			sw.WriteLine("}");

			sw.WriteLine("Tile index {");

			foreach (Contents contents in ContentsIndex)
			{
				contents.Container = false;
				ListAllContents(contents, sw);
			}

			sw.WriteLine("}");

			sw.WriteLine("Dialogue {");

			foreach (int key in Dialogue.Keys)
			{
				sw.WriteLine(key + " " + Dialogue[key]);
			}

			sw.WriteLine("}");

			sw.WriteLine("Connections {");

			foreach (string key in EventHandler.IdentifierEventMapping.Keys)
			{
				sw.WriteLine(key + " {");

				foreach (Connection connection in EventHandler.IdentifierEventMapping[key].ConnectionList)
				{
					sw.WriteLine(connection.TriggerContentsID + " " + connection.ResultContentsID + " " + connection.ResultType + " " + connection.ResultInformation);
				}

				sw.WriteLine("}");
			}

			sw.WriteLine("}");

			sw.Close();
			Output.WriteLineToConsole("World file saved successfully as " + Game.FilePath);
		}

		// This is a child function of the one above. It does the inverse of GetAllContents
		private static void ListAllContents(Contents contentsAtCoords, StreamWriter sw)
		{
			if (contentsAtCoords == null)
			{
				sw.WriteLine("null");
				return;
			}
			sw.Write(contentsAtCoords.Name + " " + contentsAtCoords.ID + " " + contentsAtCoords.VisualChar + " " + contentsAtCoords.Transparent + " " + contentsAtCoords.Durability + " " + contentsAtCoords.Size + " " + contentsAtCoords.Weight + " ");
			sw.Write(contentsAtCoords.Container + " " + contentsAtCoords.ContainerSpace + " ");
			if (UseActions.TryGetIdentifier(contentsAtCoords.UseAction, out string actionResult) && Behavior.TryGetIdentifiers(contentsAtCoords.Behaviors, out string behaviorResult))
			{
				sw.Write(actionResult + " ");
				sw.WriteLine(behaviorResult);
			}
			sw.WriteLine("contained {");
			if (!contentsAtCoords.Container)
			{
				sw.WriteLine("null");
			}
			else
			{
				foreach (Contents contained in contentsAtCoords.Contained)
				{
					ListAllContents(contained, sw);
				}
			}
			sw.WriteLine("}");
			sw.WriteLine("tags {");
			foreach (string tag in contentsAtCoords.Tags)
			{
				sw.WriteLine(tag);
			}
			sw.WriteLine("}");
		}

		// This runs through and updates every Tile *AND* runs its behavior
		public static void UpdateWorld()
		{
			Level realLoadedLevel = LoadedLevel;
			foreach (Level level in WorldMap.LevelMap)
			{
				List<Contents> contentsToUpdate = new List<Contents>();
				LoadedLevel = level;
				if (level == null)
				{
					continue;
				}
				foreach (Tile tile in level.Grid.TileGrid)
				{
					Contents contents = tile.Contents;
					if (contents == null)
					{
						continue;
					}
					contentsToUpdate.Add(contents);
				}
				foreach (Contents updateContents in contentsToUpdate)
				{
					foreach (Action<Contents> behavior in updateContents.Behaviors)
					{
						behavior(updateContents);
					}
				}
			}
			LoadedLevel = realLoadedLevel;
			if (World.Player.Contents.Durability <= 0)
			{
				Output.WriteLineTagged("You've been defeated.", Output.Tag.World);
				Game.Execute = false;
			}
			Event.currentCallCount = 0;
		}

		// Returns the specific level that the player is in
		public static bool GetPlayerLevel(out Level result)
		{
			foreach (Level level in World.WorldMap.LevelMap)
			{
				if (level == null)
				{
					continue;
				}
				if (level.Grid.TryFindContentsFromName("Player", out _))
				{
					result = level;
					return true;
				}
			}
			result = null;
			return false;
		}

		public static bool GetContentsFromID(int id, out Contents result)
		{
			result = null;
			foreach (Level level in WorldMap.LevelMap)
			{
				foreach (Tile tile in level.Grid.TileGrid)
				{
					Contents contents = tile.Contents;
					if (contents == null)
					{
						continue;
					}
					if (contents.ID == id)
					{
						result = contents;
						return true;
					}
				}
			}

			return false;
		}

	}
}