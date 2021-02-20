﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace A
{
   static class World
   {
      private static readonly Coord _zeroZero = new Coord(0, 0);

      public static Player _player = new Player(new Contents("Player", 'A', 20, 100, true, 50, 10, 50, true, 100, new List<Contents>(), UseActions.DoesNothing, Behavior.DoesNothing), null, 50);

      #region tile definitions

      public static Floor _ground = new Floor('.', "ground");

      public static Tile _empty = new Tile(_ground, null, _zeroZero);

      public static Tile _mud = new Tile(new Floor('~', "mud"), null, _zeroZero);

      public static Tile _wall = new Tile(_ground, new Contents("wall", '#', 20, 500, false, 40, 10000, 400000, UseActions._customCommands[0], Behavior.DoesNothing), _zeroZero);

      public static Tile _fence = new Tile(_ground, new Contents("fence", '+', 20, 100, true, 5, 10000, 60, UseActions.DoesNothing, Behavior.DoesNothing), _zeroZero);

      public static Tile _hog = new Tile(new Floor('~', "mudpit"), new Contents("hog", 'H', 30, 100, true, 4, 40, 20, UseActions.DoesNothing, Behavior.Aggressive), _zeroZero);

      public static Tile _plant = new Tile(new Floor('Y', "plant"), null, _zeroZero);

      public static Tile _chicken = new Tile(_ground, new Contents("chicken", '>', 20, 100, true, 1, 2, 5, UseActions.DoesNothing, Behavior.Wander), _zeroZero);

      public static Tile _gateClosed = new Tile(_ground, new Contents("gate", '-', 20, 100, true, 2000, 10, 12390, UseActions.DoesNothing, Behavior.DoesNothing), _zeroZero);

      public static Tile _person = new Tile(_ground, new Contents("person", 'p', 20, 100, true, 1, 1, 1000, UseActions.Rude, Behavior.Wander), _zeroZero);

      public static Tile _rock = new Tile(_ground, new Contents("rock", 'o', 10, 1000, true, 4, 1, 3, UseActions.DoesNothing, Behavior.DoesNothing), _zeroZero);

      public static Tile _lever = new Tile(_ground, new Contents("lever", 'L', 10, 203, true, 10, 234, 5151, UseActions.Lever, Behavior.DoesNothing), _zeroZero);

      public static Tile _tombstone = new Tile(_ground, new Contents("tombstone", 'n', 0, 50, true, 10, 30, 400, UseActions.Tombstone, Behavior.DoesNothing), _zeroZero);

      public static Tile _ghost = new Tile(_ground, new Contents("ghost", 'G', 0, 100, true, 1, 50, 2837, UseActions.Boo, Behavior.Wander), _zeroZero);

      public static Tile _target = new Tile(_ground, new Contents("target", '@', 20, 100, true, 2, 1000, 1000, UseActions.DoesNothing, Behavior.Target), _zeroZero);

      public static Tile _gunTile = new Tile(_ground, new Contents("gun", 'r', 20, 200, true, 4, 4, 5, UseActions.Gun, Behavior.DoesNothing), _zeroZero);

      public static Tile _sign = new Tile(_ground, new Contents("sign", 'S', 20, 100, true, 4, 10000, 100000, UseActions.Dialogue, Behavior.DoesNothing), _zeroZero);

      public static Tile _gateOpen = new Tile(new Floor('.', "retractedGate"), null, _zeroZero);
      #endregion

      public static Dictionary<string, string> _dialogue = new Dictionary<string, string>();

      public static Map _worldMap;

      public static Map _tutorialLevel = new Map(new Level[][]
      {
         // column 1
         new Level[]
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
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_target.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_gateClosed.Clone()
                     },
                     // row 2
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_gunTile.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_gateClosed.Clone()
                     },
                     // row 3 
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        new Tile(_ground, _player._contents, _zeroZero),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_gateClosed.Clone()
                     },
                     // row 4
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_gateClosed.Clone()
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
         new Level[]
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
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_lever.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_lever.Clone(),
                        new Tile(_ground, new Contents(
                           "chest",
                           'C',
                           20,
                           100,
                           true,
                           10,
                           1000,
                           1000,
                           true,
                           100,
                           new List<Contents>()
                           {
                              new Contents("coin", 'c', 20, 2000, true, 3, 1, 1, UseActions.DoesNothing, Behavior.DoesNothing),
                              new Contents("coin", 'c', 20, 2000, true, 3, 1, 1, UseActions.DoesNothing, Behavior.DoesNothing),
                              new Contents("coin", 'c', 20, 2000, true, 3, 1, 1, UseActions.DoesNothing, Behavior.DoesNothing)
                           },
                           UseActions.DoesNothing,
                           Behavior.DoesNothing), _zeroZero),
                     },
                     // row 2
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_gateClosed.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     // row 3
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_gateClosed.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     // row 4
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_sign.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_gateOpen.Clone(),
                        (Tile)_gateOpen.Clone(),
                     },
                     // row 5
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     // row 6
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_hog.Clone(),
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

      /*public static Map _worldMap = new Map(new Level[][]
      {
         // column 1
         new Level[]
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
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_chicken.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
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
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_changableWall.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_changableWall.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_chicken.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_person.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_changableWall.Clone(),
                        (Tile)_changableWall.Clone(),
                        (Tile)_changableWall.Clone(),
                        (Tile)_changableWall.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_rock.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_lever.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_person.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_person.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_person.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_rock.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_person.Clone(),
                        (Tile)_chicken.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_fence.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_person.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_wall.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_person.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_rock.Clone(),
                        (Tile)_wall.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_hog.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_rock.Clone(),
                        (Tile)_wall.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_wall.Clone(),
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
         new Level[]
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
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_mud.Clone(),
                        (Tile)_hog.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_empty.Clone(),
                        new Tile(_ground, _player._contents, _zeroZero),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_mud.Clone(),
                        (Tile)_mud.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_mud.Clone(),
                        (Tile)_mud.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_mud.Clone(),
                        (Tile)_mud.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_fence.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_fence.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_mud.Clone(),
                        (Tile)_hog.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_mud.Clone(),
                        (Tile)_hog.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_mud.Clone(),
                        (Tile)_mud.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_fence.Clone(),
                        (Tile)_mud.Clone(),
                        (Tile)_mud.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
                        (Tile)_wall.Clone(),
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
                        (Tile)_empty.Clone(),
                        (Tile)_chicken.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        new Tile(_ground, new Contents("monster", '!', 40, 200, false, 8, 2000, 3000, true, 10, new List<Contents>(), UseActions.MonsterDialogue, Behavior.MonsterVictory), _zeroZero),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
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
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_chicken.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_chicken.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
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
         new Level[]
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
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_chicken.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_plant.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
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
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_tombstone.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_tombstone.Clone(),
                        (Tile)_ghost.Clone(),
                        (Tile)_tombstone.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_ghost.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_tombstone.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_tombstone.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_tombstone.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_ghost.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_chicken.Clone(),
                        (Tile)_ghost.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_tombstone.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_tombstone.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_tombstone.Clone(),
                        (Tile)_empty.Clone(),
                     },
                     new Tile[]
                     {
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
                        (Tile)_empty.Clone(),
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
      },"test"); */// For editing

      public static Level _loadedLevel;

      public static List<Contents> _contentsIndex = new List<Contents>();

      public static bool LoadFromFile()
      {
         string fileMouth;
         try
         {
            fileMouth = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\maps\";
         }
         catch
         {
            Console.WriteLine("Your 'maps' folder may be missing. Be sure to put all your world files in a folder named 'maps' within the directory of the executable");
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

         foreach (string file in fileNames)
         {
            Console.WriteLine(file);
         }

         if (CommandInterpretation.InterpretString(CommandInterpretation.GetUserResponse("Which file would you like to load?"), fileNames, out string result))
         {
            Game._filePath = files[result].FullName;
            LoadFromFile(files[result].FullName);
         }
         else
         {
            if (CommandInterpretation.InterpretYesNo("No file found with that name. Would you like to try again?"))
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
                        contents._coordinates = coordinates;
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
                  _loadedLevel = levelToAdd;
               }
            }
            levelGrid.Add(levels.ToArray());
         }

         _worldMap = new Map(levelGrid.ToArray(), mapName);

         sr.ReadLine();

         splitLine = SplitNextLine(sr);

         Coord playerCoords = new Coord(int.Parse(splitLine[0]), int.Parse(splitLine[1]));

         List<Contents> allContents = GetAllContents(sr);

         _player = new Player(_loadedLevel._grid.GetTileAtCoords(playerCoords)._contents, allContents[0], int.Parse(sr.ReadLine()));

         sr.ReadLine();

         _contentsIndex = GetAllContents(sr);

         sr.ReadLine();

         for (splitLine = SplitNextLine(sr); splitLine[0] != "}"; splitLine = SplitNextLine(sr))
         {
            _dialogue.Add(splitLine[0], splitLine[1]);
         }

         sr.Close();
      }

      private static Level[][] InvertMap(Level[][] levels)
      {
         List<Level[]> levelList = new List<Level[]>();
         for (int x = 0; x < levels[0].Length; x++)
         {
            levelList.Add(new Level[levels.Length]);
         }

         for (int y = 0; y < levels.Length; y++)
         {
            Level[] levelRow = levels[y];
            for (int x = 0; x < levels[0].Length; x++)
            {
               levelList[x][y] = levels[y][x];
            }
         }
         return levelList.ToArray();
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
            bool isContainer = bool.Parse(splitLine[8]);
            if (UseActions.TryGetAction(splitLine[10], out Action<string[], Contents> actionResult) && Behavior.TryGetAction(splitLine[11], out Action<Contents> behaviorResult))
            {
               contentsList.Add(new Contents(
               name: splitLine[0],
               visualChar: splitLine[1][0],
               temperature: int.Parse(splitLine[2]),
               meltingpoint: int.Parse(splitLine[3]),
               transparent: bool.Parse(splitLine[4]),
               durability: int.Parse(splitLine[5]),
               size: int.Parse(splitLine[6]),
               weight: float.Parse(splitLine[7]),
               container: isContainer,
               containerSpace: int.Parse(splitLine[9]),
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
            if (CommandInterpretation.InterpretYesNo("File path is invalid. Would you like to try again?"))
            {
               SaveToFile(CommandInterpretation.GetUserResponse("Enter file path"));
            }
            return;
         }
         Level[][] levelMap = InvertMap(_worldMap._levelMap);
         sw.WriteLine("map " + _worldMap._name + " {");
         for (int levelY = 0; levelY < levelMap[0].Length; levelY++)
         {
            sw.WriteLine("row " + levelY + " {");
            for (int levelX = 0; levelX < levelMap.Length; levelX++)
            {
               Level level = levelMap[levelX][levelY];
               if (level == null)
               {
                  sw.WriteLine("null");
                  continue;
               }
               sw.WriteLine(
                  "level " + level._name + " " + level._visualChar + " " + (level.Equals(_loadedLevel)) +
                  " " + level._levelCoord._x + "," + level._levelCoord._y + 
                  " " + (level._northEntry == null ? "null" : (level._northEntry._x + "," + level._northEntry._y)) + 
                  " " + (level._eastEntry == null ? "null" : (level._eastEntry._x + "," + level._eastEntry._y)) + 
                  " " + (level._southEntry == null ? "null" : (level._southEntry._x + "," + level._southEntry._y)) + 
                  " " + (level._westEntry == null ? "null" : (level._westEntry._x + "," + level._westEntry._y)) + 
                  " " + "grid {");
               for (int y = 0; y < level._grid._tileGrid.GetLength(1); y++)
               {
                  sw.WriteLine("row " + y + " {");
                  for (int x = 0; x < level._grid._tileGrid.GetLength(0); x++)
                  {
                     Tile tileAtCoords = level._grid._tileGrid[x, y];
                     sw.WriteLine("tile {");

                     sw.WriteLine("floor " + tileAtCoords._floor._name + " " + tileAtCoords._floor._visualChar);
                     sw.WriteLine("contents {");
                     ListAllContents(tileAtCoords._contents, sw);
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
         sw.WriteLine(_player._contents._coordinates._x + " " + _player._contents._coordinates._y);
         sw.WriteLine("holding {");
         ListAllContents(_player._holding, sw);
         sw.WriteLine("}");
         sw.WriteLine(_player._strength);
         sw.WriteLine("}");

         sw.WriteLine("Tile index {");

         foreach (Contents contents in _contentsIndex)
         {
            contents._container = false;
            ListAllContents(contents, sw);
         }

         sw.WriteLine("}");

         sw.WriteLine("Dialogue {");

         foreach (string key in _dialogue.Keys)
         {
            sw.WriteLine(key + _dialogue[key]);
         }

         sw.WriteLine("}");

         sw.Close();
         Console.WriteLine("World file saved successfully as " + Game._filePath);
      }

      private static void ListAllContents(Contents contentsAtCoords, StreamWriter sw)
      {
         if (contentsAtCoords == null)
         {
            sw.WriteLine("null");
            return;
         }
         sw.Write(contentsAtCoords._name + " " + contentsAtCoords._visualChar + " " + contentsAtCoords._temperature + " " + contentsAtCoords._meltingpoint + " " + contentsAtCoords._transparent + " " + contentsAtCoords._durability + " " + contentsAtCoords._size + " " + contentsAtCoords._weight + " ");
         sw.Write(contentsAtCoords._container + " " + contentsAtCoords._containerSpace + " ");
         if (UseActions.TryGetIdentifier(contentsAtCoords._useAction, out string actionResult) && Behavior.TryGetIdentifier(contentsAtCoords._behavior, out string behaviorResult))
         {
            sw.Write(actionResult + " ");
            sw.WriteLine(behaviorResult);
         }
         sw.WriteLine("contained {");
         if (!contentsAtCoords._container)
         {
            sw.WriteLine("null");
         }
         else
         {
            foreach (Contents contained in contentsAtCoords._contained)
            {
               ListAllContents(contained, sw);
            }
         }
         sw.WriteLine("}");
      }

      public static void UpdateWorld()
      {
         Level realLoadedLevel = _loadedLevel;
         foreach (Level[] levels in _worldMap._levelMap)
         {
            foreach (Level level in levels)
            {
               List<Contents> contentsToUpdate = new List<Contents>();
               _loadedLevel = level;
               if (level == null)
               {
                  continue;
               }
               foreach (Tile tile in level._grid._tileGrid)
               {
                  Contents contents = tile._contents;
                  if (contents == null)
                  {
                     continue;
                  }
                  contentsToUpdate.Add(contents);
               }
               foreach (Contents updateContents in contentsToUpdate)
               {
                  updateContents._behavior(updateContents);
               }
            }
         }
         _loadedLevel = realLoadedLevel;
      }
   }
}