using System;
using System.Collections.Generic;

namespace GameEngine
{
   // UseActions is a class that is defined when the player 'interact's with a tile or 'use's something in their hand
   static class UseActions // <<<<----- Make sure this is the right file you're editing; **this looks very similar to Behavior.cs**
   {
      // Array of Actions (see below the array of strings that represents the identifiers)
      public static (Action<string[], Contents> Action, string Identifier)[] CustomCommands = new (Action<string[], Contents>, string)[]
      {
         (DoesNothing,"DoesNothing"),
         (MonsterDialogue,"MonsterDialogue"),
         (Rude, "Rude"),
         (Lever, "Lever"),
         (Tombstone, "Tombstone"),
         (Boo, "Boo"),
         (Gun, "Gun"),
         (Dialogue, "Dialogue")
      };

      public static string[] GetIdentifiers()
      {
         List<string> identifiers = new List<string>();
         foreach ((Action<string[], Contents>, string) item in CustomCommands)
         {
            identifiers.Add(item.Item2);
         }
         return identifiers.ToArray();
      }
      
      // The following two functions make this work as a make-shift two-way Dictionary
      // It's highly dependant on the order of the two arrays staying fixed.

      // Returns the UseAction if its respective index of identifier exists.
      public static bool TryGetAction(string givenIdentifier, out Action<string[], Contents> result)
      {
         // Loops through, searching for a match. Returns true when it finds a match
         string[] identifiers = GetIdentifiers();
         for (int identifierIndex = 0; identifierIndex < identifiers.Length; identifierIndex++)
         {
                string identifier = identifiers[identifierIndex];
                if (identifier.Equals(givenIdentifier))
            {
               result = CustomCommands[identifierIndex].Action;
               return true;
            }
         }
         result = null;
         return false;
      }
      
      // Returns the identifier if its respective index of Action exists.
      public static bool TryGetIdentifier(Action<string[], Contents> givenAction, out string result)
      {
         // Loops through, searching for a match. Returns true when it finds a match
         for (int actionIndex = 0; actionIndex < CustomCommands.Length; actionIndex++)
         {
            Action<string[], Contents> action = CustomCommands[actionIndex].Action;
            if (action.Equals(givenAction))
            {
               result = CustomCommands[actionIndex].Identifier;
               return true;
            }
         }
         result = null;
         return false;
      }

      // All custom Use Actions
      public static void DoesNothing(string[] parameters, Contents contents)
      {
         Console.WriteLine("The object does nothing.");
      }
      public static void MonsterDialogue(string[] parameters, Contents contents)
      {
         Console.WriteLine("> Hello friend!!! I am very...hungry and need some chickens to eat. Can you bring some? Thanks friend!\nThey look like this!\t>");
      }
      public static void Rude(string[] parameters, Contents contents)
      {
         Random rand = new Random();
         switch (rand.Next(6))
         {
            case 0:
               Console.WriteLine("You're in my way!");
               break;
            case 1:
               Console.WriteLine("What are you looking at?");
               break;
            case 2:
               Console.WriteLine("Do I know you?");
               break;
            case 3:
               Console.WriteLine("Don't bug me!");
               break;
            default:
               break;
         }
      }
      public static void Lever(string[] parameters, Contents contents)
      {
         foreach (Tile tile in World.LoadedLevel.Grid.TileGrid)
         {
            if (tile.Contents != null)
            {
               if (tile.Contents.Name == "gate")
               {
                  tile.Contents = null;
                  tile.Floor = World.GateOpen.Floor;
               }
            }
            else if (tile.Floor.Name == "retractedGate")
            {
               tile.Contents = World.GateClosed.Contents;
               tile.Contents.Coordinates = tile.Coordinates;
               tile.Floor = World.Ground;
            }
         }
      }
      public static void Tombstone(string[] parameters, Contents contents)
      {
         Random rand = new Random();
         string[] names = { "Jerry", "Larry", "Bob", "Betsy", "You", "Nobody. We just had too many gravestones :)" };
         Console.WriteLine("R.I.P " + names[rand.Next(names.Length)]);
      }
      public static void Boo(string[] parameters, Contents contents)
      {
         Console.WriteLine("Boo! :)");
      }
      public static void Gun(string[] parameters, Contents contents)
      {
         if (World.Player.Holding.Name != "gun")
         {
            Console.WriteLine("You can't fire the gun while it's on the ground.");
            return;
         }
         string x;
         string y;

         if (parameters.Length > 1)
         {
            x = parameters[0];
            y = parameters[1];
         }
         else
         {
            x = CommandInterpretation.GetUserResponse("Please enter the x coordinate of where you would like to shoot");
            y = CommandInterpretation.GetUserResponse("Please enter the y coordinate of where you would like to shoot");
         }

         if (!CommandInterpretation.InterpretAlphaNum(x, y, out Coord targetCoord))
         {
            return;
         }

         Coord playerCoord = World.Player.GetCoords();
         if (!World.LoadedLevel.Grid.VisibleAtLine(playerCoord, new Coord(targetCoord.X - playerCoord.X, targetCoord.Y - playerCoord.Y)))
         {
            Console.WriteLine("You cannot see this tile from here. Try moving");
            return;
         }

         if (!World.LoadedLevel.Grid.GetTileAtCoords(targetCoord, out Tile tileAtCoords, false))
         {
            return;
         }

         if (tileAtCoords == null || tileAtCoords.Contents == null)
         {
            return;
         }
         tileAtCoords.Contents.Damage(1);
      }
      public static void Dialogue(string[] parameters, Contents contents)
      {
         if (World.Dialogue.TryGetValue(contents.Name, out string result))
         {
            Console.WriteLine(result);
         }
         else
         {
            Console.WriteLine("Error- Inconsistency. The contents: " + contents.Name + " has a dialogue UseAction, but is not mapped to any dialogue in the world file.");
         }
      }
   }
}
