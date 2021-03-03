using System;
using System.Collections.Generic;

namespace GameEngine
{
   class Contents : ICloneable
   {
      public Coord Coordinates;

      public string Name;
      public char VisualChar;

      public int Durability;

      public int Size;
      public float Weight;
      public float TotalWeight;

      public bool Transparent;

      public bool Container = false;
      public int ContainerSpace = 0;
      public int UsedSpace = 0;
      public List<Contents> Contained = new List<Contents>();

      public Action<string[], Contents> UseAction;
      public Action<Contents> Behavior;

      public Contents(string name, char visualChar, bool transparent, int durability, int size, float weight, Action<string[], Contents> useAction, Action<Contents> behavior)
      {
         Name = name;
         VisualChar = visualChar;
         Transparent = transparent;
         Durability = durability;
         Size = size;
         Weight = weight;
         UseAction = useAction;
         Behavior = behavior;
      }

      public Contents(string name, char visualChar, bool transparent, int durability, int size, float weight, bool container, int containerSpace, List<Contents> contained, Action<string[], Contents> useAction, Action<Contents> behavior) : this(name, visualChar, transparent, durability, size, weight, useAction, behavior)
      {
         Container = container;
         ContainerSpace = containerSpace;
         Contained = contained;

         CleanOut();

         UsedSpace = 0;
         TotalWeight = GetWeightRecursive();
         if (container)
         {
            foreach (Contents contents in Contained)
            {
               UsedSpace += contents.Size;
            }
         }
      }
      private void CleanOut()
      {
         if (Container)
         {
            while (Contained.Remove(null)) ;
         }
      }

      private float GetWeightRecursive()
      {
         float totalWeight = Weight;
         if (Container)
         {
            foreach (Contents contents in Contained)
            {
               totalWeight += contents.GetWeightRecursive();
            }
         }

         return totalWeight;
      }

      public void AddContents(Contents contentsToAdd)
      {
         if (Container)
         {
            if (contentsToAdd.Size <= (ContainerSpace - UsedSpace))
            {
               Contained.Add(contentsToAdd);
               UsedSpace += contentsToAdd.Size;
               TotalWeight += contentsToAdd.Weight;
               Console.WriteLine("Added.");
            }
            else
            {
               Console.WriteLine(contentsToAdd.Name + " is to big. It doesn't fit inside " + Name);
               if (Contained.Count > 0 && ContainerSpace >= contentsToAdd.Size)
               {
                  Console.WriteLine("You can remove some items from " + Name + " to make space for it, though");
               }
            }
         }
         else
         {
            Console.WriteLine("You can't put " + contentsToAdd.Name + " in " + Name);
         }
      }
      public string ListContents()
      {
         string output = "\nHere are the contents of " + Name + "\n";
         output += "Space used in " + Name + ": " + UsedSpace + "/" + ContainerSpace + "\n\n";

         for (int contentsIndex = 0; contentsIndex < Contained.Count; contentsIndex++)
         {
            Contents contents = Contained[contentsIndex];

            output += contentsIndex + ". " + contents.Name + "\tSize: " + contents.Size + "\tWeight: " + contents.TotalWeight + "\n";
         }
         return output;
      }
      public string ListContainers()
      {
         Console.WriteLine("Here are the containers of " + Name);

         string output = "\n";

         output += "0. Self\n";
         for (int contentsIndex = 0; contentsIndex < Contained.Count; contentsIndex++)
         {
            Contents contents = Contained[contentsIndex];
            if (contents.Container)
            {
               Console.WriteLine((contentsIndex + 1) + ". " + contents.Name + "\tSize: " + contents.Size + "\tWeight: " + contents.Weight);
            }
         }
         return output;
      }

      public void Damage(int damage, bool displayMessage = true)
      {
         Durability -= damage;
         if (displayMessage)
         {
            Console.WriteLine(Name + " was damaged for " + damage);
         }
         if (Durability <= 0)
         {
            if (Container && Contained.Count > 0)
            {
               // HERE IS WHERE BAG IS DEFINED. Change as you wish
               if (!World.LoadedLevel.Grid.GetTileAtCoords(Coordinates, out Tile destroyTile))
               {
                  return;
               }

               destroyTile.Contents = new Contents
                  (
                  name: "bag",
                  visualChar: 'b',
                  true,
                  1,
                  5,
                  1,
                  true,
                  ContainerSpace,
                  Contained,
                  UseActions.DoesNothing,
                  GameEngine.Behavior.DoesNothing
                  );
               destroyTile.Contents.Coordinates = destroyTile.Coordinates;
            }
            else
            {
               if (World.LoadedLevel.Grid.GetTileAtCoords(Coordinates, out Tile tile))
               {
                  tile.Contents = null;
               }
            }
         }
      }

      public object Clone()
      {
         return new Contents(this.Name, this.VisualChar, this.Transparent, this.Durability, this.Size, this.Weight, this.Container, this.ContainerSpace, this.Contained, this.UseAction, this.Behavior);
      }
   }
}
