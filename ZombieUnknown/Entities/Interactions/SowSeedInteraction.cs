﻿using Engine;
using Engine.Entities;
using Engine.Entities.Interactions;

namespace ZombieUnknown.Entities.Interactions
{
    class SowSeedInteraction : Interaction
    {
        public static string Text = "Sow Seed";

        public override int MillisToCompleteAction { get { return 1000; } }
        
        public override void Interact(MobileEntity actor, PhysicalEntity subject)
        {
            GameController.DeleteEntity(subject);
            GameController.SpawnEntity(new Wheat("wheat", subject.MapPosition));
        }
    }
}
