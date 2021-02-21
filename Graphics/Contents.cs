using System;
using System.Collections.Generic;

namespace GameEngine
{
   class Contents : ICloneable
   {
      public Coord _coordinates;

      public string _name;
      public char _visualChar;

      public int _temperature;
      public int _meltingpoint;
      public int _durability;

      public int _size;
      public float _weight;
      public float _totalWeight;

      public bool _transparent;

      public bool _container = false;
      public int _containerSpace = 0;
      public int _usedSpace = 0;
      public List<Contents> _contained = new List<Contents>();

      public Action<string[], Contents> _useAction;
      public Action<Contents> _behavior;

      public Contents(string name, char visualChar, int temperature, int meltingpoint, bool transparent, int durability, int size, float weight, Action<string[], Contents> useAction, Action<Contents> behavior)
      {
         _name = name;
         _visualChar = visualChar;
         _temperature = temperature;
         _meltingpoint = meltingpoint;
         _transparent = transparent;
         _durability = durability;
         _size = size;
         _weight = weight;
         _useAction = useAction;
         _behavior = behavior;
      }

      public Contents(string name, char visualChar, int temperature, int meltingpoint, bool transparent, int durability, int size, float weight, bool container, int containerSpace, List<Contents> contained, Action<string[], Contents> useAction, Action<Contents> behavior) : this(name, visualChar, temperature, meltingpoint, transparent, durability, size, weight, useAction, behavior)
      {
         _container = container;
         _containerSpace = containerSpace;
         _contained = contained;

         CleanOut();

         _usedSpace = 0;
         _totalWeight = GetWeightRecursive();
         if (container)
         {
            foreach (Contents contents in _contained)
            {
               _usedSpace += contents._size;
            }
         }
      }
      private void CleanOut()
      {
         if (_container)
         {
            while (_contained.Remove(null)) ;
         }
      }

      private float GetWeightRecursive()
      {
         float totalWeight = _weight;
         if (_container)
         {
            foreach (Contents contents in _contained)
            {
               totalWeight += contents.GetWeightRecursive();
            }
         }

         return totalWeight;
      }

      public void AddContents(Contents contentsToAdd)
      {
         if (_container)
         {
            if (contentsToAdd._size <= (_containerSpace - _usedSpace))
            {
               _contained.Add(contentsToAdd);
               _usedSpace += contentsToAdd._size;
               _totalWeight += contentsToAdd._weight;
               Console.WriteLine("Added.");
            }
            else
            {
               Console.WriteLine(contentsToAdd._name + " is to big. It doesn't fit inside " + _name);
               if (_contained.Count > 0 && _containerSpace >= contentsToAdd._size)
               {
                  Console.WriteLine("You can remove some items from " + _name + " to make space for it, though");
               }
            }
         }
         else
         {
            Console.WriteLine("You can't put " + contentsToAdd._name + " in " + _name);
         }
      }
      public string ListContents()
      {
         string output = "\nHere are the contents of " + _name + "\n";
         output += "Space used in " + _name + ": " + _usedSpace + "/" + _containerSpace + "\n\n";

         for (int contentsIndex = 0; contentsIndex < _contained.Count; contentsIndex++)
         {
            Contents contents = _contained[contentsIndex];

            output += contentsIndex + ". " + contents._name + "\tSize: " + contents._size + "\tWeight: " + contents._totalWeight + "\n";
         }
         return output;
      }
      public string ListContainers()
      {
         Console.WriteLine("Here are the containers of " + _name);

         string output = "\n";

         output += "0. Self\n";
         for (int contentsIndex = 0; contentsIndex < _contained.Count; contentsIndex++)
         {
            Contents contents = _contained[contentsIndex];
            if (contents._container)
            {
               Console.WriteLine((contentsIndex + 1) + ". " + contents._name + "\tSize: " + contents._size + "\tWeight: " + contents._weight);
            }
         }
         return output;
      }

      public void Damage(int damage, bool displayMessage = true)
      {
         _durability -= damage;
         if (displayMessage)
         {
            Console.WriteLine(_name + " was damaged for " + damage);
         }
         if (_durability <= 0)
         {
            if (_container && _contained.Count > 0)
            {
               // HERE IS WHERE BAG IS DEFINED. Change as you wish
               Tile destroyTile = World._loadedLevel._grid.GetTileAtCoords(_coordinates);

               destroyTile._contents = new Contents
                  (
                  name: "bag",
                  visualChar: 'b',
                  20,
                  100,
                  true,
                  1,
                  5,
                  1,
                  true,
                  _containerSpace,
                  _contained,
                  UseActions.DoesNothing,
                  Behavior.DoesNothing
                  );
               destroyTile._contents._coordinates = destroyTile._coordinates;
            }
            else
            {
               World._loadedLevel._grid.GetTileAtCoords(_coordinates)._contents = null;
            }
         }
      }

      public object Clone()
      {
         return new Contents(this._name, this._visualChar, this._temperature, this._meltingpoint, this._transparent, this._durability, this._size, this._weight, this._container, this._containerSpace, this._contained, this._useAction, this._behavior);
      }
   }
}
