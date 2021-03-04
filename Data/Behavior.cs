using System;
using System.Collections.Generic;

namespace GameEngine
{
   static class Behavior // <<<<----- Make sure this is the right file you're editing; **this looks very similar to UseActions.cs**
   {
      // WARNING!!!! DO NOT REMOVE, SHUFFLE, OR CHANGE ANY OF THE CURRENT ACTIONS WITHIN THIS ARRAY
      // YOU MAY ADD ACTIONS, BUT BE SURE TO DO SO AT THE END OF EACH ARRAY
      // Please add a string that represents your command in the array: Identifiers in addition to adding the Action to CustomCommands

      // Array of Actions (see below the array of strings that represents the identifiers)
      public static (Action<Contents> Behavior, string Identifier)[] CustomCommands = new (Action<Contents>, string)[]
      {
         (DoesNothing, "DoesNothing"),
         (Wander, "Wander"),
         (MonsterVictory, "MonsterVictory"),
         (Aggressive, "Aggressive"),
         (Target, "Target")
      };
      
      public static string[] GetIdentifiers()
      {
         List<string> identifiers = new List<string>();
         foreach ((Action<Contents>, string) item in CustomCommands)
         {
            identifiers.Add(item.Item2);
         }
         return identifiers.ToArray();
      }
      // The following two functions make this work as a make-shift two-way Dictionary
      // It's highly dependant on the order of the two arrays staying fixed.

      // Returns the behavior if its respective index of identifier exists.
      public static bool TryGetBehavior(string givenIdentifier, out Action<Contents> result)
      {
         // Loops through, searching for a match. Returns true when it finds a match
         string[] identifiers = GetIdentifiers();
         for (int identifierIndex = 0; identifierIndex < identifiers.Length; identifierIndex++)
         {
                string identifier = identifiers[identifierIndex];
                if (identifier.Equals(givenIdentifier))
            {
               result = CustomCommands[identifierIndex].Behavior;
               return true;
            }
         }
         result = null;
         return false;
      }
      
      // Returns the identifier if its respective index of Action exists.
      public static bool TryGetIdentifier(Action<Contents> givenAction, out string result)
      {
         // Loops through, searching for a match. Returns true when it finds a match
         for (int actionIndex = 0; actionIndex < CustomCommands.Length; actionIndex++)
         {
            Action<Contents> action = CustomCommands[actionIndex].Behavior;
            if (action.Equals(givenAction))
            {
               result = CustomCommands[actionIndex].Identifier;
               return true;
            }
         }
         result = null;
         return false;
      }

      public static string AllBehavior()
      {
         for(int idendifierIndex = 0; idendifierIndex < CustomCommands.Length; idendifierIndex++)
         {
            string identifier = CustomCommands[idendifierIndex].Identifier;
            Console.WriteLine(identifier);
         }
         return string.Empty;
      }
      
      // All custom Behaviors
      public static void DoesNothing(Contents contents)
      {

      }
      public static void Wander(Contents contents)
      {
         Random rand = new Random();

         List<Coord> wanderCoords = new List<Coord>()
         {
            new Coord(0, 1),
            new Coord(0, -1),
            new Coord(1, 0),
            new Coord(-1, 0)
         };
         
         for (int coordIndex = 0; coordIndex < wanderCoords.Count; coordIndex++)
         {
            Coord coord = wanderCoords[coordIndex];
            if (!World.LoadedLevel.Grid.GetTileAtCoords(coord.Add(contents.Coordinates), out Tile tileAtCoords, false))
            {
               return;
            }
            if  (tileAtCoords == null || tileAtCoords.Contents != null)
            {
               wanderCoords.Remove(coord);
               coordIndex--;
            }
         }
         wanderCoords.Add(new Coord(0, 0));

         World.LoadedLevel.Grid.MoveContents(contents, wanderCoords[rand.Next(0, wanderCoords.Count)]);
      }
      public static void MonsterVictory(Contents contents)
      {
         bool chickens = false;
         foreach (Contents contained in contents.Contained)
         {
            if (contained.Name == "chicken")
            {
               chickens = true;
            }
            else
            {
               Console.WriteLine("> EW!!! That's so gross!!! >:( that tasted so bad");
               contents.Contained.RemoveAt(0);
            }
         }
         if (chickens)
         {
            Console.WriteLine("> Yummy! Thanks so much! That was delicious :)");
            if (contents.Durability == 1)
            {
               Console.WriteLine("Monster explodes...");
            }
            else
            {
               contents.Contained.RemoveAt(0);
               Console.WriteLine("> Could you please maybe bring me one more chicken? :(");
            }
            contents.Damage(1);
         }
      }
      public static void Aggressive(Contents contents)
      {
         Wander(contents);
         Coord distCoord = World.Player.GetCoords().Subtract(contents.Coordinates);
         if (World.LoadedLevel.Grid.TryFindContents(World.Player.Contents, out _) && (Math.Abs(distCoord.X) <= 1) && (Math.Abs(distCoord.Y) <= 1))
         {
            World.Player.Contents.Damage(1);
         }
      }
      public static void Target(Contents contents)
      {
         if (contents.Durability < 2)
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
            contents.Durability = 2;
         }
      }
   }
}
