using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace GameEngine
{
   static class World
   {
      private static readonly Coord _zeroZero = new Coord(0, 0);

      public static Player Player = new Player(new Contents("Player", 'A', true, 50, 10, 50, true, 100, new List<Contents>(), UseActions.DoesNothing, Behavior.DoesNothing), null, 50);

      #region tile definitions

      public static Floor Ground = new Floor('.', "ground");

      public static Tile Empty = new Tile(Ground, null, _zeroZero);

      public static Tile Mud = new Tile(new Floor('~', "mud"), null, _zeroZero);

      public static Tile Wall = new Tile(Ground, new Contents("wall", '#', false, 40, 10000, 400000, UseActions.DoesNothing, Behavior.DoesNothing), _zeroZero);

      public static Tile Fence = new Tile(Ground, new Contents("fence", '+', true, 5, 10000, 60, UseActions.DoesNothing, Behavior.DoesNothing), _zeroZero);

      public static Tile Hog = new Tile(new Floor('~', "mudpit"), new Contents("hog", 'H', true, 4, 40, 20, UseActions.DoesNothing, Behavior.Aggressive), _zeroZero);

      public static Tile Plant = new Tile(new Floor('Y', "plant"), null, _zeroZero);

      public static Tile Chicken = new Tile(Ground, new Contents("chicken", '>', true, 1, 2, 5, UseActions.DoesNothing, Behavior.Wander), _zeroZero);

      public static Tile Person = new Tile(Ground, new Contents("person", 'p', true, 1, 1, 1000, UseActions.Rude, Behavior.Wander), _zeroZero);

      public static Tile Rock = new Tile(Ground, new Contents("rock", 'o', true, 4, 1, 3, UseActions.DoesNothing, Behavior.DoesNothing), _zeroZero);

      public static Tile Lever = new Tile(Ground, new Contents("lever", 'L', true, 10, 234, 5151, UseActions.Lever, Behavior.DoesNothing), _zeroZero);

      public static Tile Tombstone = new Tile(Ground, new Contents("tombstone", 'n', true, 10, 30, 400, UseActions.Tombstone, Behavior.DoesNothing), _zeroZero);

      public static Tile Ghost = new Tile(Ground, new Contents("ghost", 'G', true, 1, 50, 2837, UseActions.Boo, Behavior.Wander), _zeroZero);

      public static Tile Target = new Tile(Ground, new Contents("target", '@', true, 2, 1000, 1000, UseActions.DoesNothing, Behavior.Target), _zeroZero);

      public static Tile GunTile = new Tile(Ground, new Contents("gun", 'r', true, 4, 4, 5, UseActions.Gun, Behavior.DoesNothing), _zeroZero);

      public static Tile Sign = new Tile(Ground, new Contents("sign", 'S', true, 4, 10000, 100000, UseActions.Dialogue, Behavior.DoesNothing), _zeroZero);

      public static Tile GateOpen = new Tile(new Floor('.', "retractedGate"), null, _zeroZero);

      public static Tile GateClosed = new Tile(Ground, new Contents("gate", '-', true, 2000, 10, 12390, UseActions.DoesNothing, Behavior.DoesNothing), _zeroZero);
      #endregion

      public static Dictionary<string, string> Dialogue = new Dictionary<string, string>();

      public static Map WorldMap;

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
                           'C',
                           true,
                           10,
                           1000,
                           1000,
                           true,
                           100,
                           new List<Contents>()
                           {
                              new Contents("coin", 'c', true, 3, 1, 1, UseActions.DoesNothing, Behavior.DoesNothing),
                              new Contents("coin", 'c', true, 3, 1, 1, UseActions.DoesNothing, Behavior.DoesNothing),
                              new Contents("coin", 'c', true, 3, 1, 1, UseActions.DoesNothing, Behavior.DoesNothing)
                           },
                           UseActions.DoesNothing,
                           Behavior.DoesNothing), _zeroZero),
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
      
      /*public static Map _worldMap = new Map(new Level[,]
      {
         // column 1
         {
            null,
            new Level
            (
               "Field",
               'T',
               new Grid
               (
                  new Tile[][]
                  {
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Chicken.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                  }
               ),
               new Coord(0, 1),
               null,
               new Coord(6, 3),
               new Coord(3, 6),
               null
            ),new Level
            (
               "Market",
               'M',
               new Grid
               (

                  new Tile[][]
                  {
                     new Tile[]
                     {
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)GateClosed.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)GateClosed.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Chicken.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Person.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)GateClosed.Clone(),
                        (Tile)GateClosed.Clone(),
                        (Tile)GateClosed.Clone(),
                        (Tile)GateClosed.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Rock.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Lever.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Person.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Person.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Person.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Rock.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Person.Clone(),
                        (Tile)Chicken.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Fence.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Person.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Wall.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Person.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Rock.Clone(),
                        (Tile)Wall.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Hog.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Rock.Clone(),
                        (Tile)Wall.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Wall.Clone(),
                     }
                  }
               ),
               new Coord(0, 2),
               new Coord(3, 0),
               new Coord(9, 4),
               null,
               null
            ),
         },
         // column 2
         {
            new Level
            (
               "Barn",
               'B',
               new Grid
               (
                  new Tile[][]
                  {
                     new Tile[]
                     {
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Mud.Clone(),
                        (Tile)Hog.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Empty.Clone(),
                        new Tile(Ground, Player.Contents, _zeroZero),
                        (Tile)Empty.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Mud.Clone(),
                        (Tile)Mud.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Mud.Clone(),
                        (Tile)Mud.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Mud.Clone(),
                        (Tile)Mud.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Fence.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Fence.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Mud.Clone(),
                        (Tile)Hog.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Mud.Clone(),
                        (Tile)Hog.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Mud.Clone(),
                        (Tile)Mud.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Fence.Clone(),
                        (Tile)Mud.Clone(),
                        (Tile)Mud.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                        (Tile)Wall.Clone(),
                     }
                  }
               ),
               new Coord(1, 0),
               null,
               null,
               new Coord(3, 6),
               null
            ),
            new Level
            (
               "Farm",
               'F',
               new Grid
               (
                  new Tile[][]
                  {
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Chicken.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        new Tile(Ground, new Contents("monster", '!', 40, 200, false, 8, 2000, 3000, true, 10, new List<Contents>(), UseActions.MonsterDialogue, Behavior.MonsterVictory), _zeroZero),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                  }
               ),
               new Coord(1, 1),
               new Coord(3, 0),
               new Coord(6, 3),
               new Coord(3, 6),
               new Coord(0, 3)
            ),
            new Level
            (
               "Field",
               'T',
               new Grid
               (
                  new Tile[][]
                  {
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Chicken.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Chicken.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                  }
               ),
               new Coord(1, 2),
               new Coord(3, 0),
               new Coord(6, 3),
               null,
               new Coord(0, 3)
            ),
         },
         // column 3
         {
            null,
            new Level
            (
               "Field",
               'T',
               new Grid
               (
                  new Tile[][]
                  {
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Chicken.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Plant.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                  }
               ),
               new Coord(2, 1),
               null,
               null,
               new Coord(3, 6),
               new Coord(0, 3)
            ),
            new Level
            (
               "Graveyard",
               'G',
               new Grid
               (
                  new Tile[][]
                  {
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Tombstone.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Tombstone.Clone(),
                        (Tile)Ghost.Clone(),
                        (Tile)Tombstone.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Ghost.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Tombstone.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Tombstone.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Tombstone.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Ghost.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Chicken.Clone(),
                        (Tile)Ghost.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Tombstone.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Tombstone.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Tombstone.Clone(),
                        (Tile)Empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                        (Tile)Empty.Clone(),
                     }
                  }
               ),
               new Coord(2, 2),
               new Coord(3, 0),
               null,
               null,
               new Coord(0, 3)
            ),
         }
      },"test");*/ // For editing & testing

      public static Level LoadedLevel;

      public static List<Contents> ContentsIndex = new List<Contents>();

      public static bool TryGetMapsFolder(out string result)
      {
         string tryFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\maps\";
         try
         {
            result = tryFolder;
            return true;
         }
         catch
         {
            Console.WriteLine("Your 'maps' folder may be missing. We're making one now. Be sure to put all your world files in the folder named 'maps' within the directory of the executable");
            Directory.CreateDirectory(tryFolder);
            result = null;
            return false;
         }
      }
      public static bool LoadFromFile()
      {
         string fileMouth;
         if (!TryGetMapsFolder(out fileMouth))
         {
            return false;
         }

         DirectoryInfo directory = new DirectoryInfo(fileMouth);

         Console.WriteLine("Here are the current world files");

         Dictionary<string, FileInfo> files = new Dictionary<string, FileInfo>();
         foreach (FileInfo file in directory.GetFiles("*.txt"))
         {
            files.Add(file.Name, file);
         }

         if (files.Count == 0)
         {
            Console.WriteLine("It appears there are no files to load. Either download a world file, or try designing one in the level editor");
            return false;
         }

         string[] fileNames = files.Keys.ToArray();

         Console.WriteLine("Choose a file from the options below.");
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

      public static void LoadFromFile(string filePath)
      {
         Console.WriteLine("Loading file " + filePath + "...");
         StreamReader sr;
         try
         {
            sr = new StreamReader(filePath);
         }
         catch
         {
            Console.WriteLine("File path is incorrect");
            return;
         }

         string[] splitLine = SplitNextLine(sr);
         string mapName = splitLine[1];

         List<Level[]> levelGrid = new List<Level[]>();
         // each row of levels v
         for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
         {
            List<Level> levels = new List<Level>();
            // each level within the given row v
            for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
            {
               if (splitLine[0] == "null")
               {
                  levels.Add(null);
                  continue;
               }
               List<Tile[]> tileGrid = new List<Tile[]>();
               // each row of tiles within the given level v
               for (string[] tileRowLine = SplitNextLine(sr); tileRowLine[0] != "}"; tileRowLine = SplitNextLine(sr))
               {
                  List<Tile> tiles = new List<Tile>();
                  // each tile within the given row v
                  for (tileRowLine = SplitNextLine(sr); tileRowLine[0] != "}"; tileRowLine = SplitNextLine(sr))
                  {
                     tileRowLine = SplitNextLine(sr);
                     Floor floor = new Floor(tileRowLine[2][0], tileRowLine[1]);
                     Contents contents = GetAllContents(sr)[0];
                     tileRowLine = SplitNextLine(sr);
                     Coord coordinates = new Coord(int.Parse(tileRowLine[1]), int.Parse(tileRowLine[2]));
                     if (contents != null)
                     {
                        contents.Coordinates = coordinates;
                     }

                     tiles.Add(new Tile(floor, contents, coordinates));
                     sr.ReadLine();
                  }
                  tileGrid.Add(tiles.ToArray());
               }

               // Parsing Coord from comma-delimited storage on file
               Coord levelCoord = new Coord(int.Parse(splitLine[4].Split(",")[0]), int.Parse(splitLine[4].Split(",")[1]));
               Coord northEntry;
               Coord eastEntry;
               Coord southEntry;
               Coord westEntry;
               if (splitLine[5] == "null")
               {
                  northEntry = null;
               }
               else
               {
                  northEntry = new Coord(int.Parse(splitLine[5].Split(",")[0]), int.Parse(splitLine[5].Split(",")[1]));
               }

               if (splitLine[6] == "null")
               {
                  eastEntry = null;
               }
               else
               {
                  eastEntry = new Coord(int.Parse(splitLine[6].Split(",")[0]), int.Parse(splitLine[6].Split(",")[1]));
               }

               if (splitLine[7] == "null")
               {
                  southEntry = null;
               }
               else
               {
                  southEntry = new Coord(int.Parse(splitLine[7].Split(",")[0]), int.Parse(splitLine[7].Split(",")[1]));
               }

               if (splitLine[8] == "null")
               {
                  westEntry = null;
               }
               else
               {
                  westEntry = new Coord(int.Parse(splitLine[8].Split(",")[0]), int.Parse(splitLine[8].Split(",")[1]));
               }

               Level levelToAdd = new Level(splitLine[1], splitLine[2][0], new Grid(tileGrid.ToArray()), levelCoord, northEntry, eastEntry, southEntry, westEntry);
               levels.Add(levelToAdd);
               if (bool.Parse(splitLine[3]))
               {
                  LoadedLevel = levelToAdd;
               }
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

         sr.ReadLine();

         splitLine = SplitNextLine(sr);

         Coord playerCoords = new Coord(int.Parse(splitLine[0]), int.Parse(splitLine[1]));

         List<Contents> allContents = GetAllContents(sr);
         
         if (!World.LoadedLevel.Grid.GetTileAtCoords(playerCoords, out Tile tile, false))
         {
            return;
         }
         
         Player = new Player(tile.Contents, allContents[0], int.Parse(sr.ReadLine()));

         sr.ReadLine();

         ContentsIndex = GetAllContents(sr);

         sr.ReadLine();

         for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
         {
            string restOfLine = "";
            for (int splitLineIndex = 1; splitLineIndex < splitLine.Length; splitLineIndex++)
            {
               restOfLine += splitLine[splitLineIndex] + " ";
            }
            restOfLine.Trim();
            Dialogue.Add(splitLine[0], restOfLine);
         }

         sr.Close();
      }

      private static List<Contents> GetAllContents(StreamReader sr)
      {
         sr.ReadLine();
         List<Contents> contentsList = new List<Contents>();
         for (string[] splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
         {
            if (splitLine[0] == "null")
            {
               contentsList.Add(null);
               continue;
            }
            bool isContainer = bool.Parse(splitLine[6]);
            if (UseActions.TryGetAction(splitLine[8], out Action<string[], Contents> actionResult) && Behavior.TryGetBehavior(splitLine[9], out Action<Contents> behaviorResult))
            {
               contentsList.Add(new Contents(
               name: splitLine[0],
               visualChar: splitLine[1][0],
               transparent: bool.Parse(splitLine[2]),
               durability: int.Parse(splitLine[3]),
               size: int.Parse(splitLine[4]),
               weight: float.Parse(splitLine[5]),
               container: isContainer,
               containerSpace: int.Parse(splitLine[7]),
               useAction: actionResult,
               behavior: behaviorResult,
               contained: (isContainer ? GetAllContents(sr) : null)
               ));
               if (!isContainer)
               {
                  sr.ReadLine();
                  sr.ReadLine();
                  sr.ReadLine();
               }
            }
         }
         return contentsList;
      }

      private static string[] SplitNextLine(StreamReader sr)
      {
         return sr.ReadLine().Split(" ");
      }

      public static void SaveToFile(string filePath)
      {
         StreamWriter sw;
         try
         {
            sw = new StreamWriter(filePath);
         }
         catch
         {
            if (CommandInterpretation.AskYesNo("File path is invalid. Would you like to try again?"))
            {
               SaveToFile(CommandInterpretation.GetUserResponse("Enter file path"));
            }
            return;
         }
         Level[,] levelMap = WorldMap.LevelMap;
         sw.WriteLine("map " + WorldMap.Name + " {");
         for (int levelY = 0; levelY < levelMap.GetLength(1); levelY++)
         {
            sw.WriteLine("row " + levelY + " {");
            for (int levelX = 0; levelX < levelMap.GetLength(0); levelX++)
            {
               Level level = levelMap[levelX, levelY];
               if (level == null)
               {
                  sw.WriteLine("null");
                  continue;
               }
               // v This is on one line
               sw.WriteLine(
                  "level " + level.Name + " " + level.VisualChar + " " + (level.Equals(LoadedLevel)) +
                  " " + level.LevelCoord.X + "," + level.LevelCoord.Y + 
                  " " + (level.NorthEntry == null ? "null" : (level.NorthEntry.X + "," + level.NorthEntry.Y)) + 
                  " " + (level.EastEntry == null ? "null" : (level.EastEntry.X + "," + level.EastEntry.Y)) + 
                  " " + (level.SouthEntry == null ? "null" : (level.SouthEntry.X + "," + level.SouthEntry.Y)) + 
                  " " + (level.WestEntry == null ? "null" : (level.WestEntry.X + "," + level.WestEntry.Y)) + 
                  " " + "grid {");
               for (int y = 0; y < level.Grid.TileGrid.GetLength(1); y++)
               {
                  sw.WriteLine("row " + y + " {");
                  for (int x = 0; x < level.Grid.TileGrid.GetLength(0); x++)
                  {
                     Tile tileAtCoords = level.Grid.TileGrid[x, y];
                     sw.WriteLine("tile {");

                     sw.WriteLine("floor " + tileAtCoords.Floor.Name + " " + tileAtCoords.Floor.VisualChar);
                     sw.WriteLine("contents {");
                     ListAllContents(tileAtCoords.Contents, sw);
                     sw.WriteLine("}");
                     sw.WriteLine("coordinates " + x + " " + y);

                     sw.WriteLine("}");
                  }
                  sw.WriteLine("}");
               }
               sw.WriteLine("}");
            }
            sw.WriteLine("}");
         }
         sw.WriteLine("}");

         sw.WriteLine("player {");
         sw.WriteLine(Player.Contents.Coordinates.X + " " + Player.Contents.Coordinates.Y);
         sw.WriteLine("holding {");
         ListAllContents(Player.Holding, sw);
         sw.WriteLine("}");
         sw.WriteLine(Player.Strength);
         sw.WriteLine("}");

         sw.WriteLine("Tile index {");

         foreach (Contents contents in ContentsIndex)
         {
            contents.Container = false;
            ListAllContents(contents, sw);
         }

         sw.WriteLine("}");

         sw.WriteLine("Dialogue {");

         foreach (string key in Dialogue.Keys)
         {
            sw.WriteLine(key + " " + Dialogue[key]);
         }

         sw.WriteLine("}");

         sw.Close();
         Console.WriteLine("World file saved successfully as " + Game.FilePath);
      }

      private static void ListAllContents(Contents contentsAtCoords, StreamWriter sw)
      {
         if (contentsAtCoords == null)
         {
            sw.WriteLine("null");
            return;
         }
         sw.Write(contentsAtCoords.Name + " " + contentsAtCoords.VisualChar + " "  + contentsAtCoords.Transparent + " " + contentsAtCoords.Durability + " " + contentsAtCoords.Size + " " + contentsAtCoords.Weight + " ");
         sw.Write(contentsAtCoords.Container + " " + contentsAtCoords.ContainerSpace + " ");
         if (UseActions.TryGetIdentifier(contentsAtCoords.UseAction, out string actionResult) && Behavior.TryGetIdentifier(contentsAtCoords.Behavior, out string behaviorResult))
         {
            sw.Write(actionResult + " ");
            sw.WriteLine(behaviorResult);
         }
         sw.WriteLine("contained {");
         if (!contentsAtCoords.Container)
         {
            sw.WriteLine("null");
         }
         else
         {
            foreach (Contents contained in contentsAtCoords.Contained)
            {
               ListAllContents(contained, sw);
            }
         }
         sw.WriteLine("}");
      }

      public static void UpdateWorld()
      {
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
               updateContents.Behavior(updateContents);
            }
         }
         LoadedLevel = realLoadedLevel;
      }

        public static bool GetPlayerLevel(out Level result)
        {
            foreach (Level level in World.WorldMap.LevelMap)
            {
                if (level.Grid.TryFindContents(World.Player.Contents, out _))
                {
                    result = level;
                    return true;
                }
            }
            result = null;
            return false;
        }

   }
}