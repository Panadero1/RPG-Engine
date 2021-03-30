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

        private void RunConnections()
        {
            foreach(Connection connection in ConnectionList)
            {
                connection.PerformResult();
            }
        }
        public EventHandler.EventResult RunEvent(object[] parameters = null)
        {
            RunConnections();
            return _eventAction(parameters);
        }
    }
}