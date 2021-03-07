using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    static class Output
    {
        private static Dictionary<tag, string> tagMapping = new Dictionary<tag, string>()
        {
            { tag.Dialogue, "Dialogue" },
            { tag.Error, "Error" },
            { tag.World, "World" },
            { tag.Prompt, "Prompt" },
            { tag.List, "List" },
            { tag.Text, "Text" },
            { tag.Tutorial, "Tutorial" }
        };
        public enum tag
        {
            Dialogue,
            Error,
            World,
            Prompt,
            List,
            Text,
            Tutorial,
        }
        public static void WriteLineTagged(string line, tag tag)
        {
            WriteLineToConsole("[" + tagMapping[tag] + "] " + line);
        }
        public static void WriteLineToConsole(string line)
        {
            Console.WriteLine(line);
        }
    }
}