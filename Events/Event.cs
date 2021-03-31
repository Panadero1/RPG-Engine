using System.Collections.Generic;

namespace GameEngine
{
    class Event
    {
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
            RunConnections(triggerID);
            return _eventAction(parameters);
        }
    }
}