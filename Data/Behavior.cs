using System;
using System.Collections.Generic;

namespace GameEngine
{
   static class Behavior // <<<<----- Make sure this is the right file you're editing; **this looks very similar to UseActions.cs**
   {
      // WARNING!!!! DO NOT REMOVE, SHUFFLE, OR CHANGE ANY OF THE CURRENT ACTIONS WITHIN THIS ARRAY
      // YOU MAY ADD ACTIONS, BUT BE SURE TO DO SO AT THE END OF EACH ARRAY
      // Please add a string that represents your command in the array: _identifiers
      public static Action<Contents>[] CustomCommands = new Action<Contents>[]
      {
         DoesNothing,
         Wander,
         MonsterVictory,
         Aggressive,
         Target
      };
      public static string[] Identifiers = new string[]
      {
         "DoesNothing",
         "Wander",
         "MonsterVictory",
         "Aggressive",
         "Target"
      };

      public static bool TryGetAction(string givenIdentifier, out Action<Contents> result)
      {
         for (int identifierIndex = 0; identifierIndex < Identifiers.Length; identifierIndex++)
         {
            string identifier = (string)Identifiers[identifierIndex];
            if (identifier.Equals(givenIdentifier))
            {
               result = CustomCommands[identifierIndex];
               return true;
            }
         }
         result = null;
         return false;
      }
      public static bool TryGetIdentifier(Action<Contents> givenAction, out string result)
      {
         for (int actionIndex = 0; actionIndex < Identifiers.Length; actionIndex++)
         {
            Action<Contents> action = CustomCommands[actionIndex];
            if (action.Equals(givenAction))
            {
               result = Identifiers[actionIndex];
               return true;
            }
         }
         result = null;
         return false;
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

            Tile tileAtContents = World.LoadedLevel.Grid.GetTileAtCoords(coord.Add(contents.Coordinates), false);
            if  (tileAtContents == null || tileAtContents.Contents != null)
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
