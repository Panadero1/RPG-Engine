using System.Collections.Generic;

namespace GameEngine
{
	static class EventHandler
	{
		public enum EventResult
		{
				TerminateAction,
				Nothing
		};


		// Player events
		public static Event OnPlayerDamaged = new Event
		(
				(parameters) => 
				{
					return EventResult.Nothing;
				}
		);

		public static Dictionary<string, Event> IdentifierEventMapping = new Dictionary<string, Event>()
		{
				{
					"OnContentsDamaged",
					new Event
					(
						// Contents target, int damage, bool displayMessage
						(parameters) =>
						{
								Contents target = (Contents)parameters[0];

								bool displayMessage = (bool)parameters[2];
								if (target.HasTag("invulnerable"))
								{
									if (displayMessage)
									{
										Output.WriteLineTagged("Object is invulnerable", Output.Tag.World);
									}
									return EventResult.TerminateAction;
								}
								return EventResult.Nothing;
						}
					)
				},
				{
					"OnContentsDestroyed",
					new Event
					(
						// Contents target, int damage, bool displayMessage
						(parameters) =>
						{
								Contents contents = (Contents)parameters[0];

								bool displayMessage = (bool)parameters[2];
								if (contents.HasTag("nodestroy"))
								{
									if (displayMessage)
									{
										Output.WriteLineTagged("Object is invulnerable", Output.Tag.World);
									}
									return EventResult.TerminateAction;
								}
								if (contents.HasTag("explode"))
								{
									Behavior.DamageAllAround(contents);
								}
								if (contents.ID == World.Player.Contents.ID)
								{
									return IdentifierEventMapping["OnPlayerDied"].RunEvent(World.Player.Contents.ID);
								}
								return EventResult.Nothing;
						}
					)
				},
				{
					"OnPlayerHealed",
					new Event
					(
						// No parameters yet
						(parameters) =>
						{
								return EventResult.Nothing;
						}
					)
				},
				{
					"OnContentsNewLevel",
					new Event
					(
						// Coord direction
						(parameters) =>
						{
								return EventResult.Nothing;
						}
					)
				},
				{
					"OnContentsMove",
					new Event
					(
						// Coord direction
						(parameters) =>
						{
								return EventResult.Nothing;
						}
					)
				},
				{
					"OnUse",
					new Event
					(
						// Contents holding
						(parameters) =>
						{
								return EventResult.Nothing;
						}
					)
				},
				{
					"OnInteract",
					new Event
					(
						// Contents interactContents
						(parameters) =>
						{
								return EventResult.Nothing;
						}
					)
				},
				{
					"OnPlayerDied",
					new Event
					(
						// No params
						(parameters) =>
						{
								return EventResult.Nothing;
						}
					)
				},
				{
					"OnContentsAdded",
					new Event
					(
						// Contents addedContents
						(parameters) =>
						{
								return EventResult.Nothing;
						}
					)
				},
				{
					"OnContentsRemoved",
					new Event
					(
						// Contents removedContents
						(parameters) =>
						{
								return EventResult.Nothing;
						}
					)
				},
		};
	}
}