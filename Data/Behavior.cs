using System;
using System.Collections.Generic;

namespace GameEngine
{
   static class Behavior // <<<<----- Make sure this is the right file you're editing; **this looks very similar to UseActions.cs**
   {
      // WARNING!!!! DO NOT REMOVE, SHUFFLE, OR CHANGE ANY OF THE CURRENT ACTIONS WITHIN THIS ARRAY
      // YOU MAY ADD ACTIONS, BUT BE SURE TO DO SO AT THE END OF EACH ARRAY
      // Please add a string that represents your command in the array: _identifiers
      public static Action<Contents>[] _customCommands = new Action<Contents>[]
      {
         DoesNothing,
         Wander,
         MonsterVictory,
         Aggressive,
         Target
      };
      public static string[] _identifiers = new string[]
      {
         "DoesNothing",
         "Wander",
         "MonsterVictory",
         "Aggressive",
         "Target"
      };

      public static bool TryGetAction(string givenIdentifier, out Action<Contents> result)
      {
         for (int identifierIndex = 0; identifierIndex < _identifiers.Length; identifierIndex++)
         {
            string identifier = (string)_identifiers[identifierIndex];
            if (identifier.Equals(givenIdentifier))
            {
               result = _customCommands[identifierIndex];
               return true;
            }
         }
         result = null;
         return false;
      }
      public static bool TryGetIdentifier(Action<Contents> givenAction, out string result)
      {
         for (int actionIndex = 0; actionIndex < _identifiers.Length; actionIndex++)
         {
            Action<Contents> action = _customCommands[actionIndex];
            if (action.Equals(givenAction))
            {
               result = _identifiers[actionIndex];
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

            Tile tileAtContents = World._loadedLevel._grid.GetTileAtCoords(coord.Add(contents._coordinates), false);
            if  (tileAtContents == null || tileAtContents._contents != null)
            {
               wanderCoords.Remove(coord);
               coordIndex--;
            }
         }
         wanderCoords.Add(new Coord(0, 0));

         World._loadedLevel._grid.MoveContents(contents, wanderCoords[rand.Next(0, wanderCoords.Count)]);
      }
      public static void MonsterVictory(Contents contents)
      {
         bool chickens = false;
         foreach (Contents contained in contents._contained)
         {
            if (contained._name == "chicken")
            {
               chickens = true;
            }
            else
            {
               Console.WriteLine("> EW!!! That's so gross!!! >:( that tasted so bad");
               contents._contained.RemoveAt(0);
            }
         }
         if (chickens)
         {
            Console.WriteLine("> Yummy! Thanks so much! That was delicious :)");
            if (contents._durability == 1)
            {
               Console.WriteLine("Monster explodes...");
            }
            else
            {
               contents._contained.RemoveAt(0);
               Console.WriteLine("> Could you please maybe bring me one more chicken? :(");
            }
            contents.Damage(1);
         }
      }
      public static void Aggressive(Contents contents)
      {
         Wander(contents);
         Coord distCoord = World._player.GetCoords().Subtract(contents._coordinates);
         if (World._loadedLevel._grid.TryFindContents(World._player._contents, out _) && (Math.Abs(distCoord._x) <= 1) && (Math.Abs(distCoord._y) <= 1))
         {
            World._player._contents.Damage(1);
         }
      }
      public static void Target(Contents contents)
      {
         if (contents._durability < 2)
         {
            foreach (Tile tile in World._loadedLevel._grid._tileGrid)
            {
               if (tile._contents != null)
               {
                  if (tile._contents._name == "gate")
                  {
                     tile._contents = null;
                     tile._floor = World._gateOpen._floor;
                  }
               }
               else if (tile._floor._name == "retractedGate")
               {
                  tile._contents = World._gateClosed._contents;
                  tile._contents._coordinates = tile._coordinates;
                  tile._floor = World._ground;
               }
            }
            contents._durability = 2;
         }
      }
   }
}
