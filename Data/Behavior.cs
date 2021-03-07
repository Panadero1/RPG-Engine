using System;
using System.Collections.Generic;

namespace GameEngine
{
   // The Behavior class is used to determine specific actions that a contents may take upon the world updating
   // It also has useful functions used in the saving & loading processes
   static class Behavior // <<<<----- Make sure this is the right file you're editing; **this looks very similar to UseActions.cs**
   {
      // Array of Actions (see below the array of strings that represents the identifiers)
      public static (Action<Contents> Behavior, string Identifier)[] CustomCommands = new (Action<Contents>, string)[]
      {
         (DoesNothing, "DoesNothing"),
         (Wander, "Wander"),
         (MonsterVictory, "MonsterVictory"),
         (AttackClose, "AttackClose"),
         (AttackMed, "AttackMed"),
         (AttackFar, "AttackFar"),
         (MoveTowardsPlayer, "MoveTowardsPlayer"),
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
      public static bool TryGetBehaviors(string[] givenIdentifiers, out Action<Contents>[] result)
      {
         List<Action<Contents>> allBehaviors = new List<Action<Contents>>();
         string[] identifiers = GetIdentifiers();
         foreach(string givenIdentifier in givenIdentifiers)
         {
            for (int identifierIndex = 0; identifierIndex < identifiers.Length; identifierIndex++)
            {
               string identifier = identifiers[identifierIndex];
               if (identifier.Equals(givenIdentifier))
               {
                  allBehaviors.Add(CustomCommands[identifierIndex].Behavior);
                  break;
               }
            }
         }
         result = allBehaviors.ToArray();
         return true;
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

      public static bool TryGetIdentifiers(Action<Contents>[] givenActions, out string result)
      {
         result = "";
         // Loops through, searching for a match. Returns true when it finds a match
         foreach (Action<Contents> givenAction in givenActions)
         {
            for (int actionIndex = 0; actionIndex < CustomCommands.Length; actionIndex++)
            {
               Action<Contents> action = CustomCommands[actionIndex].Behavior;
               if (action.Equals(givenAction))
               {
                  result += CustomCommands[actionIndex].Identifier + ",";
                  break;
               }
            }
         }
         result = result.Substring(0, result.Length - 1);
         return true;
      }

      // All custom Behaviors.. will be more fleshed out later
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
               Output.WriteLineTagged("EW!!! That's so gross!!! >:( that tasted so bad", Output.tag.Dialogue);
               contents.Contained.RemoveAt(0);
            }
         }
         if (chickens)
         {
            Output.WriteLineTagged("Yummy! Thanks so much! That was delicious :)", Output.tag.Dialogue);
            if (contents.Durability == 1)
            {
               Output.WriteLineTagged("Monster explodes...", Output.tag.Dialogue);
            }
            else
            {
               contents.Contained.RemoveAt(0);
               Output.WriteLineTagged("> Could you please maybe bring me one more chicken? :(", Output.tag.Dialogue);
            }
            contents.Damage(1);
         }
      }
      public static void AttackClose(Contents contents)
      {
         if (!(World.GetPlayerLevel(out Level playerLevel) && playerLevel.Equals(World.LoadedLevel)))
         {
            return;
         }
         if (World.Player.GetCoords().Distance(contents.Coordinates) <= 1)
         {
            World.Player.Contents.Damage(1);
         }
      }
      public static void AttackMed(Contents contents)
      {
         if (!(World.GetPlayerLevel(out Level playerLevel) && playerLevel.Equals(World.LoadedLevel)))
         {
            return;
         }
         if (World.Player.GetCoords().Distance(contents.Coordinates) <= 2)
         {
            World.Player.Contents.Damage(1);
         }
      }
      public static void AttackFar(Contents contents)
      {
         if (!(World.GetPlayerLevel(out Level playerLevel) && playerLevel.Equals(World.LoadedLevel)))
         {
            return;
         }
         if (World.Player.GetCoords().Distance(contents.Coordinates) <= 3)
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
      public static void MoveTowardsPlayer(Contents contents)
      {
         if (!(World.GetPlayerLevel(out Level playerLevel) && playerLevel.Equals(World.LoadedLevel)))
         {
            return;
         }
         Coord playerCoords = World.Player.GetCoords();
         if (!World.LoadedLevel.Grid.VisibleAtLine(contents.Coordinates, playerCoords.Subtract(contents.Coordinates)))
         {
            return;
         }

         Coord relativeCoord = playerCoords.Subtract(contents.Coordinates);
         bool yMove = (Math.Abs(relativeCoord.Y) > Math.Abs(relativeCoord.X));
         Coord currentMoveCoord;
         if (relativeCoord.X == 0)
         {
            if (relativeCoord.Y == 0)
            {
               return;
            }
            else
            {
               int ySign = relativeCoord.Y / Math.Abs(relativeCoord.Y);
               currentMoveCoord = new Coord(0, ySign);
            }
         }
         else if (relativeCoord.Y == 0)
         {
            int xSign = relativeCoord.X / Math.Abs(relativeCoord.X);
            currentMoveCoord = new Coord(xSign, 0);
         }
         else
         {
            int ySign = relativeCoord.Y / Math.Abs(relativeCoord.Y);
            int xSign = relativeCoord.X / Math.Abs(relativeCoord.X);
            currentMoveCoord = yMove ? new Coord(0, ySign) : new Coord(xSign, 0);
         }

         if (!(World.LoadedLevel.Grid.GetTileAtCoords(contents.Coordinates.Add(currentMoveCoord), out Tile tileAtCoords, false)) && tileAtCoords.Contents != null)
         {
            return;
         }
         World.LoadedLevel.Grid.MoveContents(contents, currentMoveCoord, false);

      }

   }
}
