using System;
using System.Collections.Generic;
using Engine;
using Engine.AI.Steering;
using Engine.Drawing;
using Engine.Entities;
using Engine.Input;
using Engine.InventoryObjects;
using Engine.Pathfinding;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI;
using ZombieUnknown.AI.FiniteStateMachines.Human;
using ZombieUnknown.InventoryObjects.Wearables;
using Console = Engine.Drawing.UI.Console;

namespace ZombieUnknown.Entities.Mobiles
{
    public class Human : MobileEntity, IMovementBlocker, IClickable
    {
        private readonly HumanMind _mind;

        public WearableRig Rig { get; private set; }
        public double Hunger { get; set; }
        public override float MaxVelocity { get { return 0.015f; } }

        public Human(string name, Vector2 mapPosition)
            : base(name, ResourceManager.GetSprite("human"), mapPosition)
        {
            Rig = new WearableRig();
            Rig.PutOn(new Backback());

            _mind = new HumanMind(this);

            IsStatic = false;

            CurrentState = HumanStates.Instance.IdleState;
            CurrentState.OnEnter(this);

            AvoidActorsBehavior = new AvoidActorsBehavior();
            ContainmentBehavior = new ContainmentBehavior();
            AvoidanceBehavior = new AvoidanceBehavior();
            QueueBehavior = new QueueBehavior();

            Hunger = 20;

            ClickLocationManager.Instance.RegisterClickLocation(this);
        }

        public override void Update()
        {
            _mind.Think();
            //CurrentState.Update(this);

            //Hunger -= GameState.GameTime.ElapsedGameTime.TotalSeconds;

            base.Update();
        }

        public override IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, LightValue, new Vector2(0.000001f, 0.000001f));
        }

        public void GiveItem(IInventoryObject inventoryObject)
        {
            var inventories = Rig.GetInventories();
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

        public string GetCurrentStateName()
        {
            return CurrentState.Name;
        }

        public bool BlocksTile { get; private set; }
        public bool BlocksDiagonals { get; private set; }


        public Rectangle Bounds
        {
            get
            {
                var coords = new Vector2();
                if (GameState.Camera.GetScreenCoordinates(MapPosition, out coords))
                {
                    var topLeft = GameState.VirtualScreen.ConvertVirtualScreenCoordinatesToScreenCoordinates(coords);
                    var offset = GameState.VirtualScreen.ConvertVirtualScreenCoordinatesToScreenCoordinates(Sprite.Offset);
                    var size = GameState.VirtualScreen.ConvertVirtualScreenCoordinatesToScreenCoordinates(new Vector2(Sprite.Width, Sprite.Height));
                    return new Rectangle((int)(topLeft.X - offset.X), (int)(topLeft.Y - offset.Y), (int)size.X, (int)size.Y);
                }
                return new Rectangle();
            }
        }
        public bool IsEnabled { get { return true; } }

        public event EventHandler OnClick;
        public void Click()
        {
            Console.WriteLine(Name);
            if (OnClick != null)
            {
                OnClick(this, new EventArgs());
            }
        }

        public bool TestClick(Vector2 clickPosition)
        {
            var inBounds = Bounds.Contains(clickPosition);
            if (!inBounds) return false;

            var positionInBounds = clickPosition - new Vector2(Bounds.Location.X, Bounds.Location.Y);
            var scaledPosition = GameState.VirtualScreen.ConvertScreenCoordinatesToVirtualScreenCoordinates(positionInBounds);
            return Sprite.IsPixelFilledIn(scaledPosition);
        }
    }
}
