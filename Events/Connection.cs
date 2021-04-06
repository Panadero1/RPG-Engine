using System;
using System.Linq;

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
					case "edit":
						string[] info = ResultInformation.Split(" ");

						switch (info[0])
						{
								case "Name":
									contents.Name = info[1];
									break;
								case "Visual Character":
									contents.VisualChar = info[1][0];
									break;
								case "Transparent":
									if (info[1] == "change")
									{
										contents.Transparent = !contents.Transparent;
									}
									else
									{
										contents.Transparent = bool.Parse(info[1]);
									}
									break;
								case "Durability":
									if (info[1] == "up")
									{
										contents.Durability++;
									}
									else if (info[1] == "down")
									{
										contents.Durability--;
									}
									else
									{
										contents.Durability = int.Parse(info[1]);
									}
									break;
								case "Size":
									if (info[1] == "up")
									{
										contents.Size++;
									}
									else if (info[1] == "down")
									{
										contents.Size--;
									}
									else
									{
										contents.Size = int.Parse(info[1]);
									}
									break;
								case "Weight":
									if (info[1] == "up")
									{
										contents.Weight++;
									}
									else if (info[1] == "down")
									{
										contents.Weight--;
									}
									else
									{
										contents.Weight = float.Parse(info[1]);
									}
									break;
								case "Use Action":
									if (!UseActions.TryGetAction(info[1], out Action<string[], Contents> action))
									{
										return;
									}
									contents.UseAction = action;
									break;
								case "Behavior":
									if (!Behavior.TryGetBehaviors(info[1].Split(","), out Action<Contents>[] behavior))
									{
										return;
									}
									contents.Behaviors = behavior;
									break;
						}
						break;
				}
		}

	}
}