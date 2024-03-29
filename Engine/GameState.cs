﻿using System;
using System.Collections.Generic;
using Engine.AI.Steering;
using Engine.AI.Tasks;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public static class GameState
    {
        public static Map Map { get; set; }
        public static PhysicalEntity Selected { get; set; }
        public static PathfindingMap PathfindingMap { get; set; }
        public static GameTime GameTime { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static Entity MainCharacter { get; set; }
        public static Random RandomNumberGenerator = new Random();
        public static TaskList TaskList { get; set; }

        public static PhysicalEntity InteractionObject { get; set; } // temporary
        public static PhysicalEntity ZombieTarget { get; set; }
        public static List<IActor> Actors { get; set; }
        public static VirtualScreen VirtualScreen { get; set; }
        public static ICamera Camera { get; set; }
    }
}
