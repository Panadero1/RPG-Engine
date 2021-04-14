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
						// Contents moveContents Coord direction
						
						(parameters) =>
						{
							Contents moveContents = (Contents)parameters[0];
							Coord direction = (Coord)parameters[1];

							if (moveContents.HasTag("nomove"))
							{
								Output.WriteLineTagged("Object cannot be moved", Output.Tag.World);
								return EventResult.TerminateAction;
							}
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
							Contents holding = (Contents)parameters[0];

							if (holding.HasTag("nouse"))
							{
								Output.WriteLineTagged("Object cannot be used", Output.Tag.World);
								return EventResult.TerminateAction;
							}
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
							Contents interactContents = (Contents)parameters[0];

							if (interactContents.HasTag("nointeract"))
							{
								Output.WriteLineTagged("Object cannot be interacted with", Output.Tag.World);
								return EventResult.TerminateAction;
							}
							return EventResult.Nothing;
						}
					)
				},
				{
					"OnLooked",
					new Event
					(
						// Contents lookContents

						(parameters) =>
						{
							Contents lookContents = (Contents)parameters[0];
							if (lookContents.HasTag("nolook"))
							{
								Output.WriteLineTagged("Contents on tile are indescribable/unnoticable", Output.Tag.World);
								return EventResult.TerminateAction;
							}
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
					"OnPlayerMovedNear",
					new Event
					(
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
							Contents addedContents = (Contents)parameters[0];

							if (addedContents.HasTag("noadd"))
							{
								Output.WriteLineTagged("Contents cannot be added to a container", Output.Tag.World);
								return EventResult.TerminateAction;
							}
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
							Contents removedContents = (Contents)parameters[0];

							if (removedContents.HasTag("noadd"))
							{
								Output.WriteLineTagged("Contents cannot be removed from its container", Output.Tag.World);
								return EventResult.TerminateAction;
							}
							return EventResult.Nothing;
						}
					)
				},
				{
					"OnUpdate",
					new Event
					(
						(parameters) =>
						{
							return EventResult.Nothing;
						}
					)
				}
		};
	}
}