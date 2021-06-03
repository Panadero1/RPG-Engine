#RPG-Engine

## Welcome

Hello and welcome to the official RPG-Engine website

If you don't already know, RPG-Engine is a text-based, console-run RPG engine with a working level editor

It's also open source! [(check it out)](https://github.com/Panadero1/RPG-Engine)

## Download

Want to skip past everything and just get to the game?

Here's a link to the [releases](https://github.com/Panadero1/RPG-Engine/releases). Just download the .zip of the latest version

**Warning** RPG-Engine is still pre-release and has a few major bugs remaining.

## About RPG-Engine

This engine runs solely through the console, but still carries a lot of capabilities

- Tutorial level built into the game
- High versatility of game style depending on the world file
- Level editor with pseudo-programming abilities [More info here](https://github.com/Panadero1/RPG-Engine/wiki/Level-Editor-Tutorial#connections)
- Modability
- Ease of sharing

## What does it look like?

All graphics are displayed through the console in a tile-based system. Here is an example from an upcoming demo world

```Example1

    A                   B
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8
  +-------------------^-------------------+
A0|         # # # # # # # # # #         ~ |
 1| # . . . . . . B B B + . . . #     ~ ~ |
 2| # . . . . . . . B B + . . . . # ~     |
 3| # . . . . . . . . . + . . . . O       |
 4| # . . . . . . . . . + . . . . #       |
 5< . . . . . . . . . . + . . A . #       |
 6| # . . . . . . . . . + . . . . O ~     |
 7| # . . . . . . . . . + . . . . # ~ ~ ~ |
 8| # . . . . . . . . . + . . . # #     ~ |
 9|             # # # # # # # #           |
  +---------------------------------------+

```

This doesn't look like much, but you gather more information from your surroundings.

For example, looking at one of the '#' tiles, you will see that it represents a wall.

The 'A' will typically represent the position of the player

All these tiles are saved onto a legend that can be recalled at any time

You can also interact with, use, and pick up the contents of some specific tiles

You can move around using the 'move' command

##### "This is great and all, but that's a pretty small game"

Sure, but this is just one level here

Moving off the left edge here will load into the next level on the map

```Example2

Level
    A
    0 1 2 3 4 5 6
  +-------^-------+
A0| # # # . # # # |
 1| # . . . . . # |
 2| # . . . . . # |
 3| # . . . . . # |
 4| # . . . . . # |
 5| # . . A . . . >
 6| # . . . . . # |
 7| # . . . . . # |
 8| # & & & & & # |
 9|               |
  +---------------+

```

You can move back to the previous room or explore further northward (as shown by the arrows on the edges of the level)

This is how the gameplay of this engine usually takes place

###### There are many more features apart from moving, including interaction, inventory management, and many other things, but I won't show it here.
Download it and play around!
