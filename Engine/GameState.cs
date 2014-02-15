using Engine.Entities;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine
{
    public static class GameState
    {
        public static Map Map { get; set; }
        public static DrawableEntity Selected { get; set; }
        public static PathfindingMap PathfindingMap { get; set; }
        public static GameTime GameTime { get; set; }
    }
}
