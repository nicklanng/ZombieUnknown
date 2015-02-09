﻿using System.Linq;
using Engine;
using Engine.Entities;
using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    internal class SowSeedInteraction : Interaction
    {
        public static string Text = "Sow Seed";

        public override int MillisToCompleteAction
        {
            get { return 1000; }
        }

        public SowSeedInteraction(PhysicalEntity subject) : base(subject)
        {
        }

        public override void Interact(PhysicalEntity entity)
        {
            if (IsPossible(entity))
            {
                var humanActor = (Human)entity;

                humanActor.Rig.GetInventories().TakeItemOfType<WheatSeedObject>();
                GameController.DeleteEntity(Subject);
                GameController.SpawnEntity(new Wheat("wheat", Subject.MapPosition));
            }
        }

        public override bool IsPossible(PhysicalEntity actor)
        {
            var humanActor = actor as Human;

            if (humanActor == null)
            {
                return false;
            }
            return humanActor.Rig.GetInventories().HasItemOfType<WheatSeedObject>();
        }
    }
}