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

#region temp
        public static EventResult OnPlayerHealed()
        {

            return EventResult.Nothing;
        }

        public static EventResult OnPlayerEnterNewLevel()
        {

            return EventResult.Nothing;
        }
        public static EventResult OnPlayerMove()
        {
            
            return EventResult.Nothing;
        }
        public static EventResult OnPlayerUse()
        {

            return EventResult.Nothing;
        }
        public static EventResult OnPlayerInteract()
        {

            return EventResult.Nothing;
        }
        public static EventResult OnPlayerDied()
        {

            return EventResult.Nothing;
        }


        // Contents events
        public static EventResult OnAddContents()
        {

            return EventResult.Nothing;
        }
        public static EventResult OnRemoveContents()
        {
            return EventResult.Nothing;
        }
        public static EventResult OnContentsDestroyed(Contents target)
        {
            if (target.HasTag("explode"))
            {
                Behavior.DamageAllAround(target);
            }
            return EventResult.Nothing;
        }
        public static EventResult OnContentsMoved()
        {

            return EventResult.Nothing;
        }
        #endregion
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
                        if (target.HasTag("invulnerable"))
                        {
                            return EventResult.TerminateAction;
                        }
                        if (target.ID == World.Player.Contents.ID)
                        {
                            IdentifierEventMapping["OnPlayerDamaged"].RunEvent(target.ID, parameters);
                        }
                        return EventResult.Nothing;
                    }
                )
            },
            {
                "OnPlayerDamaged",
                new Event
                (
                    (parameters) => 
                    {
                        return EventResult.Nothing;
                    }
                )
            },
            {
                "OnContentsDestroyed",
                new Event
                (
                    (parameters) =>
                    {
                        Contents contents = (Contents)parameters[0];
                        if (contents.HasTag("explode"))
                        {
                            Behavior.DamageAllAround(contents);
                        }
                        return EventResult.Nothing;
                    }
                )
            },
        };
    }
}