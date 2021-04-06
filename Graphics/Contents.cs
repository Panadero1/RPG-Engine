using System;
using System.Collections.Generic;

namespace GameEngine
{
	// Data class Contents represents just about any object in the game
	class Contents : ICloneable
	{
		// Used for unique contents identification
		public static int uniqueIndex = 0;

		public static Dictionary<string, Type> EditableMembers = new Dictionary<string, Type>
		{
			{ "Name", typeof(string) },
			{ "Visual Character", typeof(char) },
			{ "Transparent", typeof(bool) },
			{ "Durability", typeof(int) },
			{ "Size", typeof(int) },
			{ "Weight", typeof(float) },
			{ "Use Action", typeof(Action<string[], Contents>)},
			{ "Behaviors", typeof(Action<Contents>[]) }
		};

		// The coordinates of the contents
		public Coord Coordinates;

		// A unique identifier for the contents
		public string Name;

		// A **Guaranteed** unique integer for this specific contents
		public int ID;
		
		// The letter, number or symbol that represents the contents
		public char VisualChar;

		// The health of the contents. When reduced to zero through damage, it becomes nothing.
		public int Durability;

		// The amount of space it takes up in a container
		public int Size;

		// The amount it weighs
		public float Weight;

		// The total amount of weight- including its contents
		public float TotalWeight;

		// Whether it is visible and targetable through
		public bool Transparent;


		// Whether or not it is able to store other contents
		public bool Container = false;
		
		// How much space this container can hold within it
		public int ContainerSpace = 0;

		// The amount of space taken up in this container
		public int UsedSpace = 0;

		// The contents within this container
		public List<Contents> Contained = new List<Contents>();

		// The funciton this calls when it is interacted with or used
		public Action<string[], Contents> UseAction;

		// The function this calls every time the world updates. (only when it is on a tile)
		public Action<Contents>[] Behaviors;

		public string[] Tags = new string[0];

		public Contents(string name, int id, char visualChar, bool transparent, int durability, int size, float weight, Action<string[], Contents> useAction, Action<Contents>[] behaviors)
		{
			Name = name;
			ID = id;
			VisualChar = visualChar;
			Transparent = transparent;
			Durability = durability;
			Size = size;
			Weight = weight;
			UseAction = useAction;
			Behaviors = behaviors;
		}

		public Contents(string name, int id, char visualChar, bool transparent, int durability, int size, float weight, bool container, int containerSpace, List<Contents> contained, Action<string[], Contents> useAction, Action<Contents>[] behaviors) : this(name, id, visualChar, transparent, durability, size, weight, useAction, behaviors)
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
		
		public bool HasTag(string compareTag)
		{
			foreach(string tag in Tags)
			{
				if (tag.Equals(compareTag, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Removes all null members of contained
		private void CleanOut()
		{
			if (Container)
			{
				while (Contained.Remove(null)) ;
			}
		}

		// Where TotalWeight is determined
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

		// Returns a string of all the contents within this container
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
		
		// Lists all containers within this container. Currently unused
		public string ListContainers()
		{
			Output.WriteLineTagged("Here are the containers of " + Name, Output.Tag.List);

			string output = "\n";

			output += "0. Self\n";
			for (int contentsIndex = 0; contentsIndex < Contained.Count; contentsIndex++)
			{
				Contents contents = Contained[contentsIndex];
				if (contents.Container)
				{
					Output.WriteLineToConsole((contentsIndex + 1) + ". " + contents.Name + "\tSize: " + contents.Size + "\tWeight: " + contents.Weight);
				}
			}
			return output;
		}

		// Reduce durability by damage. If it is less than zero, destroy object
		// CURRENTLY ONLY WORKS IF THE TILE IS ON THE GRID (no held items, no contained items)
		public void Damage(int damage, bool displayMessage = true)
		{
			// Are things breaking and not dying when they should be?
			// This statement is why... ;)
			// Just an easy fix to an annoying problem
			if (Durability <= 0)
			{
				return;
			}
			if (EventHandler.IdentifierEventMapping["OnContentsDamaged"].RunEvent(this.ID, new object[] {this, damage, displayMessage}) == EventHandler.EventResult.TerminateAction)
			{
				return;
			}
			if (ID == World.Player.Contents.ID)
			{
				Output.WriteLineTagged("You were damaged for " + damage, Output.Tag.World);
			}

			Durability -= damage;

			if (displayMessage)
			{
				Output.WriteLineTagged(Name + " was damaged for " + damage, Output.Tag.World);
			}
			if (Durability <= 0)
			{
				if (EventHandler.IdentifierEventMapping["OnContentsDestroyed"].RunEvent(this.ID, new object[] {this, damage, displayMessage}) == EventHandler.EventResult.TerminateAction)
				{
					return;
				}
				
				// Deleting all connections associated with contents
				foreach (string eventKey in EventHandler.IdentifierEventMapping.Keys)
				{
					for (int connectionIndex = 0; connectionIndex < EventHandler.IdentifierEventMapping[eventKey].ConnectionList.Count; connectionIndex++)
					{
						Connection connection = EventHandler.IdentifierEventMapping[eventKey].ConnectionList[connectionIndex];
						if (connection.TriggerContentsID == ID)
						{
							EventHandler.IdentifierEventMapping[eventKey].ConnectionList.Remove(connection);
							// v So that it doesn't skip anything
							connectionIndex--;
						}
					}
				}

				// Deleting dialogue line if it exists
				World.Dialogue.Remove(ID);

				if (Container && Contained.Count > 0)
				{
					if (!World.LoadedLevel.Grid.GetTileAtCoords(Coordinates, out Tile destroyTile))
					{
						return;
					}

					// HERE IS WHERE BAG IS DEFINED. Change as you wish

					destroyTile.Contents = new Contents
						(
						name: "bag",
						UniqueID(),
						visualChar: 'b',
						true,
						1,
						5,
						1,
						true,
						ContainerSpace,
						Contained,
						UseActions.DoesNothing,
						new Action<Contents>[] {GameEngine.Behavior.DoesNothing}
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
			Contents contentsToClone = new Contents(this.Name, UniqueID(), this.VisualChar, this.Transparent, this.Durability, this.Size, this.Weight, this.Container, this.ContainerSpace, this.Contained, this.UseAction, this.Behaviors);
			contentsToClone.Tags = this.Tags;
			return contentsToClone;
		}
	
		public static int UniqueID()
		{
			uniqueIndex++;
			return uniqueIndex;
		}
	}
}
