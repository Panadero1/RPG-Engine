using System;
using System.Collections.Generic;

namespace GameEngine
{
    static class GameModeCommands
    {
        #region Actions
        private static void Remove(string[] parameters)
        {
            if (World._player._holding != null)
            {
                Console.WriteLine("You're already holding something");
                return;
            }
            Contents container;
            if (parameters[0].Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                container = World._player._contents;
            }
            else if (CommandInterpretation.InterpretDirection(parameters[0], out Coord coord))
            {
                Tile givenTile = World._loadedLevel._grid.GetTileAtCoords(World._player.GetCoords().Add(coord));
                if (givenTile == null)
                {
                    return;
                }
                if (givenTile._contents == null)
                {
                    Console.WriteLine("There are no contents at this location");
                    return;
                }
                if (!givenTile._contents._container)
                {
                    Console.WriteLine("The contents at this tile is not a container");
                    return;
                }
                container = givenTile._contents;

            }
            else
            {
                Console.WriteLine("Parameter entered is wrong. You may enter a cardinal direction or 'self'");
                return;
            }

            if (container._contained.Count == 0)
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

            if (CommandInterpretation.InterpretInt(index, 0, container._contained.Count - 1, out int result))
            {
                Contents removedContents = container._contained[result];
                container._contained.RemoveAt(result);
                Console.WriteLine(removedContents._name + " was removed!");
                World._player._holding = removedContents;
            }
        }

        private static void Move(string[] parameters)
        {
            if (CommandInterpretation.InterpretDirection(parameters[0], out Coord addCoord))
            {
                World._loadedLevel._grid.MoveContents(World._player._contents, addCoord);

            }
        }

        private static void Bag(string[] parameters)
        {
            if (World._player._holding == null)
            {
                Console.WriteLine("You are not holding anything");
            }
            else
            {
                Console.WriteLine("You are currently holding " + World._player._holding._name);
            }
            Console.WriteLine(World._player._contents.ListContents());
        }

        private static void Add(string[] parameters)
        {
            if (World._player._holding == null)
            {
                Console.WriteLine("You aren't holding anything!");
                return;
            }
            Contents container;
            if (parameters[0].Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                container = World._player._contents;
            }
            else if (CommandInterpretation.InterpretDirection(parameters[0], out Coord result))
            {
                Tile givenTile = World._loadedLevel._grid.GetTileAtCoords(result.Add(World._player.GetCoords()));
                if (givenTile == null)
                {
                    return;
                }
                if (givenTile._contents == null)
                {
                    Console.WriteLine("There are no contents at this location");
                    return;
                }
                if (!givenTile._contents._container)
                {
                    Console.WriteLine("The contents at this tile is not a container");
                    return;
                }
                container = givenTile._contents;

            }
            else
            {
                Console.WriteLine("Parameter entered is wrong. You may enter a cardinal direction or 'self'");
                return;
            }

            Console.WriteLine(World._player._holding._name + " was added to " + container._name);
            container._contained.Add(World._player._holding);
            World._player._holding = null;
        }

        private static void Drop(string[] parameters)
        {
            if (World._player._holding == null)
            {
                Console.WriteLine("You're not holding anything.");
                return;
            }
            if (CommandInterpretation.InterpretDirection(parameters[0], out Coord result))
            {
                Coord newCoords = World._player.GetCoords().Add(result);
                Tile tileAtCoords = World._loadedLevel._grid.GetTileAtCoords(newCoords);

                if (tileAtCoords == null)
                {
                    Console.WriteLine("That tile does not exist");
                    return;
                }
                if (tileAtCoords._contents != null)
                {
                    Console.WriteLine("That tile is occupied.");
                    return;
                }
                else
                {
                    World._loadedLevel._grid.SetContentsAtCoords(newCoords, World._player._holding);
                    tileAtCoords._contents._coordinates = newCoords;

                    World._player._holding = null;
                }
            }
        }

        private static void Exit(string[] parameters)
        {
            Game._execute = false;
        }

        private static void Save(string[] parameters)
        {
            if (CommandInterpretation.InterpretYesNo("Would you like to save the file in a new location?"))
            {
                World.SaveToFile(CommandInterpretation.GetUserResponse("Please indicate the path of the file"));
            }
            else
            {
                World.SaveToFile(Game._filePath);
            }
        }

        private static void EngineHelp(string[] parameters)
        {
            if (_engineCommands.TryFindCommand(parameters[0], out Command result))
            {
                Console.WriteLine("\n" + result._helpText + "\n");
            }
        }
        
        private static void TutorialHelp(string[] parameters)
        {
            if (_tutorialCommands.TryFindCommand(parameters[0], out Command result))
            {
                Console.WriteLine("\n" + result._helpText + "\n");
            }
        }
        
        private static void LevelEditorHelp(string[] parameters)
        {
            if (_tutorialCommands.TryFindCommand(parameters[0], out Command result))
            {
                Console.WriteLine("\n" + result._helpText + "\n");
            }
        }

        private static void Pick(string[] parameters)
        {
            if (World._player._holding != null)
            {
                Console.WriteLine("You're already holding something! Either drop it or add it to your inventory");
                return;
            }
            if (CommandInterpretation.InterpretDirection(parameters[0], out Coord result))
            {
                Coord newCoords = World._player.GetCoords().Add(result);
                Tile tileAtCoords = World._loadedLevel._grid.GetTileAtCoords(newCoords);

                if (tileAtCoords == null)
                {
                    Console.WriteLine("That tile does not exist");
                    return;
                }
                if (tileAtCoords._contents == null)
                {
                    Console.WriteLine("There is nothing on that tile");
                    return;
                }
                if (World._player._strength < tileAtCoords._contents._totalWeight)
                {
                    Console.WriteLine("You're not strong enough to lift this");
                    return;
                }
                Console.WriteLine(tileAtCoords._contents._name + " was picked up");
                World._player._holding = tileAtCoords._contents;
                tileAtCoords._contents = null;
            }
        }

        private static void Load(string[] parameters)
        {
            World.LoadFromFile(parameters[0]);
        }

        private static void Look(string[] paramters)
        {
            if (CommandInterpretation.InterpretAlphaNum(paramters[0], paramters[1], out Coord result))
            {
                Coord relativeCoord = new Coord(result._x - World._player._contents._coordinates._x, result._y - World._player._contents._coordinates._y);
                if (!World._loadedLevel._grid.VisibleAtLine(relativeCoord))
                {
                    Console.WriteLine("Tile is not visible!");
                    return;
                }

                Console.WriteLine("Observing tile " + paramters[0] + " " + paramters[1]);

                Tile lookTile = World._loadedLevel._grid.GetTileAtCoords(result);

                if (lookTile == null)
                {
                    return;
                }

                Console.WriteLine("The floor appears to be " + lookTile._floor._name);
                if (lookTile._contents == null)
                {
                    Console.WriteLine("There is nothing on the tile.");
                    return;
                }
                Console.WriteLine("The contents at this tile has these statistics:");
                Console.WriteLine("Name: " + lookTile._contents._name);
                Console.WriteLine("Size: " + lookTile._contents._size);
                Console.WriteLine("Weight: " + lookTile._contents._totalWeight);
                Console.WriteLine("Temperature: " + lookTile._contents._temperature);
                Console.WriteLine("Melting point: " + lookTile._contents._meltingpoint);
                Console.WriteLine("Durability: " + lookTile._contents._durability);

                Console.WriteLine("Transparent: " + lookTile._contents._transparent);
                Console.WriteLine("Container: " + lookTile._contents._container);

                World._contentsIndex.Add(lookTile._contents);
            }
        }

        private static void Use(string[] parameters)
        {
            if (World._player._holding == null)
            {
                Console.WriteLine("You aren't holding anything");
                return;
            }
            World._player._holding._useAction(parameters, World._player._holding);
        }

        private static void Map(string[] parameters)
        {
            Console.WriteLine(World._worldMap.GraphicString());
        }

        private static void Interact(string[] parameters)
        {
            if (CommandInterpretation.InterpretDirection(parameters[0], out Coord result))
            {
                Coord newCoords = World._player.GetCoords().Add(result);
                Tile tileAtCoords = World._loadedLevel._grid.GetTileAtCoords(newCoords);

                if (tileAtCoords == null)
                {
                    Console.WriteLine("That tile does not exist");
                    return;
                }
                if (tileAtCoords._contents == null)
                {
                    Console.WriteLine("You cannot interact with the floor..");
                    return;
                }
                else
                {
                    tileAtCoords._contents._useAction(parameters, tileAtCoords._contents);
                }
            }
        }

        private static void Wait(string[] parameters)
        {

        }

        private static void Index(string[] parameters)
        {
            if (World._contentsIndex.Count == 0)
            {
                Console.WriteLine("There are no current discovered contents. Use the 'look' command to discover new tiles & contents.");
                return;
            }
            int longestName = 0;
            foreach (Contents contents in World._contentsIndex)
            {
                int nameLength = contents._name.Length;
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

            foreach (Contents contents in World._contentsIndex)
            {
                string contentsName = contents._name;
                Console.Write(contentsName);
                for (int space = contentsName.Length; space <= longestName; space++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(contents._visualChar);
            }
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
            emptyString,
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
            emptyString,
            Exit,
            false);

        private static readonly Command _save = new Command(
            "save",
            "Saves the current state of the game into a file.",
            emptyString,
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
            new string[]
            {
                "Please enter the full path of your world file"
            },
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
            emptyString,
            Use,
            true);

        private static readonly Command _map = new Command(
            "map",
            "Displays the map",
            emptyString,
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
            emptyString,
            Wait,
            true);

        private static readonly Command _index = new Command(
            "index",
            "Displays all learned contents",
            emptyString,
            GameModeCommands.Index,
            false);
        #endregion

        private static string[] emptyString = new string[0];
        public static CommandChoices _engineCommands = new CommandChoices(new List<Command>()
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
        public static CommandChoices _tutorialCommands = new CommandChoices(new List<Command>()
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
        public static CommandChoices _editorCommands = new CommandChoices(new List<Command>()
        {
           _levelEditorHelp
        });
    }
}