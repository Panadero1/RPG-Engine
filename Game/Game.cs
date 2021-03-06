﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
	// The main class of the game. Contains references for each gamemode
	static class Game
	{
		// TODO: documentation so people understand what's happening in this chaos, epic demo world
		public static bool Execute = true;
		public static string FilePath;

		public const string Version = "v0.6.4";

		public static Dictionary<string, (Action action, CommandChoices commands)> GameModes = new Dictionary<string, (Action, CommandChoices)>() 
		{
			// For new gamemodes, add a new entry to this table
			// { "newGameMode", (ReferenceToAFunction, GameModeCommands.CommandChoiceForThisMode) }
			{ "Game", (GameEngine, GameModeCommands.EngineCommands) },
			{ "Tutorial", (Tutorial, GameModeCommands.TutorialCommands)},
			{ "Level Editor", (LevelEditor, GameModeCommands.EditorCommands) },
			{ "Show maps folder", (ShowMaps, null)},
			{ "Exit", (Exit, null) },
		};

		public static string GameMode;

		// When a gamemode is loaded, com is defined by GameModes (^)
		public static CommandChoices Com;

		// Always runs. Prompts the user to enter a gamemode and runs that specific gamemode
		static void Main(string[] args)
		{
			do
			{
				ClearEverything();

				Output.WriteLineToConsole("\n\nWelcome to RPG Engine (still in development)");

				Output.WriteLineToConsole("Version: " + Version + "\n");

				string result;
				while (!CommandInterpretation.InterpretString(GameModes.Keys.ToArray(), out result))
				{
					if (!CommandInterpretation.AskYesNo("Could not identify gamemode. Would you like to try again?"))
					{
						return;
					}
				}
				GameMode = result;
				Com = GameModes[result].commands;
				GameModes[result].action();
			} while (GameMode != "Exit");
		}
		
		private static void ClearEverything()
		{
			foreach (string key in EventHandler.IdentifierEventMapping.Keys)
			{
				EventHandler.IdentifierEventMapping[key].ConnectionList.Clear();
			}
			GameMode = null;
			World.WorldMap = null;
			Execute = true;
			World.Dialogue = new Dictionary<int, string>();
			World.ContentsIndex = new List<Contents>();

			Event.currentCallCount = 0;
			Contents.uniqueIndex = 1;
		}

		// Gamemode-specific functions

		// Base game
		static void GameEngine()
		{
			if (!World.LoadFromFile())
			{
				return;
			}

			Output.WriteLineToConsole("Type \"exit\" at any time to close the game");
			Output.WriteLineToConsole("Type \"help\" to view a list of commands");

			for (Output.WriteLineToConsole(World.LoadedLevel.Grid.GraphicString()); Execute; Output.WriteLineToConsole(World.LoadedLevel.Grid.GraphicString()))
			{
				Output.WriteLineTagged("Holding: " + (World.Player.Holding == null ? "Nothing" : World.Player.Holding.Name), Output.Tag.World);
				if (Com.EvaluateCommand(CommandInterpretation.GetUserResponse("Enter command:")))
				{
					World.UpdateWorld();
				}
			}
		}
		
		// Built-in tutorial
		static void Tutorial()
		{

			string[] lines = new string[]
			{
				// v 0
				"Hello! Welcome to the game. The first thing you will need to do is open your eyes.",
				// v 1
				"Great. Well, actually the game does that for you... Let's learn about commands.\nCommands are how you interact with this world\nTry typing something random into the console when prompted.",
				// v 2
				"So that didn't do anything... obviously. Let's learn about the format that the console *can* take and interpret.\nCommands start with a command keyword and can then be followed by parameters.\nLet's start with the 'wait' command. It is used to skip a turn. Type that when prompted.",
				// v 3
				"The 'wait' command does not take any parameters, so just typing it is enough.\nFor commands with parameters, you can separate them with spaces (you have to put them in the right order) or enter them one at a time into the console if you forget which is which\n",
				// v 4
				"Now this world is very confusing... You see this 'A' in the middle of the grid? That's what you control. Let's try looking around.\nA nice command you'll need to learn is 'look'.\nUse the 'look' command at a coordinate within your vision.\nYou will be prompted to enter the x and y coordinates in this specific format: '<letter><number> <letter><number>'.\nEg. 'look A2 A4'.\nLetters are required to help with coordinates on larger levels\n\nLook at four or five different tiles now.",
				// v 5
				"You can use the 'index' command to review all the contents you have seen in this world.",
				// v 6
				"If you haven't already, look at the tile represented by the 'r', then use the 'move' and 'pick' commands to go next to, and grab it.\nRemember, if you're ever unaware of the parameters for a command, just type the identifier and you will be prompted for each parameter one-at-a-time.",
				// v 7
				"You just picked up a gun and it's now in your hand. You need to shoot the gun at the target to be able to progress.\n\nMake use of the 'use' command to fire the gun. The command takes coordinates as parameters, similarly to the 'look' command.",
				// v 8
				"When you shoot the target, it destroys the gates to your east, which will allow you to pass through to the next level area.\nKeep moving east to go to the next level",
				// v 9
				"This is the next level. When you walk off the side of one level and another one exists in that direction, the new one will be loaded in.\n\nUse the 'map' command to check your map. \nThe 'A' represents where you are in the larger world map.",
				// v 10
				"Use your newfound skills and the 'interact' command to figure out this next level.",
				// v 11
				"The chest marked by 'C' has some coins in it. Use the 'remove' and 'add' commands to take all the coins out of it and put them into your bag. \n\nYou will need to get the gun out of your hands first. Either 'add' it to your inventory or 'drop' it on the ground\n\nYou can check what's in your inventory at any time by typing 'bag'",
				// v 12
				"Nice work! You've solidified the basic skills needed to understand and manipualte your surroundings.\nRemember to use 'help' if you need a refresher on what commands to use."
			};
			World.Dialogue = new Dictionary<int, string>()
			{
				{ 22, "Treasure is past these gates, but beware! Flip that lever and you will release the beast!!!\n\nTry to trap it then shoot it from afar."}
			};

			World.WorldMap = World.TutorialLevel;
			World.LoadedLevel = World.WorldMap.LevelMap[0, 0];

			World.Player.Contents = World.LoadedLevel.Grid.TileGrid[2, 2].Contents;

			int index = 0;

			WriteNextLine(lines, ref index);

			for (Output.WriteLineToConsole(World.LoadedLevel.Grid.GraphicString()); Execute; Output.WriteLineToConsole(World.LoadedLevel.Grid.GraphicString()))
			{
				Output.WriteLineTagged("Holding: " + (World.Player.Holding == null ? "Nothing" : World.Player.Holding.Name + " | Health: " + World.Player.Contents.Durability), Output.Tag.World);
				TutorialProgression(lines, ref index);

				if (Com.EvaluateCommand(CommandInterpretation.GetUserResponse("Enter command:")))
				{
					World.UpdateWorld();
				}
			}
		}
		
		// Child function of Tutorial() for writing specific info to guide the user
		private static void WriteNextLine(string[] lines, ref int index)
		{
			Output.WriteLineTagged(lines[index], Output.Tag.Tutorial);
			Output.WriteLineTagged("\nPress any key to continue.\n", Output.Tag.Prompt);
			Console.ReadKey(true);
			index++;
		}
		
		// Child function of Tutorial() for determining when it progresses.
		private static void TutorialProgression(string[] lines, ref int index)
		{
			switch (index)
			{
				case 1:
					WriteNextLine(lines, ref index);
					break;
				case 2:
					WriteNextLine(lines, ref index);
					break;
				case 3:
					WriteNextLine(lines, ref index);
					WriteNextLine(lines, ref index);
					break;
				case 5:
					if (World.ContentsIndex.Count > 3)
					{
						WriteNextLine(lines, ref index);
					}
					break;
				case 6:
					WriteNextLine(lines, ref index);
					break;
				case 7:
					if (World.Player.Holding != null && World.Player.Holding.Name == "gun")
					{
						WriteNextLine(lines, ref index);
					}
					break;
				case 8:
					WriteNextLine(lines, ref index);
					break;
				case 9:
					if (World.LoadedLevel == World.TutorialLevel.LevelMap[1, 0])
					{
						WriteNextLine(lines, ref index);
					}
					break;
				case 10:
					WriteNextLine(lines, ref index);
					break;
				case 11:
					if (World.LoadedLevel == World.TutorialLevel.LevelMap[1, 0] && !World.LoadedLevel.Grid.TryFindContentsFromName("hog", out _))
					{
						WriteNextLine(lines, ref index);
					}
					break;
				case 12:
					int coins = 0;
					foreach (Contents contents in World.Player.Contents.Contained)
					{
						if (contents.Name == "coin")
						{
							coins++;
						}
					}
					if (coins > 2)
					{
						WriteNextLine(lines, ref index);
					}
					break;
				default:
					Output.WriteLineTagged("That is the end of the tutorial now. For more information on commands, type 'help'. Type 'exit' to leave", Output.Tag.Tutorial);
					break;
			}
		}

		// Level editor gamemode
		static void LevelEditor()
		{
			Editor.Brush = null;
			Editor.EditorState = Editor.State.Map;
			if (CommandInterpretation.InterpretString(new string[] { "new", "load" }, out string result))
			{
				Com.EvaluateCommand(result);
			}
			else
			{
				if (CommandInterpretation.AskYesNo("That is not a valid choice. Would you like to try again?"))
				{
					LevelEditor();
				}
				return;
			}
			if (World.WorldMap == null)
			{
				return;
			}
			while (Execute)
			{
				switch (Editor.EditorState)
				{
					case Editor.State.Map:
						Output.WriteLineToConsole(World.WorldMap.GraphicString());
						break;
					case Editor.State.Level:
						Output.WriteLineToConsole(World.LoadedLevel.Grid.GraphicString(false));
				Output.WriteLineTagged("Brush: " + (Editor.Brush == null ? "Nothing" : (Editor.Brush.Contents == null ? Editor.Brush.Floor.Name : Editor.Brush.Contents.Name)), Output.Tag.World);
						break;
				}
				Com.EvaluateCommand(CommandInterpretation.GetUserResponse("Enter command: "));
			}
		}
		
		// For exiting
		static void Exit()
		{

		}

		// For showing the maps folder
		static void ShowMaps()
		{
			if (!World.TryGetMapsFolder(out string folderPath))
			{
				return;
			}
			
			Output.WriteLineToConsole("Maps folder path: " + folderPath);
		}

	}
}
