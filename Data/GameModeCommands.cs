using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    // This class contains all the functions and Commands listed together.
    static class GameModeCommands
    {

        // *******************FOR MODDING*******************
        // To create a gamemode, you need to add a CommandChoices (at the bottom of the file) and name it well
        // Add any custom commands you like by first defining a function with params of string[] parameters
        // You can also take any of the currently defined commands and use them in this gamemode
        // You don't have to use the string[] parameter, but just make sure it's there (it will be fed in as any parameters the user puts after the command)
        // Define your command and add it inside the #region Commands. You will need an identifier, help description, an array of strings that asks for each individual parameter...
        // ... a reference to the function, and finally whether or not the world should pass time after the function has ended.
        // Use other commands/functions for reference if you get stuck
        // **************************************************

        // just an empty string array. Used for reference for non-parameter commands
        private static string[] _emptyString = new string[0];
        
        // The list of all functions. 
        #region Functions
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
                if (!World.LoadedLevel.Grid.GetTileAtCoords(World.Player.GetCoords().Add(coord), out Tile givenTile, false))
                {
                    return;
                }
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
                if (!World.LoadedLevel.Grid.GetTileAtCoords(result.Add(World.Player.GetCoords()), out Tile givenTile, false))
                {
                    return;
                }
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
                if (!World.LoadedLevel.Grid.GetTileAtCoords(newCoords, out Tile tileAtCoords))
                {
                    return;
                }

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

        private static void ExitEngine(string[] parameters)
        {
            if (CommandInterpretation.AskYesNo("Would you like to save before exiting?"))
            {
                SaveEngine(_emptyString);
            }
            Game.Execute = false;
        }

        private static void ExitEditor(string[] parameters)
        {
            if (CommandInterpretation.AskYesNo("Would you like to save before exiting?"))
            {
                SaveEditor(_emptyString);
            }
            Game.Execute = false;
        }
        
        private static void SaveEngine(string[] parameters)
        {
            if (CommandInterpretation.AskYesNo("Would you like to save the file as a different name?"))
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

        private static void SaveEditor(string[] parameters)
        {
            if (World.GetPlayerLevel(out Level playerLevel))
            {
                World.LoadedLevel = playerLevel;
            }
            else
            {
                if(CommandInterpretation.AskYesNo("Player location is not defined. Would you like to indicate their location now?\nNot doing so will result in the overriding of a a tile"))
                {
                    Game.com.EvaluateCommand("player move");
                }
                else
                {
                    World.Player.Contents.Coordinates = new Coord(0, 0);
                    World.LoadedLevel = World.WorldMap.LevelMap[0, 0];
                    World.LoadedLevel.Grid.TileGrid[0, 0].Contents = World.Player.Contents;
                }
            }
            if (CommandInterpretation.AskYesNo("Would you like to save the file as a different name?"))
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
                if (!World.LoadedLevel.Grid.GetTileAtCoords(newCoords, out Tile tileAtCoords))
                {
                    return;
                }

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
                if (!World.LoadedLevel.Grid.VisibleAtLine(World.Player.GetCoords(), relativeCoord))
                {
                    Console.WriteLine("Tile is not visible!");
                    return;
                }

                Console.WriteLine("Observing tile " + paramters[0] + " " + paramters[1]);

                if (!World.LoadedLevel.Grid.GetTileAtCoords(result, out Tile lookTile))
                {
                    return;
                }

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
                if (!(UseActions.TryGetIdentifier(lookTile.Contents.UseAction, out string action) && Behavior.TryGetIdentifiers(lookTile.Contents.Behaviors, out string behavior)))
                {
                    return;
                }
                Console.WriteLine(" -- Contents: "  + lookTile.Contents.Name + " --\n");
                Console.WriteLine("Size: " + lookTile.Contents.Size);
                Console.WriteLine("Weight: " + lookTile.Contents.TotalWeight);
                Console.WriteLine("Durability: " + lookTile.Contents.Durability);

                Console.WriteLine("Transparent: " + lookTile.Contents.Transparent);
                Console.WriteLine("Container: " + lookTile.Contents.Container);

                Console.WriteLine("Use Action: " + action);
                Console.WriteLine("Behaviors: " + behavior);

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
                if (!World.LoadedLevel.Grid.GetTileAtCoords(newCoords, out Tile tileAtCoords))
                {
                    return;
                }

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

        private static void Level(string[] parameters)
        {
            Console.WriteLine("Current level is " + World.LoadedLevel.Name);
        }

        // Level editor commands
        private static void NewMap(string[] parameters)
        {
            string fileMouth;
            if (!World.TryGetMapsFolder(out fileMouth))
            {
                return;
            }
            string mapName = CommandInterpretation.GetUserResponse("Enter a name for this new map.");
            Game.FilePath = Path.Combine(fileMouth, Path.ChangeExtension(mapName, ".txt"));
            World.WorldMap = new Map(new Level[1, 1], mapName);
        }

        private static void Expand(string[] parameters)
        {
            if (!CommandInterpretation.InterpretDirection(parameters[0], out Coord direction))
            {
                return;
            }
            switch (Editor.EditorState)
            {
                case Editor.State.Map:

                    Level [,] expandedMap = new Level[World.WorldMap.LevelMap.GetLength(0) + Math.Abs(direction.X), World.WorldMap.LevelMap.GetLength(1) + Math.Abs(direction.Y)];

                    int xOffset = (direction.X == -1 ? 1 : 0);
                    int yOffset = (direction.Y == -1 ? 1 : 0);
                    for (int y = yOffset; y < World.WorldMap.LevelMap.GetLength(1) + yOffset; y++)
                    {
                        for(int x = xOffset; x < World.WorldMap.LevelMap.GetLength(0) + xOffset; x++)
                        {
                            expandedMap[x, y] = World.WorldMap.LevelMap[x - xOffset, y - yOffset];
                        }
                    }

                    World.WorldMap = new Map(expandedMap, World.WorldMap.Name);
                    break;

                case Editor.State.Level:

                    Tile[,] expandedGrid = new Tile[World.LoadedLevel.Grid.TileGrid.GetLength(0) + Math.Abs(direction.X), World.LoadedLevel.Grid.TileGrid.GetLength(1) + Math.Abs(direction.Y)];

                    for (int y = 0; y < expandedGrid.GetLength(1); y++)
                    {
                        bool makeEmpty = ((y == 0 && direction.Y == -1) || (y == expandedGrid.GetLength(1) - 1 && direction.Y == 1));
                        for (int x = 0; x < expandedGrid.GetLength(0); x++)
                        {
                            if (makeEmpty || ((direction.X == -1 && x == 0) || (direction.X == 1 && x == expandedGrid.GetLength(0) - 1)))
                            {
                                expandedGrid[x, y] = (Tile)World.Empty.Clone();
                            }
                            else
                            {
                                expandedGrid[x, y] = World.LoadedLevel.Grid.TileGrid[x - (direction.X == -1 ? 1 : 0), y - (direction.Y == -1 ? 1 : 0)];
                            }
                        }
                    }
                    World.LoadedLevel.Grid = new Grid(expandedGrid);

                    #region Updating entry points
                    int levelWidth = World.LoadedLevel.Grid.TileGrid.GetLength(0);
                    int levelHeight = World.LoadedLevel.Grid.TileGrid.GetLength(1);
                    World.LoadedLevel.WestEntry = new Coord(0, (levelHeight - 1) / 2);
                    World.LoadedLevel.EastEntry = new Coord(levelWidth - 1, (levelHeight - 1) / 2);
                    World.LoadedLevel.NorthEntry = new Coord((levelWidth - 1) / 2, 0);
                    World.LoadedLevel.SouthEntry = new Coord((levelWidth - 1) / 2, levelHeight - 1);
                    #endregion
                    break;
            }
        }
        
        private static void Shrink(string[] parameters)
        {
            if (!CommandInterpretation.InterpretDirection(parameters[0], out Coord direction))
            {
                return;
            }
            int newHeight = World.WorldMap.LevelMap.GetLength(1) - Math.Abs(direction.Y);
            int newWidth = World.WorldMap.LevelMap.GetLength(0) - Math.Abs(direction.X);
            if (newHeight < 1 || newWidth < 1)
            {
                Console.WriteLine("This grid is too small to shrink that way");
                return;
            }
            switch (Editor.EditorState)
            {
                case Editor.State.Map:

                    Level [,] shrunkenMap = new Level[newWidth, newHeight];

                    int xOffsetMap = (direction.X == -1 ? 1 : 0);
                    int yOffsetMap = (direction.Y == -1 ? 1 : 0);
                    for (int y = yOffsetMap; y < shrunkenMap.GetLength(1) + yOffsetMap; y++)
                    {
                        for(int x = xOffsetMap; x < shrunkenMap.GetLength(0) + xOffsetMap; x++)
                        {
                            shrunkenMap[x - xOffsetMap, y - yOffsetMap] = World.WorldMap.LevelMap[x, y];
                        }
                    }

                    World.WorldMap = new Map(shrunkenMap, World.WorldMap.Name);
                    break;

                case Editor.State.Level:

                    Tile[,] shrunkenGrid = new Tile[World.LoadedLevel.Grid.TileGrid.GetLength(0) - Math.Abs(direction.X), World.LoadedLevel.Grid.TileGrid.GetLength(1) - Math.Abs(direction.Y)];

                    int xOffsetGrid = (direction.X == -1 ? 1 : 0);
                    int yOffsetGrid = (direction.Y == -1 ? 1 : 0);
                    for (int y = yOffsetGrid; y < shrunkenGrid.GetLength(1) + yOffsetGrid; y++)
                    {
                        for(int x = xOffsetGrid; x < shrunkenGrid.GetLength(0) + xOffsetGrid; x++)
                        {
                            shrunkenGrid[x - xOffsetGrid, y - yOffsetGrid] = World.LoadedLevel.Grid.TileGrid[x, y];
                        }
                    }
                    World.LoadedLevel.Grid = new Grid(shrunkenGrid);

                    #region Updating entry points
                    int levelWidth = World.LoadedLevel.Grid.TileGrid.GetLength(0);
                    int levelHeight = World.LoadedLevel.Grid.TileGrid.GetLength(1);
                    World.LoadedLevel.WestEntry = new Coord(0, (levelHeight - 1) / 2);
                    World.LoadedLevel.EastEntry = new Coord(levelWidth - 1, (levelHeight - 1) / 2);
                    World.LoadedLevel.NorthEntry = new Coord((levelWidth - 1) / 2, 0);
                    World.LoadedLevel.SouthEntry = new Coord((levelWidth - 1) / 2, levelHeight - 1);
                    #endregion
                    break;
            }
        }

        private static void MakeBrush(string[] parameters)
        {
            if (Editor.EditorState == Editor.State.Map)
            {
                Console.WriteLine("You are not viewing any level currently");
                return;
            }
            if (CommandInterpretation.InterpretString(parameters[0], new string[] { "tile", "new" }, out string result))
            {
                switch (result)
                {
                    case "tile":
                        Coord coord;
                        if (!CommandInterpretation.InterpretAlphaNum(CommandInterpretation.GetUserResponse("Enter the x coordinate of the tile"), CommandInterpretation.GetUserResponse("Enter the y coordinate of the tile"), out coord))
                        {
                            return;
                        }
                        if (!World.LoadedLevel.Grid.GetTileAtCoords(coord, out Tile tileAtCoords, false))
                        {
                            return;
                        }
                        if (tileAtCoords == null)
                        {
                            return;
                        }
                        Editor.Brush = tileAtCoords;

                        break;
                    case "new":
                        if (!CommandInterpretation.InterpretTile(out Editor.Brush))
                        {
                            return;
                        }
                        break;
                }
            }
        }

        private static void SwitchMap(string[] parameters)
        {
            Editor.EditorState = Editor.State.Map;
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
                    CommandInterpretation.GetUserResponse("Enter a character (letter, number, or symbol) to represent this level")[0],
                    new Grid(new Tile[1,1] { { (Tile)World.Empty.Clone() } } ),
                    levelCoord,
                    null,
                    null,
                    null,
                    null);

                #region evaluating entry points
                Level result;
                if (World.WorldMap.GetLevelAtCoords(levelCoord.Add(new Coord(1, 0)), out result, false))
                {
                    if (result != null)
                    {
                        levelAtCoords.EastEntry = new Coord(0, 0);
                        result.WestEntry = new Coord(0, (result.Grid.TileGrid.GetLength(1) - 1) / 2);
                    }
                }
                if (World.WorldMap.GetLevelAtCoords(levelCoord.Add(new Coord(-1, 0)), out result, false))
                {
                    if (result != null)
                    {
                        levelAtCoords.WestEntry = new Coord(0, 0);
                        result.EastEntry = new Coord(result.Grid.TileGrid.GetLength(0) - 1, (result.Grid.TileGrid.GetLength(1) - 1) / 2);
                    }
                }
                if (World.WorldMap.GetLevelAtCoords(levelCoord.Add(new Coord(0, 1)), out result, false))
                {
                    if (result != null)
                    {
                        levelAtCoords.SouthEntry = new Coord(0, 0);
                        result.NorthEntry = new Coord((result.Grid.TileGrid.GetLength(0) - 1) / 2, 0);
                    }
                }
                if (World.WorldMap.GetLevelAtCoords(levelCoord.Add(new Coord(0, -1)), out result, false))
                {
                    if (result != null)
                    {
                        levelAtCoords.NorthEntry = new Coord(0, 0);
                        result.SouthEntry = new Coord((result.Grid.TileGrid.GetLength(0) - 1) / 2, result.Grid.TileGrid.GetLength(1) - 1);
                    }
                }
                #endregion
            }
            World.WorldMap.LevelMap[levelCoord.X, levelCoord.Y] = levelAtCoords;
            World.LoadedLevel = levelAtCoords;
            Editor.EditorState = Editor.State.Level;
        }

        private static void EditTile(string[] parameters)
        {
            if (Editor.EditorState == Editor.State.Map)
            {
                Console.WriteLine("You are not viewing any level currently");
                return;
            }
            if (!CommandInterpretation.InterpretAlphaNum(parameters[0], parameters[1], out Coord tileCoord))
            {
                return;
            }
            if (!World.LoadedLevel.Grid.GetTileAtCoords(tileCoord, out Tile tileAtCoords))
            {
                return;
            }
            do
            {
                string[] memberNames = new string[]
                {
                    // v 0
                    "(Floor) Name: " + tileAtCoords.Floor.Name,
                    // v 1
                    "(Floor) Visual Character: " + tileAtCoords.Floor.VisualChar.ToString(),
                    "Contents: " + (tileAtCoords.Contents != null ? tileAtCoords.Contents.Name : "null")
                };
                if (CommandInterpretation.InterpretString(memberNames, out string result))
                {
                    // Unfortunately, I can't use a switch statment here... UGH
                    if (result == memberNames[0])
                    {
                        if (!CommandInterpretation.InterpretString(result, out string tileName))
                        {
                            return;
                        }
                        tileAtCoords.Floor.Name = tileName;
                    }
                    else if (result == memberNames[1])
                    {
                        if (!CommandInterpretation.InterpretChar(result, out char visualChar))
                        {
                            return;
                        }
                        tileAtCoords.Floor.VisualChar = visualChar;
                    }
                    else
                    {
                        EditContents(ref tileAtCoords.Contents);
                    }
                }
            } while(CommandInterpretation.AskYesNo("Would you like to continue editing?"));

        }
        
        // v *THIS IS NOT A COMMAND; IT'S AN ADD-ON RECURSIVE FUNCTION NECESSARY FOR THE COMMAND ABOVE* ^
        private static void EditContents(ref Contents contents)
        {
            if (contents == null)
            {
                if (!CommandInterpretation.InterpretContents(out Contents newContents))
                {
                    return;
                }
                contents = newContents;
            }
            else
            {
                if (!UseActions.TryGetIdentifier(contents.UseAction, out string action))
                {
                    return;
                }
                if (!Behavior.TryGetIdentifiers(contents.Behaviors, out string behaviors))
                {
                    return;
                }
                string[] memberNames = new string[]
                {
                    // v 0
                    "(Contents) Name: " + contents.Name,
                    // v 1
                    "(Contents) Visual Character: " + contents.VisualChar,
                    // v 2
                    "(Contents) Transparent: " + contents.Transparent,
                    // v 3
                    "(Contents) Durability: " + contents.Durability,
                    // v 4
                    "(Contents) Size: " + contents.Size,
                    // v 5
                    "(Contents) Weight: " + contents.Weight,
                    // v 6
                    "(Contents) Use Action: " + action,
                    // v 7
                    "(Contents) Behaviors: " + behaviors,
                    // v 8
                    "(Contents) Container: " + contents.Container,
                    // v 9
                    "(Contents) Container Space: " + contents.ContainerSpace,
                    // v 10
                    "(Contents) Contained: " + (contents.Contained != null ? (contents.Contained.Count + " items.") : "No objects contained.")
                };

                
                if (!CommandInterpretation.InterpretString(memberNames, out string result))
                {
                    return;
                }

                if (result == memberNames[0])
                {
                    if (!CommandInterpretation.InterpretString(CommandInterpretation.GetUserResponse("Enter the name:"), out string contentsName))
                    {
                        return;
                    }
                    contents.Name = contentsName;
                }
                else if (result == memberNames[1])
                {
                    if (!CommandInterpretation.InterpretChar(CommandInterpretation.GetUserResponse("Enter the visual character:"), out char visualChar))
                    {
                        return;
                    }
                    contents.VisualChar = visualChar;
                }
                else if (result == memberNames[2])
                {
                    contents.Transparent = CommandInterpretation.AskYesNo(CommandInterpretation.GetUserResponse("Would you like this to be transparent?"));
                }
                else if (result == memberNames[3])
                {
                    if (!CommandInterpretation.InterpretInt(CommandInterpretation.GetUserResponse("Enter the durability:"), out int durability))
                    {
                        return;
                    }
                    contents.Durability = durability;
                }
                else if (result == memberNames[4])
                {
                    if (!CommandInterpretation.InterpretInt(CommandInterpretation.GetUserResponse("Enter the size:"), out int size))
                    {
                        return;
                    }
                    contents.Size = size;
                }
                else if (result == memberNames[5])
                {
                    if (!CommandInterpretation.InterpretFloat(CommandInterpretation.GetUserResponse("Enter the weight:"), out float weight))
                    {
                        return;
                    }
                    contents.Weight = weight;
                }
                else if (result == memberNames[6])
                {
                    Console.WriteLine("Enter the action:");
                    if (!(CommandInterpretation.InterpretString(UseActions.GetIdentifiers(), out string actionString) && UseActions.TryGetAction(actionString, out Action<string[], Contents> _action)))
                    {
                        return;
                    }
                    if (actionString == "Dialogue")
                    {
                        if (World.Dialogue.TryGetValue(contents.Name, out string dialogue))
                        {
                        Console.WriteLine("There is already a dialogue line for this content's name (give it a unique name to give it a unique dialogue)");
                        Console.WriteLine(dialogue);
                        }
                        else
                        {
                        Console.WriteLine("There is no current dialogue for this content's name. Please define it below");
                        dialogue = Console.ReadLine();
                        World.Dialogue.Add(contents.Name, dialogue);
                        }
                    }
                    contents.UseAction = _action;
                }
                else if (result == memberNames[7])
                {
                    Console.WriteLine("Enter the behavior:");
                    if (!(CommandInterpretation.InterpretStringMC(Behavior.GetIdentifiers(), out string[] behaviorString) && Behavior.TryGetBehaviors(behaviorString, out Action<Contents>[] _behavior)))
                    {
                        return;
                    }
                    contents.Behaviors = _behavior;
                }
                else if (result == memberNames[8])
                {
                    contents.Container = CommandInterpretation.AskYesNo(CommandInterpretation.GetUserResponse("Would you like this to be a container?"));
                }
                else if (result == memberNames[9])
                {
                    if (!CommandInterpretation.InterpretInt(CommandInterpretation.GetUserResponse("Enter the container space:"), out int space))
                    {
                        return;
                    }
                    contents.ContainerSpace = space;
                }
                else if (result == memberNames[10])
                {
                    Contents referenceContents;
                    Dictionary<string, Contents> nameContentsMapping = new Dictionary<string, Contents>();
                    foreach (Contents contained in contents.Contained)
                    {
                        nameContentsMapping.Add(contained.Name, contained);
                    }
                    if (!CommandInterpretation.InterpretString(nameContentsMapping.Keys.ToArray(), out string contentsName))
                    {
                        return;
                    }
                    referenceContents = nameContentsMapping[contentsName];
                    if (CommandInterpretation.InterpretString(new string[] { "delete", "edit", "duplicate" }, out string response))
                    {
                        switch (response)
                        {
                            case "delete":
                                contents.Contained.Remove(referenceContents);
                                break;
                            case "edit":
                                EditContents(ref referenceContents);
                                break;
                            case "duplicate":
                                contents.Contained.Add((Contents)referenceContents.Clone());
                                break;
                        }
                    }
                }
            }
        }

        private static void Draw(string[] parameters)
        {
            if (Editor.EditorState == Editor.State.Map)
            {
                Console.WriteLine("You are not viewing any level currently");
                return;
            }
            if (Editor.Brush == null)
            {
                Console.WriteLine("Brush is empty");
                return;
            }
            if (CommandInterpretation.InterpretString(new string[] { "rectangle", "box", "line", "point", "circle" }, out string result))
            {
                switch (result)
                {
                    case "rectangle":
                        if (!CommandInterpretation.InterpretAlphaNum(CommandInterpretation.GetUserResponse("Enter x coordinate of the starting tile"), CommandInterpretation.GetUserResponse("Enter y coordinate of the starting tile"), out Coord startCoordRect))
                        {
                            return;
                        }
                        if (!CommandInterpretation.InterpretAlphaNum(CommandInterpretation.GetUserResponse("Enter x coordinate of the ending tile"), CommandInterpretation.GetUserResponse("Enter y coordinate of the ending tile"), out Coord endCoordRect))
                        {
                            return;
                        }
                        for (int rectX = startCoordRect.X; rectX <= endCoordRect.X; rectX++)
                        {
                            for (int rectY = startCoordRect.Y; rectY <= endCoordRect.Y; rectY++)
                            {
                                World.LoadedLevel.Grid.SetTileAtCoords(new Coord(rectX, rectY), (Tile)Editor.Brush.Clone());
                            }
                        }
                        break;
                    case "box":
                        if (!CommandInterpretation.InterpretAlphaNum(CommandInterpretation.GetUserResponse("Enter x coordinate of the starting tile"), CommandInterpretation.GetUserResponse("Enter y coordinate of the starting tile"), out Coord startCoordBox))
                        {
                            return;
                        }
                        if (!CommandInterpretation.InterpretAlphaNum(CommandInterpretation.GetUserResponse("Enter x coordinate of the ending tile"), CommandInterpretation.GetUserResponse("Enter y coordinate of the ending tile"), out Coord endCoordBox))
                        {
                            return;
                        }
                        for (int boxX = startCoordBox.X; boxX <= endCoordBox.X; boxX++)
                        {
                            World.LoadedLevel.Grid.SetTileAtCoords(new Coord(boxX, startCoordBox.Y), (Tile)Editor.Brush.Clone());
                            World.LoadedLevel.Grid.SetTileAtCoords(new Coord(boxX, endCoordBox.Y), (Tile)Editor.Brush.Clone());
                        }
                        for (int boxY = startCoordBox.Y + 1; boxY < endCoordBox.Y; boxY++)
                        {
                            World.LoadedLevel.Grid.SetTileAtCoords(new Coord(startCoordBox.X, boxY), (Tile)Editor.Brush.Clone());
                            World.LoadedLevel.Grid.SetTileAtCoords(new Coord(endCoordBox.X, boxY), (Tile)Editor.Brush.Clone());
                        }
                        break;
                    case "line":
                        if (!CommandInterpretation.InterpretAlphaNum(CommandInterpretation.GetUserResponse("Enter x coordinate of the starting tile"), CommandInterpretation.GetUserResponse("Enter y coordinate of the starting tile"), out Coord startCoordLine))
                        {
                            return;
                        }
                        if (!CommandInterpretation.InterpretAlphaNum(CommandInterpretation.GetUserResponse("Enter x coordinate of the ending tile"), CommandInterpretation.GetUserResponse("Enter y coordinate of the ending tile"), out Coord endCoordLine))
                        {
                            return;
                        }

                        Coord relativeCoord = endCoordLine.Subtract(startCoordLine);

                        double lineX = startCoordLine.X + 0.5;
                        double lineY = startCoordLine.Y + 0.5;

                        double endingX = lineX + relativeCoord.X;
                        double endingY = lineY + relativeCoord.Y;

                        int xSign = relativeCoord.X == 0 ? 0 : (relativeCoord.X / Math.Abs(relativeCoord.X));
                        int ySign = relativeCoord.Y == 0 ? 0 : (relativeCoord.Y / Math.Abs(relativeCoord.Y));


                        double? slope;
                        if (relativeCoord.X != 0)
                        {
                            slope = (double)relativeCoord.Y / (double)relativeCoord.X;
                        }
                        else
                        {
                            for (; lineY != endingY; lineY += ySign)
                            {
                                World.LoadedLevel.Grid.SetTileAtCoords(new Coord((int)lineX, (int)lineY), (Tile)Editor.Brush.Clone());
                            }
                            return;
                        }
                        bool xMove = Math.Abs(relativeCoord.X) > Math.Abs(relativeCoord.Y);

                        if (xMove)
                        {
                            for (; lineX != endingX; lineX += xSign)
                            {
                                lineY = GetYTile(lineX - 0.1, (double)slope, startCoordLine);

                                double tryY = GetYTile(lineX + 0.1, (double)slope, startCoordLine);
                                if (tryY != lineY)
                                {
                                    World.LoadedLevel.Grid.SetTileAtCoords(new Coord((int)lineX, (int)tryY), (Tile)Editor.Brush.Clone());
                                }

                                World.LoadedLevel.Grid.SetTileAtCoords(new Coord((int)lineX, (int)lineY), (Tile)Editor.Brush.Clone());
                            }
                        }
                        else
                        {
                            for (; lineY != endingY; lineY += ySign)
                            {
                                lineX = GetXTile(lineY - 0.1, (double)slope, startCoordLine);

                                double tryX = GetXTile(lineY + 0.1, (double)slope, startCoordLine);
                                if (tryX != lineX)
                                {
                                    World.LoadedLevel.Grid.SetTileAtCoords(new Coord((int)tryX, (int)lineY), (Tile)Editor.Brush.Clone());
                                }
                                
                                World.LoadedLevel.Grid.SetTileAtCoords(new Coord((int)lineX, (int)lineY), (Tile)Editor.Brush.Clone());
                            }
                        }
                        break;
                    case "point":
                        if (!CommandInterpretation.InterpretAlphaNum(CommandInterpretation.GetUserResponse("Enter x coordinate"), CommandInterpretation.GetUserResponse("Enter y coordinate"), out Coord pointCoord))
                        {
                            return;
                        }
                        World.LoadedLevel.Grid.SetTileAtCoords(pointCoord, (Tile)Editor.Brush.Clone());
                        break;
                    case "circle":
                        if (!CommandInterpretation.InterpretAlphaNum(CommandInterpretation.GetUserResponse("Enter x coordinate"), CommandInterpretation.GetUserResponse("Enter y coordinate"), out Coord centerCoord))
                        {
                            return;
                        }
                        if (!CommandInterpretation.InterpretInt(CommandInterpretation.GetUserResponse("Enter radius"), out int radius))
                        {
                            return;
                        }
                        World.LoadedLevel.Grid.SetTileAtCoords(centerCoord, (Tile)Editor.Brush.Clone());
                        // Might not be the best way to implement this but it's the only nive way I can think of so far.
                        // Works with expanding box rings as long as there's still some parts of it that are within the distance (radius)
                        bool ringSmallEnough = true;
                        for (int distance = 1; ringSmallEnough; distance++)
                        {
                            ringSmallEnough = false;
                            Coord startCoordCircle = centerCoord.Add(new Coord(-1 * distance, -1 * distance));
                            Coord endCoordCircle = centerCoord.Add(new Coord(distance, distance));
                            Coord newCoord;
                            for (int x = startCoordCircle.X; x <= endCoordCircle.X; x++)
                            {
                                newCoord = new Coord(x, startCoordCircle.Y);
                                if (World.LoadedLevel.Grid.GetTileAtCoords(newCoord, out _, false) && centerCoord.Distance(newCoord) <= radius)
                                {
                                    World.LoadedLevel.Grid.SetTileAtCoords(newCoord, (Tile)Editor.Brush.Clone());
                                    ringSmallEnough = true;
                                }
                                newCoord = new Coord(x, endCoordCircle.Y);
                                if (World.LoadedLevel.Grid.GetTileAtCoords(newCoord, out _, false) && centerCoord.Distance(newCoord) <= radius)
                                {
                                    World.LoadedLevel.Grid.SetTileAtCoords(newCoord, (Tile)Editor.Brush.Clone());
                                    ringSmallEnough = true;
                                }
                            }
                            for (int y = startCoordCircle.Y + 1; y < endCoordCircle.Y; y++)
                            {
                                newCoord = new Coord(startCoordCircle.X, y);
                                if (World.LoadedLevel.Grid.GetTileAtCoords(newCoord, out _, false) && centerCoord.Distance(newCoord) <= radius)
                                {
                                    World.LoadedLevel.Grid.SetTileAtCoords(newCoord, (Tile)Editor.Brush.Clone());
                                    ringSmallEnough = true;
                                }
                                newCoord = new Coord(endCoordCircle.X, y);
                                if (World.LoadedLevel.Grid.GetTileAtCoords(newCoord, out _, false) && centerCoord.Distance(newCoord) <= radius)
                                {
                                    World.LoadedLevel.Grid.SetTileAtCoords(newCoord, (Tile)Editor.Brush.Clone());
                                    ringSmallEnough = true;
                                }
                            }
                        }
                        break;
                }
            }
        }
        
        // v THESE THREE FUNCTIONS ARE SUPPORTING THE ABOVE FUNCTION. THEY ARE NOT COMMMANDS
        private static double CenterTile(double input)
        {
            return (Math.Floor(input) + 0.5f);
        }
        private static double GetYTile(double x, double slope, Coord startCoord)
        {
            if (slope == 0)
            {
                return CenterTile(startCoord.Y);
            }
            return CenterTile(startCoord.Y + slope * (x + (0.5 / slope) - startCoord.X - 0.5));
        }
        private static double GetXTile(double y, double slope, Coord startCoord)
        {
            return CenterTile(startCoord.X + ((y + (slope * 0.5) - startCoord.Y - 0.5) / slope));
        }

        private static void ChangePlayer(string[] parameters)
        {
            if (CommandInterpretation.InterpretString(parameters[0], new string[] { "edit", "move" }, out string result))
            {
                switch (result)
                {
                    case "edit":
                        EditContents(ref World.Player.Contents);
                        break;
                    case "move":
                        Console.WriteLine(World.WorldMap.GraphicString());
                        Console.WriteLine("Enter coordinates of the level to send player");
                        if (!(CommandInterpretation.InterpretAlphaNum(out Coord levelCoord) && World.WorldMap.GetLevelAtCoords(levelCoord, out Level newPlayerLevel)))
                        {
                            return;
                        }
                        Console.WriteLine(newPlayerLevel.Grid.GraphicString());
                        Console.WriteLine("Enter coordinates of the tile to send player");
                        if (!(CommandInterpretation.InterpretAlphaNum(out Coord tileCoord) && newPlayerLevel.Grid.GetTileAtCoords(tileCoord, out Tile newPlayerTile)))
                        {
                            return;
                        }
                        if (World.GetPlayerLevel(out Level  playerLevel))
                        {
                            if (playerLevel.Grid.GetTileAtCoords(World.Player.GetCoords(), out Tile playerTile))
                            {
                                playerTile.Contents = null;
                            }
                        }
                        World.Player.Contents.Coordinates = tileCoord;
                        World.LoadedLevel = newPlayerLevel;
                        newPlayerLevel.Grid.TileGrid[tileCoord.X, tileCoord.Y].Contents = World.Player.Contents;
                        break;
                }
            }
        }
        
        #endregion

        // The list of all Commands.
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

        private static readonly Command _exitEngine = new Command(
            "exit",
            "Closes the program and gives you an option to save.",
            _emptyString,
            ExitEngine,
            false);
        private static readonly Command _saveEditor = new Command(
            "save",
            "Saves the current state of the game into a file.",
            _emptyString,
            SaveEditor,
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
        private static readonly Command _level = new Command(
            "level",
            "Provides info about the current level",
            _emptyString,
            Level,
            false);

        // Editor commands
        private static readonly Command _exitEditor = new Command(
            "exit",
            "Closes the program and gives you an option to save.",
            _emptyString,
            ExitEditor,
            false);
        private static readonly Command _saveEngine = new Command(
            "save",
            "Saves the current state of the game into a file.",
            _emptyString,
            SaveEngine,
            false);
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
        private static readonly Command _shrink = new Command(
            "shrink",
            "Shrinks the map or level, depending on the current state",
            new string[]
            {
                "Indicate the cardinal direction in which you would like to shrink."
            },
            Shrink,
            false);// TEST THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
        private static readonly Command _makeBrush = new Command(
            "brush",
            "Sets the brush for drawing with tiles",
            new string[]
            {
                "Type \"tile\" if you would like to pick a tile on the grid. Type \"new\" if you would like to make a new tile for the brush",
            },
            MakeBrush,
            false);
        private static readonly Command _editTile = new Command(
            "edit",
            "Edits a specific tile either of the map or of the level",
            new string[]
            {
                "Enter the x coordinate",
                "Enter the y coordinate"
            },
            EditTile,
            false);
        private static readonly Command _draw = new Command(
            "draw",
            "Draws with the brush",
            _emptyString,
            Draw,
            false);

        private static readonly Command _changePlayer = new Command(
            "player",
            "Edits the controllable player",
            new string[]
            {
                "Type either \"edit\" or \"move\""
            },
            ChangePlayer,
            false);
        #endregion

        // Each CommandChoices represents a gamemode
        public static CommandChoices EngineCommands = new CommandChoices(new List<Command>()
        {
            _remove,
            _move,
            _bag,
            _add,
            _drop,
            _exitEngine,
            _saveEngine,
            _engineHelp,
            _pick,
            _load,
            _look,
            _use,
            _map,
            _interact,
            _wait,
            _index,
            _level
         });
        public static CommandChoices TutorialCommands = new CommandChoices(new List<Command>()
        {
            _remove,
            _move,
            _bag,
            _add,
            _drop,
            _exitEngine,
            _tutorialHelp,
            _pick,
            _look,
            _use,
            _map,
            _interact,
            _wait,
            _index,
            _level
        });
        public static CommandChoices EditorCommands = new CommandChoices(new List<Command>()
        {
           _saveEditor,
           _load,
           _exitEditor,
           _levelEditorHelp,
           _newMap,
           _expand,
           _shrink,
           _switchMap,
           _focusLevel,
           _makeBrush,
           _editTile,
           _draw,
           _changePlayer
        });
    }
}