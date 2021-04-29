using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;


namespace GameEngine
{
	// Where all world data is stored.
	static class World
	{
		// Coordinates are loaded into each tile & contents when they run through the Grid() constructor, so this is just used as a filler
		private static readonly Coord _zeroZero = new Coord(0, 0);

		// The player is the controllable character. It is set as a default only for the purpose of satisfying a default player for levelEditor
		public static Player Player = new Player(new Contents("Player", 0, 'A', true, 10, 10, 50, true, 100, new List<Contents>() { }, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing }), null, 5);

		// Tile templates used in tutorial and level editor palette
		#region tile definitions

		public static Floor Ground = new Floor('.', "ground");

		public static Tile Empty = new Tile(Ground, null, _zeroZero);

		public static Tile Wall = new Tile(Ground, new Contents("wall", 0, '#', false, 40, 10000, 400000, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile Fence = new Tile(Ground, new Contents("fence", 0, '+', true, 5, 10000, 60, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile Hog = new Tile(new Floor('~', "mudpit"), new Contents("hog", 0, 'H', true, 4, 40, 20, UseActions.DoesNothing, new Action<Contents>[] { Behavior.AttackClose, Behavior.Wander, Behavior.MoveTowardsPlayer }), _zeroZero);

		public static Tile Plant = new Tile(new Floor('Y', "plant"), null, _zeroZero);

		public static Tile Chicken = new Tile(Ground, new Contents("chicken", 0, '>', true, 1, 2, 5, UseActions.DoesNothing, new Action<Contents>[] { Behavior.Wander }), _zeroZero);

		public static Tile Rock = new Tile(Ground, new Contents("rock", 0, 'o', true, 4, 1, 3, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile Lever = new Tile(Ground, new Contents("lever", 0, 'L', true, 10, 234, 5151, UseActions.Lever, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

		public static Tile Target = new Tile(Ground, new Contents("target", 0, '@', true, 2, 1000, 1000, UseActions.DoesNothing, new Action<Contents>[] { Behavior.Target }), _zeroZero);

		public static Tile GunTile = new Tile(Ground, new Contents("gun", 0, 'r', true, 4, 4, 5, UseActions.Shoot, new Action<Contents>[] { Behavior.DoesNothing }), _zeroZero);

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
			foreach (FileInfo file in directory.GetFiles("*.xml"))
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
		// <worldFile>.xml -> WorldMap
		public static void LoadFromFile(string filePath)
		{
			Output.WriteLineToConsole("Loading file " + filePath + "...");
			if (!File.Exists(filePath))
			{
				Output.WriteLineTagged("File path is incorrect", Output.Tag.Error);
				return;
			}

			XmlReaderSettings readerSettings = new XmlReaderSettings();

			readerSettings.IgnoreWhitespace = true;

			FileStream fileStream = File.OpenRead(filePath);

			using (XmlReader xr = XmlReader.Create(fileStream, readerSettings))
			{
				xr.MoveToContent();

				xr.Read();

				xr.MoveToNextAttribute();

				string mapName = xr.Value;

				List<Level[]> levelGrid = new List<Level[]>();
				// Row of levels
				while (xr.Read())
				{
					if (xr.NodeType == XmlNodeType.EndElement)
					{
						break;
					}
					List<Level> levels = new List<Level>();
					// Level
					while (xr.Read())
					{
						if (xr.NodeType == XmlNodeType.EndElement)
						{
							break;
						}

						if (!xr.MoveToNextAttribute())
						{
							levels.Add(null);
							continue;
						}

						Level level = new Level(null, ' ', null, null, null, null, null, null);

						level.Name = xr.Value;

						xr.MoveToNextAttribute();
						level.VisualChar = xr.Value[0];

						xr.MoveToNextAttribute();
						bool loadedLevel = bool.Parse(xr.Value);

						xr.Read();
						Coord levelCoord = new Coord(0, 0);
						xr.MoveToNextAttribute();
						levelCoord.X = int.Parse(xr.Value);
						xr.MoveToNextAttribute();
						levelCoord.Y = int.Parse(xr.Value);

						level.LevelCoord = levelCoord;

						#region NESW entry

						// NorthEntry
						xr.Read();
						if (xr.HasAttributes)
						{
							xr.MoveToNextAttribute();
							level.NorthEntry = new Coord(0, 0);
							// X
							level.NorthEntry.X = int.Parse(xr.Value);
							// Y
							xr.MoveToNextAttribute();
							level.NorthEntry.Y = int.Parse(xr.Value);
							xr.Read();
						}

						// EastEntry
						xr.Read();
						if (xr.HasAttributes)
						{
							xr.MoveToNextAttribute();
							level.EastEntry = new Coord(0, 0);
							// X
							level.EastEntry.X = int.Parse(xr.Value);
							// Y
							xr.MoveToNextAttribute();
							level.EastEntry.Y = int.Parse(xr.Value);
							xr.Read();
						}

						// SouthEntry
						xr.Read();
						if (xr.HasAttributes)
						{
							xr.MoveToNextAttribute();
							level.SouthEntry = new Coord(0, 0);
							// X
							level.SouthEntry.X = int.Parse(xr.Value);
							// Y
							xr.MoveToNextAttribute();
							level.SouthEntry.Y = int.Parse(xr.Value);
							xr.Read();
						}

						// WestEntry
						xr.Read();
						if (xr.HasAttributes)
						{
							xr.MoveToNextAttribute();
							level.WestEntry = new Coord(0, 0);
							// X
							level.WestEntry.X = int.Parse(xr.Value);
							// Y
							xr.MoveToNextAttribute();
							level.WestEntry.Y = int.Parse(xr.Value);
							xr.Read();
						}
						#endregion

						// Grid
						xr.Read();

						List<Tile[]> tileGrid = new List<Tile[]>();
						// each row of tiles within the given level v
						while (xr.Read())
						{
							if (xr.NodeType == XmlNodeType.EndElement)
							{
								break;
							}
							List<Tile> tiles = new List<Tile>();
							// each tile within the given row v
							while (xr.Read())
							{
								if (xr.NodeType == XmlNodeType.EndElement)
								{
									break;
								}
								// Coords of the tile
								xr.Read();
								Coord tileCoord = new Coord(0, 0);
								xr.MoveToNextAttribute();
								tileCoord.X = int.Parse(xr.Value);
								xr.MoveToNextAttribute();
								tileCoord.Y = int.Parse(xr.Value);

								// Floor
								xr.Read();
								Floor tileFloor = new Floor(' ', "");
								xr.MoveToNextAttribute();
								tileFloor.Name = xr.Value;
								xr.MoveToNextAttribute();
								tileFloor.VisualChar = xr.Value[0];

								xr.Read();
								Contents tileContents = GetAllContents(xr)[0];

								tiles.Add(new Tile(tileFloor, tileContents, tileCoord));
							}
							tileGrid.Add(tiles.ToArray());
						}
						level.Grid = new Grid(tileGrid.ToArray());

						if (loadedLevel)
						{
							World.LoadedLevel = level;
						}
						xr.Read();
						levels.Add(level);
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

				xr.Read();

				xr.MoveToNextAttribute();
				Player.Strength = int.Parse(xr.Value);

				xr.Read();
				Coord playerCoords = new Coord(0, 0);

				xr.MoveToNextAttribute();
				playerCoords.X = int.Parse(xr.Value);

				xr.MoveToNextAttribute();
				playerCoords.Y = int.Parse(xr.Value);

				xr.Read();
				Player.Holding = GetAllContents(xr)[0];

				xr.Read();

				// TileIndex
				xr.Read();
				xr.Read();
				xr.Read();
				ContentsIndex = GetAllContents(xr);

				int count = ContentsIndex.Count;
				for (int index = 0; index < count; index++)
				{
					Contents contentsAtIndex = ContentsIndex[index];
					if (contentsAtIndex == null)
					{
						ContentsIndex.Remove(contentsAtIndex);
					}
				}

				// Dialogue

				xr.Read();
				xr.Read();

				if (xr.Name != "Connections")
				{
					do
					{
						if (xr.NodeType == XmlNodeType.EndElement)
						{
							break;
						}
						xr.MoveToNextAttribute();
						int id = int.Parse(xr.Value);
						xr.MoveToNextAttribute();
						Dialogue.Add(id, xr.Value);


					} while(xr.Read());
				}

				// Connections
				xr.Read();
				xr.Read();
				while (xr.Name != "Connections")
				{
					string currentEvent = xr.Name;

					while (xr.Read())
					{
						if (xr.Name != "Connection")
						{
							if (xr.Name == currentEvent)
							{
								xr.Read();
							}
							break;
						}

						xr.MoveToNextAttribute();
						int triggerID = int.Parse(xr.Value);
						xr.MoveToNextAttribute();
						int resultID = int.Parse(xr.Value);
						xr.MoveToNextAttribute();
						string resultType = xr.Value;
						xr.MoveToNextAttribute();
						string resultInformation = xr.Value;

						EventHandler.IdentifierEventMapping[currentEvent].ConnectionList.Add(new Connection(triggerID, resultID, resultType, resultInformation));
					}
				}
			}
			fileStream.Close();
		}

		// This is a child function of the one above. It recursively gets all contents, since a contents can contain a contents. 
		// This is the only reasonable way to get all contents (thus, the name)
		private static List<Contents> GetAllContents(XmlReader xr)
		{
			List<Contents> contentsList = new List<Contents>();
			if (!xr.HasAttributes)
			{
				contentsList.Add(null);
				xr.Read();
				return contentsList;
			}
			while (xr.Name == "Contents")
			{
				Contents contents = new Contents("", 0, ' ', true, 1, 1, 1f, UseActions.DoesNothing, new Action<Contents>[] { Behavior.DoesNothing });
				xr.MoveToNextAttribute();
				contents.Name = xr.Value;

				xr.MoveToNextAttribute();
				contents.ID = int.Parse(xr.Value);

				xr.MoveToNextAttribute();
				contents.VisualChar = xr.Value[0];

				xr.MoveToNextAttribute();
				contents.Transparent = bool.Parse(xr.Value);

				xr.MoveToNextAttribute();
				contents.Durability = int.Parse(xr.Value);

				xr.MoveToNextAttribute();
				contents.Size = int.Parse(xr.Value);
				
				xr.MoveToNextAttribute();
				contents.Weight = float.Parse(xr.Value);
				
				xr.MoveToNextAttribute();
				contents.Container = bool.Parse(xr.Value);
				
				xr.MoveToNextAttribute();
				contents.ContainerSpace = int.Parse(xr.Value);
				
				xr.MoveToNextAttribute();
				UseActions.TryGetAction(xr.Value, out Action<string[], Contents> action);
				contents.UseAction = action;
				
				xr.MoveToNextAttribute();
				Behavior.TryGetBehaviors(xr.Value.Split(','), out Action<Contents>[] behaviors);
				contents.Behaviors = behaviors;

				xr.Read();
				if (xr.HasValue)
				{
					contents.Contained = GetAllContents(xr);
				}

				xr.Read();

				xr.MoveToNextAttribute();
				contents.Tags = xr.Value.Split(',');

				xr.Read();
				xr.Read();
				contentsList.Add(contents);
			}
			return contentsList;
		}

		// This saves the world map into a file
		// WorldMap -> <worldFile>.xml
		public static void SaveToFile(string filePath)
		{
			XmlWriter xw;
			XmlWriterSettings xmlSettings = new XmlWriterSettings();

			xmlSettings.Indent = true;
			xmlSettings.NewLineOnAttributes = true;
			xmlSettings.IndentChars = "\t";

			try
			{
				xw = XmlWriter.Create(filePath, xmlSettings);
			}
			catch (Exception e)
			{
				Output.WriteLineTagged(e.Message, Output.Tag.Error);
				if (CommandInterpretation.AskYesNo("File path is invalid. Would you like to try again?"))
				{
					SaveToFile(CommandInterpretation.GetUserResponse("Enter file path"));
				}
				return;
			}
			Game.FilePath = filePath;

			Level[,] levelMap = WorldMap.LevelMap;
			xw.WriteComment("RPG-Engine XML world file");
			xw.WriteComment("Version No. " + Game.Version);
			xw.WriteComment(" - - - - - - - - - - - - - - - - - - - - ");

			xw.WriteWhitespace("\n\n");
			xw.WriteStartElement("World");
			xw.WriteStartElement("Map");
			xw.WriteAttributeString("Name", WorldMap.Name);
			for (int levelY = 0; levelY < levelMap.GetLength(1); levelY++)
			{
				xw.WriteStartElement("Row");
				xw.WriteAttributeString("Number", levelY.ToString());
				for (int levelX = 0; levelX < levelMap.GetLength(0); levelX++)
				{
					Level level = levelMap[levelX, levelY];
					if (level == null)
					{
						xw.WriteStartElement("Level");
						xw.WriteEndElement();
						continue;
					}
					// v This is on one line
					xw.WriteStartElement("Level");
					
					xw.WriteAttributeString("Name", level.Name);
					xw.WriteAttributeString("VisualChar", level.VisualChar.ToString());
					xw.WriteAttributeString("LoadedLevel", level.Equals(LoadedLevel).ToString());

					xw.WriteStartElement("LevelCoord", null);

					xw.WriteAttributeString("X", level.LevelCoord.X.ToString());
					xw.WriteAttributeString("Y", level.LevelCoord.Y.ToString());

					xw.WriteEndElement();
					
					#region entry points
					xw.WriteStartElement("NorthEntry");

					if (level.NorthEntry != null)
					{
						xw.WriteAttributeString("X", level.NorthEntry.X.ToString());
						xw.WriteAttributeString("Y", level.NorthEntry.Y.ToString());
					}

					xw.WriteEndElement();
					
					xw.WriteStartElement("EastEntry");
					if (level.EastEntry != null)
					{
						xw.WriteAttributeString("X", level.EastEntry.X.ToString());
						xw.WriteAttributeString("Y", level.EastEntry.Y.ToString());
					}
					
					xw.WriteEndElement();

					xw.WriteStartElement("SouthEntry");
					if (level.SouthEntry != null)
					{
						xw.WriteAttributeString("X", level.SouthEntry.X.ToString());
						xw.WriteAttributeString("Y", level.SouthEntry.Y.ToString());
					}
					
					xw.WriteEndElement();

					xw.WriteStartElement("WestEntry");
					if (level.WestEntry != null)
					{
						xw.WriteAttributeString("X", level.WestEntry.X.ToString());
						xw.WriteAttributeString("Y", level.WestEntry.Y.ToString());
					}
					
					xw.WriteEndElement();
					#endregion

					xw.WriteStartElement("Grid");

					for (int y = 0; y < level.Grid.TileGrid.GetLength(1); y++)
					{
						xw.WriteStartElement("Row");
						xw.WriteAttributeString("Number", y.ToString());
						for (int x = 0; x < level.Grid.TileGrid.GetLength(0); x++)
						{
							Tile tileAtCoords = level.Grid.TileGrid[x, y];
							xw.WriteStartElement("Tile");

							xw.WriteStartElement("TileCoords");

							xw.WriteAttributeString("X", x.ToString());
							xw.WriteAttributeString("Y", y.ToString());
							
							xw.WriteEndElement();

							xw.WriteStartElement("Floor");

							xw.WriteAttributeString("Name", tileAtCoords.Floor.Name);
							xw.WriteAttributeString("VisualChar", tileAtCoords.Floor.VisualChar.ToString());

							xw.WriteEndElement();
							
							ListAllContents(tileAtCoords.Contents, xw);

							xw.WriteEndElement();

						}
						xw.WriteEndElement();
					}
					xw.WriteEndElement();
					xw.WriteEndElement();
				}
				xw.WriteEndElement();
			}
			xw.WriteEndElement();

			xw.WriteStartElement("Player");

			xw.WriteAttributeString("Strength", Player.Strength.ToString());

			xw.WriteStartElement("Coordinates");

			xw.WriteAttributeString("X", Player.Contents.Coordinates.X.ToString());
			xw.WriteAttributeString("Y", Player.Contents.Coordinates.Y.ToString());

			xw.WriteEndElement();

			xw.WriteStartElement("Holding");

			ListAllContents(Player.Holding, xw);

			xw.WriteEndElement();

			xw.WriteEndElement();

			xw.WriteStartElement("TileIndex");

			foreach (Contents contents in ContentsIndex)
			{
				if (contents == null)
				{
					continue;
				}
				contents.Container = false;
				ListAllContents(contents, xw);
			}

			xw.WriteEndElement();

			xw.WriteStartElement("Dialogue");

			foreach (int key in Dialogue.Keys)
			{
				xw.WriteStartElement("Line");

				xw.WriteAttributeString("ID", key.ToString());
				xw.WriteAttributeString("Dialogue", Dialogue[key]);

				xw.WriteEndElement();
			}

			xw.WriteEndElement();

			xw.WriteStartElement("Connections");

			foreach (string key in EventHandler.IdentifierEventMapping.Keys)
			{
				xw.WriteStartElement(key);

				foreach (Connection connection in EventHandler.IdentifierEventMapping[key].ConnectionList)
				{
					xw.WriteStartElement("Connection");
					xw.WriteAttributeString("TriggerContentsID", connection.TriggerContentsID.ToString());
					xw.WriteAttributeString("ResultContentsID", connection.ResultContentsID.ToString());
					xw.WriteAttributeString("ResultType", connection.ResultType);
					xw.WriteAttributeString("ResultInformation", connection.ResultInformation);
					xw.WriteEndElement();
				}

				xw.WriteEndElement();
			}

			xw.WriteEndElement();

			xw.Close();

			Output.WriteLineToConsole("World file saved successfully as " + Game.FilePath);
			
		}

		// This is a child function of the one above. It does the inverse of GetAllContents
		private static void ListAllContents(Contents contents, XmlWriter xw)
		{
			xw.WriteStartElement("Contents");
			if (contents == null)
			{
				xw.WriteEndElement();
				return;
			}

			xw.WriteAttributeString("Name", contents.Name);
			xw.WriteAttributeString("ID", contents.ID.ToString());
			xw.WriteAttributeString("VisualChar", contents.VisualChar.ToString());
			xw.WriteAttributeString("Transparent", contents.Transparent.ToString());
			xw.WriteAttributeString("Durability", contents.Durability.ToString());
			xw.WriteAttributeString("Size", contents.Size.ToString());
			xw.WriteAttributeString("Weight", contents.Weight.ToString());
			xw.WriteAttributeString("Container", contents.Container.ToString());
			xw.WriteAttributeString("ContainerSpace", contents.ContainerSpace.ToString());


			if (UseActions.TryGetIdentifier(contents.UseAction, out string actionResult) && Behavior.TryGetIdentifiers(contents.Behaviors, out string behaviorResult))
			{
				xw.WriteAttributeString("UseAction", actionResult);
				xw.WriteAttributeString("Behavior", behaviorResult);
			}
			xw.WriteStartElement("Contained");
			if (contents.Container)
			{
				foreach (Contents contained in contents.Contained)
				{
					ListAllContents(contained, xw);
				}
			}
			xw.WriteEndElement();
			xw.WriteStartElement("Tags");

			xw.WriteAttributeString("Tags", string.Join(',', contents.Tags));

			xw.WriteEndElement();
			xw.WriteEndElement();
		}

		// This runs through and updates every Tile *AND* runs its behavior
		public static void UpdateWorld()
		{
			if (EventHandler.IdentifierEventMapping["OnUpdate"].RunEvent(0) == EventHandler.EventResult.TerminateAction)
			{
				return;
			}
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