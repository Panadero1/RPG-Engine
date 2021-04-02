using System.Collections.Generic;

namespace GameEngine
{
    class Event
    {
        public static readonly int callMaximum = 1022;

        public static int currentCallCount = 0;

        public delegate EventHandler.EventResult EventActionDelegate(object[] parameters);

        private EventActionDelegate _eventAction;

        public List<Connection> ConnectionList = new List<Connection>();
        
        public Event(EventActionDelegate eventAction)
        {
            _eventAction = eventAction;
        }

        private void RunConnections(int triggerID)
        {
            foreach(Connection connection in ConnectionList)
            {
                if (connection.TriggerContentsID == triggerID)
                {
                    connection.PerformResult();
                }
            }
        }

        public EventHandler.EventResult RunEvent(int triggerID, object[] parameters = null)
        {
            currentCallCount++;
            if (currentCallCount > callMaximum)
            {
                Output.WriteLineTagged("Game is manually breaking out of event system to prevent stack overflow (too many events at once)", Output.Tag.Error);
                return EventHandler.EventResult.Nothing;
            }
            RunConnections(triggerID);
            EventHandler.EventResult result = _eventAction(parameters);
            currentCallCount--;
            return result;
        }
    }
}