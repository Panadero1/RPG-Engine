using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
	static class Output
	{
		private static Dictionary<Tag, string> _tagMapping = new Dictionary<Tag, string>()
		{
				{ Tag.Dialogue, "Dialogue" },
				{ Tag.Error, "Error" },
				{ Tag.World, "World" },
				{ Tag.Prompt, "Prompt" },
				{ Tag.List, "List" },
				{ Tag.Text, "Text" },
				{ Tag.Tutorial, "Tutorial" },
				{ Tag.Info, "Info" }
		};
		public enum Tag
		{
				Dialogue,
				Error,
				World,
				Prompt,
				List,
				Text,
				Tutorial,
				Info,
		}
		public static void WriteLineTagged(string line, Tag tag)
		{
				WriteLineToConsole("[" + _tagMapping[tag] + "] " + line);
		}
		public static void WriteToConsole(string text)
		{
				Console.Write(text);
		}
		public static void WriteLineToConsole(string line)
		{
				Console.WriteLine(line);
		}
	}
}