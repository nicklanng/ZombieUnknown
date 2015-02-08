using System;
using System.Collections.Generic;
using Engine;
using Engine.Drawing;
using Engine.Entities;
using Engine.InventoryObjects;
using Engine.Maps;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI;
using ZombieUnknown.AI.FiniteStateMachines.Human;
using ZombieUnknown.InventoryObjects.Wearables;
using Console = Engine.Drawing.Console;

namespace ZombieUnknown.Entities.Mobiles
{
    class Human : MobileEntity
    {
        private readonly WearableRig _rig;
        private readonly HumanMind _mind;

        public double Hunger { get; set; }
        public override float Speed { get { return 15; } }

        public Human(string name, Coordinate mapPosition)
            : base(name, ResourceManager.GetSprite("human"), mapPosition)
        {
            _rig = new WearableRig();
            _rig.PutOn(new Backback());

            _mind = new HumanMind(this);

            IsStatic = false;

            CurrentState = HumanStates.Instance.IdleState;
            CurrentState.OnEnter(this);

            Hunger = 20;
        }

        public override void Update()
        {
            _mind.Think();
            CurrentState.Update(this);

            Hunger -= GameState.GameTime.ElapsedGameTime.TotalSeconds;
            Console.WriteLine("Hunger: " + Math.Ceiling(Hunger));

            base.Update();
        }

        public override IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, LightValue, new Vector2(0.000001f, 0.000001f));
        }

        public void GiveItem(IInventoryObject inventoryObject)
        {
            var inventories = _rig.GetInventories();
            foreach (var inventory in inventories)
            {
                var availableSlot = inventory.GetAvailableSlot(inventoryObject);
                if (availableSlot != null)
                {
                    inventory.Insert(availableSlot, inventoryObject);
                    return;
                }
            }
        }
    }
}
