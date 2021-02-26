using System;
using System.IO;
using System.Collections.Generic;

namespace GameEngine
{
    static class GameModeCommands
    {
        private static string[] _emptyString = new string[0];

        #region Actions
        private static void Remove(string[] parameters)
        {
            if (World.Player.Holding != null)
            {
                Console.WriteLine("You're already holding something");
                return;
            }
            Contents container;
            if (parameters[0].Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                container = World.Player.Contents;
            }
            else if (CommandInterpretation.InterpretDirection(parameters[0], out Coord coord))
            {
                Tile givenTile = World.LoadedLevel.Grid.GetTileAtCoords(World.Player.GetCoords().Add(coord));
                if (givenTile == null)
                {
                    return;
                }
                if (givenTile.Contents == null)
                {
                    Console.WriteLine("There are no contents at this location");
                    return;
                }
                if (!givenTile.Contents.Container)
                {
                    Console.WriteLine("The contents at this tile is not a container");
                    return;
                }
                container = givenTile.Contents;

            }
            else
            {
                Console.WriteLine("Parameter entered is wrong. You may enter a cardinal direction or 'self'");
                return;
            }

            if (container.Contained.Count == 0)
            {
                Console.WriteLine("There are no items in this container.");
                return;
            }
            string index;
            // parameters.Length will never be less than 1
            if (parameters.Length == 1)
            {
                index = CommandInterpretation.GetUserResponse("Please indicate the index of the item from which you want an object removed" + container.ListContents());
            }
            else
            {
                index = parameters[1];
            }

            if (CommandInterpretation.InterpretInt(index, 0, container.Contained.Count - 1, out int result))
            {
                Contents removedContents = container.Contained[result];
                container.Contained.RemoveAt(result);
                Console.WriteLine(removedContents.Name + " was removed!");
                World.Player.Holding = removedContents;
            }
        }

        private static void Move(string[] parameters)
        {
            if (CommandInterpretation.InterpretDirection(parameters[0], out Coord addCoord))
            {
                World.LoadedLevel.Grid.MoveContents(World.Player.Contents, addCoord);

            }
        }

        private static void Bag(string[] parameters)
        {
            if (World.Player.Holding == null)
            {
                Console.WriteLine("You are not holding anything");
            }
            else
            {
                Console.WriteLine("You are currently holding " + World.Player.Holding.Name);
            }
            Console.WriteLine(World.Player.Contents.ListContents());
        }

        private static void Add(string[] parameters)
        {
            if (World.Player.Holding == null)
            {
                Console.WriteLine("You aren't holding anything!");
                return;
            }
            Contents container;
            if (parameters[0].Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                container = World.Player.Contents;
            }
            else if (CommandInterpretation.InterpretDirection(parameters[0], out Coord result))
            {
                Tile givenTile = World.LoadedLevel.Grid.GetTileAtCoords(result.Add(World.Player.GetCoords()));
                if (givenTile == null)
                {
                    return;
                }
                if (givenTile.Contents == null)
                {
                    Console.WriteLine("There are no contents at this location");
                    return;
                }
                if (!givenTile.Contents.Container)
                {
                    Console.WriteLine("The contents at this tile is not a container");
                    return;
                }
                container = givenTile.Contents;

            }
            else
            {
                Console.WriteLine("Parameter entered is wrong. You may enter a cardinal direction or 'self'");
                return;
            }

            Console.WriteLine(World.Player.Holding.Name + " was added to " + container.Name);
            container.Contained.Add(World.Player.Holding);
            World.Player.Holding = null;
        }

        private static void Drop(string[] parameters)
        {
            if (World.Player.Holding == null)
            {
                Console.WriteLine("You're not holding anything.");
                return;
            }
            if (CommandInterpretation.InterpretDirection(parameters[0], out Coord result))
            {
                Coord newCoords = World.Player.GetCoords().Add(result);
                Tile tileAtCoords = World.LoadedLevel.Grid.GetTileAtCoords(newCoords);

                if (tileAtCoords == null)
                {
                    Console.WriteLine("That tile does not exist");
                    return;
                }
                if (tileAtCoords.Contents != null)
                {
                    Console.WriteLine("That tile is occupied.");
                    return;
                }
                else
                {
                    World.LoadedLevel.Grid.SetContentsAtCoords(newCoords, World.Player.Holding);
                    tileAtCoords.Contents.Coordinates = newCoords;

                    World.Player.Holding = null;
                }
            }
        }

        private static void Exit(string[] parameters)
        {
            if (CommandInterpretation.InterpretYesNo("Would you like to save before exiting?"))
            {
                Save(_emptyString);
            }
            Game.Execute = false;
        }

        private static void Save(string[] parameters)
        {
            if (CommandInterpretation.InterpretYesNo("Would you like to save the file in a new location?"))
            {
                string response = CommandInterpretation.GetUserResponse("Please indicate the name of the file");
                Path.ChangeExtension(response, ".txt");

                World.SaveToFile(Path.Combine(Game.FilePath, response));
            }
            else
            {
                World.SaveToFile(Game.FilePath);
            }
        }

        private static void EngineHelp(string[] parameters)
        {
            if (EngineCommands.TryFindCommand(parameters[0], out Command result))
            {
                Console.WriteLine("\n" + result.HelpText + "\n");
            }
        }
        
        private static void TutorialHelp(string[] parameters)
        {
            if (TutorialCommands.TryFindCommand(parameters[0], out Command result))
            {
                Console.WriteLine("\n" + result.HelpText + "\n");
            }
        }
        
        private static void LevelEditorHelp(string[] parameters)
        {
            if (EditorCommands.TryFindCommand(parameters[0], out Command result))
            {
                Console.WriteLine("\n" + result.HelpText + "\n");
            }
        }

        private static void Pick(string[] parameters)
        {
            if (World.Player.Holding != null)
            {
                Console.WriteLine("You're already holding something! Either drop it or add it to your inventory");
                return;
            }
            if (CommandInterpretation.InterpretDirection(parameters[0], out Coord result))
            {
                Coord newCoords = World.Player.GetCoords().Add(result);
                Tile tileAtCoords = World.LoadedLevel.Grid.GetTileAtCoords(newCoords);

                if (tileAtCoords == null)
                {
                    Console.WriteLine("That tile does not exist");
                    return;
                }
                if (tileAtCoords.Contents == null)
                {
                    Console.WriteLine("There is nothing on that tile");
                    return;
                }
                if (World.Player.Strength < tileAtCoords.Contents.TotalWeight)
                {
                    Console.WriteLine("You're not strong enough to lift this");
                    return;
                }
                Console.WriteLine(tileAtCoords.Contents.Name + " was picked up");
                World.Player.Holding = tileAtCoords.Contents;
                tileAtCoords.Contents = null;
            }
        }

        private static void Load(string[] parameters)
        {
            World.LoadFromFile();
        }

        private static void Look(string[] paramters)
        {
            if (CommandInterpretation.InterpretAlphaNum(paramters[0], paramters[1], out Coord result))
            {
                Coord relativeCoord = new Coord(result.X - World.Player.Contents.Coordinates.X, result.Y - World.Player.Contents.Coordinates.Y);
                if (!World.LoadedLevel.Grid.VisibleAtLine(relativeCoord))
                {
                    Console.WriteLine("Tile is not visible!");
                    return;
                }

                Console.WriteLine("Observing tile " + paramters[0] + " " + paramters[1]);

                Tile lookTile = World.LoadedLevel.Grid.GetTileAtCoords(result);

                if (lookTile == null)
                {
                    return;
                }

                Console.WriteLine("The floor appears to be " + lookTile.Floor.Name);
                if (lookTile.Contents == null)
                {
                    Console.WriteLine("There is nothing on the tile.");
                    return;
                }
                Console.WriteLine("The contents at this tile has these statistics:");
                Console.WriteLine("Name: " + lookTile.Contents.Name);
                Console.WriteLine("Size: " + lookTile.Contents.Size);
                Console.WriteLine("Weight: " + lookTile.Contents.TotalWeight);
                Console.WriteLine("Temperature: " + lookTile.Contents.Temperature);
                Console.WriteLine("Melting point: " + lookTile.Contents.MeltingPoint);
                Console.WriteLine("Durability: " + lookTile.Contents.Durability);

                Console.WriteLine("Transparent: " + lookTile.Contents.Transparent);
                Console.WriteLine("Container: " + lookTile.Contents.Container);

                World.ContentsIndex.Add(lookTile.Contents);
            }
        }

        private static void Use(string[] parameters)
        {
            if (World.Player.Holding == null)
            {
                Console.WriteLine("You aren't holding anything");
                return;
            }
            World.Player.Holding.UseAction(parameters, World.Player.Holding);
        }

        private static void Map(string[] parameters)
        {
            Console.WriteLine(World.WorldMap.GraphicString());
        }

        private static void Interact(string[] parameters)
        {
            if (CommandInterpretation.InterpretDirection(parameters[0], out Coord result))
            {
                Coord newCoords = World.Player.GetCoords().Add(result);
                Tile tileAtCoords = World.LoadedLevel.Grid.GetTileAtCoords(newCoords);

                if (tileAtCoords == null)
                {
                    Console.WriteLine("That tile does not exist");
                    return;
                }
                if (tileAtCoords.Contents == null)
                {
                    Console.WriteLine("You cannot interact with the floor..");
                    return;
                }
                else
                {
                    tileAtCoords.Contents.UseAction(parameters, tileAtCoords.Contents);
                }
            }
        }

        private static void Wait(string[] parameters)
        {

        }

        private static void Index(string[] parameters)
        {
            if (World.ContentsIndex.Count == 0)
            {
                Console.WriteLine("There are no current discovered contents. Use the 'look' command to discover new tiles & contents.");
                return;
            }
            int longestName = 0;
            foreach (Contents contents in World.ContentsIndex)
            {
                int nameLength = contents.Name.Length;
                if (nameLength > longestName)
                {
                    longestName = nameLength;
                }
            }
            longestName++;
            Console.Write("\nName");

            for (int space = 4; space <= longestName; space++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("Symbol");

            foreach (Contents contents in World.ContentsIndex)
            {
                string contentsName = contents.Name;
                Console.Write(contentsName);
                for (int space = contentsName.Length; space <= longestName; space++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(contents.VisualChar);
            }
        }

        private static void NewMap(string[] parameters)
        {
            string fileMouth;
            if (!World.TryGetMapsFolder(out fileMouth))
            {
                return;
            }
            string mapName = CommandInterpretation.GetUserResponse("Enter a name for this new map.");
            Game.FilePath = Path.Combine(fileMouth, Path.ChangeExtension(mapName, ".txt"));
            World.WorldMap = new Map(new Level[1,1], mapName);
        }

        private static void Expand(string[] parameters)
        {
            Coord direction;
            if (!CommandInterpretation.InterpretDirection(parameters[0], out direction))
            {
                return;
            }
            switch (Game.EditorState)
            {
                case Game._editorState.Map:

                    Level [,] expandedMap = new Level[World.WorldMap.LevelMap.GetLength(0) + Math.Abs(direction.X), World.WorldMap.LevelMap.GetLength(1) + Math.Abs(direction.Y)];

                    for (int y = (direction.Y == -1 ? 0 : 1); y < World.WorldMap.LevelMap.GetLength(1); y++)
                    {
                        for(int x = (direction.X == -1 ? 0 : 1); x < World.WorldMap.LevelMap.GetLength(0); x++)
                        {
                            expandedMap[x, y] = World.WorldMap.LevelMap[x, y];
                        }
                    }

                    World.WorldMap = new Map(expandedMap, World.WorldMap.Name);
                    break;

                case Game._editorState.Level:

                    Grid expandedGrid = new Grid(new Tile[World.LoadedLevel.Grid.TileGrid.GetLength(0) + direction.X, World.LoadedLevel.Grid.TileGrid.GetLength(1) + direction.Y]);

                    for (int y = (direction.Y == -1 ? 0 : 1); y < World.LoadedLevel.Grid.TileGrid.GetLength(1); y++)
                    {
                        for (int x = (direction.X == -1 ? 0 : 1); x < World.LoadedLevel.Grid.TileGrid.GetLength(1); x++)
                        {
                            expandedGrid.TileGrid[x, y] = World.LoadedLevel.Grid.TileGrid[x, y];
                        }
                    }
                    World.LoadedLevel.Grid = expandedGrid;

                    #region Updating entry points
                    int levelWidth = World.LoadedLevel.Grid.TileGrid.GetLength(0);
                    int levelHeight = World.LoadedLevel.Grid.TileGrid.GetLength(1);
                    World.LoadedLevel.WestEntry = new Coord(0, (levelHeight - 1) / 2);
                    World.LoadedLevel.EastEntry = new Coord(levelWidth - 1, (levelHeight - 1) / 2);
                    World.LoadedLevel.NorthEntry = new Coord((levelWidth - 1) / 2, 0);
                    World.LoadedLevel.NorthEntry = new Coord((levelWidth - 1) / 2, levelHeight - 1);
                    #endregion
                    break;
            }
        }
        
        private static void ViewMap(string[] parameters)
        {
            Console.WriteLine(World.WorldMap.GraphicString());
        }

        private static void SwitchMap(string[] parameters)
        {
            Game.EditorState = Game._editorState.Map;
        }

        private static void FocusLevel(string[] parameters)
        {
            Coord levelCoord;
            Level levelAtCoords;
            if (!Coord.FromAlphaNum(parameters[0], parameters[1], out levelCoord))
            {
                return;
            }
            if (!World.WorldMap.GetLevelAtCoords(levelCoord, out levelAtCoords))
            {
                return;
            }
            if (levelAtCoords == null)
            {
                Console.WriteLine("This level is empty. We are going to create a new one here.");
                levelAtCoords = new Level(
                    CommandInterpretation.GetUserResponse("Enter a name for this level"),
                    CommandInterpretation.GetUserResponse("Enter a character (letter or number) to represent this level")[0],
                    new Grid(new Tile[1,1]),
                    levelCoord,
                    null,
                    null,
                    null,
                    null);

                #region evaluating entry points
                Level result;
                if (World.WorldMap.GetLevelAtCoords(levelCoord.Add(new Coord(1, 0)), out result))
                {
                    if (result != null)
                    {
                        levelAtCoords.EastEntry = new Coord(0, 0);
                    }
                }
                if (World.WorldMap.GetLevelAtCoords(levelCoord.Add(new Coord(-1, 0)), out result))
                {
                    if (result != null)
                    {
                        levelAtCoords.WestEntry = new Coord(0, 0);
                    }
                }
                if (World.WorldMap.GetLevelAtCoords(levelCoord.Add(new Coord(0, 1)), out result))
                {
                    if (result != null)
                    {
                        levelAtCoords.SouthEntry = new Coord(0, 0);
                    }
                }
                if (World.WorldMap.GetLevelAtCoords(levelCoord.Add(new Coord(0, -1)), out result))
                {
                    if (result != null)
                    {
                        levelAtCoords.NorthEntry = new Coord(0, 0);
                    }
                }
                #endregion
            }
            World.LoadedLevel = levelAtCoords;
            Game.EditorState = Game._editorState.Level;
        }

        #endregion

        #region Commands
        private static Command _remove = new Command(
            "remove",
            "Removes one item from a container and places it in your hand. Does not work if your hands are already full.",
            new string[]
            {
                "Indicate the cardinal direction from your player to the container. Type 'self' if you want to remove an item from yourself."
            },
            Remove,
            true);

        private static Command _move = new Command(
            "move",
            "Moves your character to an adjacent tile in one of the four cardinal directions.",
            new string[]
            {
                "Type the cardinal direction where you want to move"
            },
            Move,
            true);

        private static readonly Command _bag = new Command(
            "bag",
            "Displays the contents of your bag and what you are holding.",
            _emptyString,
            Bag,
            false);

        private static readonly Command _add = new Command(
            "add",
            "Adds the item in your hand into either your inventory or the inventory of something adjacent to you",
            new string[]
            {
                "Indicate the cardinal direction from your player to the container. Type 'self' if you want to put the item you're holding in your bag."
            },
            Add,
            true);

        private static readonly Command _drop = new Command(
            "drop",
            "Drops the item in your hand to an adjacent tile in a cardinal direction.",
            new string[]
            {
                "Enter the cardinal direction in which you want to drop the item you are holding"
            },
            Drop,
            true);

        private static readonly Command _exit = new Command(
            "exit",
            "Closes the program and gives you an option to save.",
            _emptyString,
            Exit,
            false);

        private static readonly Command _save = new Command(
            "save",
            "Saves the current state of the game into a file.",
            _emptyString,
            Save,
            false);

        private static readonly Command _engineHelp = new Command(
            "help",
            "Displays the help index for each command",
            new string[]
            {
                ""
            },
            EngineHelp,
            false);

        private static readonly Command _tutorialHelp = new Command(
            "help",
            "Displays the help index for each command",
            new string[]
            {
                ""
            },
            TutorialHelp,
            false);
         private static readonly Command _levelEditorHelp = new Command(
            "help",
            "Displays the help index for each command",
            new string[]
            {
                ""
            },
            LevelEditorHelp,
            false);

        private static readonly Command _pick = new Command(
            "pick",
            "Picks an item off the ground",
            new string[]
            {
                "Enter the cardinal direction in which you want to pick up an item"
            },
            Pick,
            true);

        private static readonly Command _load = new Command(
            "load",
            "Loads a file from a file path",
            _emptyString,
            Load,
            false);

        private static readonly Command _look = new Command(
            "look",
            "Observes the coordinates of a visible tile. Enter coordiantes in alphanum format (eg. A0 A0)",
            new string[]
            {
                "Enter the 'x' coordinate of the tile (<letter><number>)",
                "Enter the 'y' coordinate of the tile (<letter><number>)"
            },
            Look,
            true);

        private static readonly Command _use = new Command(
            "use",
            "Uses the item currently in your hand",
            _emptyString,
            Use,
            true);

        private static readonly Command _map = new Command(
            "map",
            "Displays the map",
            _emptyString,
            GameModeCommands.Map,
            false);

        private static readonly Command _interact = new Command(
            "interact",
            "Interacts with an adjacent content",
            new string[]
            {
                "Enter the cardinal direction in which you want to interact"
            },
            Interact,
            true);

        private static readonly Command _wait = new Command(
            "wait",
            "Skips one action",
            _emptyString,
            Wait,
            true);

        private static readonly Command _index = new Command(
            "index",
            "Displays all learned contents",
            _emptyString,
            GameModeCommands.Index,
            false);

        // Editor commands
        private static readonly Command _newMap = new Command(
            "new",
            "creates a new world file",
            _emptyString,
            NewMap,
            false);
        private static readonly Command _expand = new Command(
            "expand",
            "Expands the map or level, depending on the current state",
            new string[]
            {
                "Indicate the cardinal direction in which you would like to expand."
            },
            Expand,
            false);
        private static readonly Command _switchMap = new Command(
            "map",
            "Switches to the map view",
            _emptyString,
            SwitchMap,
            false);
        private static readonly Command _focusLevel = new Command(
            "level",
            "Switches to a view of a specific level",
            new string[]
            {
                "Enter the x coordinate of the level that you would like to load.",
                "Enter the y coordinate of the level that you would like to load.",
            },
            FocusLevel,
            false);
        #endregion

        public static CommandChoices EngineCommands = new CommandChoices(new List<Command>()
        {
            _remove,
            _move,
            _bag,
            _add,
            _drop,
            _exit,
            _save,
            _engineHelp,
            _pick,
            _load,
            _look,
            _use,
            _map,
            _interact,
            _wait,
            _index
         });
        public static CommandChoices TutorialCommands = new CommandChoices(new List<Command>()
        {
            _remove,
            _move,
            _bag,
            _add,
            _drop,
            _exit,
            _tutorialHelp,
            _pick,
            _look,
            _use,
            _map,
            _interact,
            _wait,
            _index
        });
        public static CommandChoices EditorCommands = new CommandChoices(new List<Command>()
        {
           _save,
           _load,
           _exit,
           _levelEditorHelp,
           _newMap,
           _expand,
           _switchMap,
           _focusLevel
        });
    }
}