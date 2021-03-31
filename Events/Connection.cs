using System;

namespace GameEngine
{
    class Connection
    {
        public int TriggerContentsID;
        public int ResultContentsID;
        public string ResultType;
        public string ResultInformation;

        public Connection(int triggerContentsID, int resultContentsID, string resultType, string resultInformation)
        {
            TriggerContentsID = triggerContentsID;
            ResultContentsID = resultContentsID;
            
            ResultType = resultType;
            ResultInformation = resultInformation;
        }

        public void PerformResult()
        {
            if (!World.GetContentsFromID(ResultContentsID, out Contents contents))
            {
                // Contents was destroyed - or does not exist
                return;
            }
            // Making lowercase provides less ability to mess up
            switch (ResultType.ToLower())
            {
                case "useaction":
                    if (!UseActions.TryGetAction(ResultInformation, out Action<string[], Contents> useAction))
                    {
                        Output.WriteLineTagged("Contents: " + contents.Name + " (" + contents.ID + ") has an incorrect UseAction.", Output.Tag.Error);
                        return;
                    }
                    useAction(new string[0], contents);
                    break;
                case "behavior":
                    if (!Behavior.TryGetBehaviors(ResultInformation.Split(","), out Action<Contents>[] behaviors))
                    {
                        Output.WriteLineTagged("Contents: " + contents.Name + " (" + contents.ID + ") has an incorrect Behavior list.", Output.Tag.Error);
                        return;
                    }
                    foreach(Action<Contents> behavior in behaviors)
                    {
                        behavior(contents);
                    }
                    break;
                case "interact":
                    contents.UseAction(new string[0], contents);
                    break;
                case "display":
                    Output.WriteLineTagged(ResultInformation, Output.Tag.World);
                    break;
            }
        }

    }
}