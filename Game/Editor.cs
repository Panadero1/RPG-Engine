using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
  static class Editor
  {
    public enum State 
    {
        Map,
        Level
    }
    
    public static State EditorState;

    public static Tile Brush;

  }
}