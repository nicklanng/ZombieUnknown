using System.Collections.Generic;
using Engine;
using Engine.AI.Steering;
using Engine.Drawing;
using Engine.Entities;
using Engine.InventoryObjects;
using Engine.Maps;
using Engine.Pathfinding;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI;
using ZombieUnknown.AI.FiniteStateMachines.Human;
using ZombieUnknown.InventoryObjects.Wearables;

namespace ZombieUnknown.Entities.Mobiles
{
    class Human : MobileEntity, IMovementBlocker
    {
        private readonly HumanMind _mind;

        public WearableRig Rig { get; private set; }
        public double Hunger { get; set; }
        public override float MaxVelocity { get { return 0.012f; } }

        public Human(string name, Vector2 mapPosition)
            : base(name, ResourceManager.GetSprite("human"), mapPosition)
        {
            Rig = new WearableRig();
            Rig.PutOn(new Backback());

            //_mind = new HumanMind(this);

            IsStatic = false;

            CurrentState = HumanStates.Instance.IdleState;
            CurrentState.OnEnter(this);

            var newLocationX = GameState.RandomNumberGenerator.Next(GameState.Map.Width - 1);
            var newLocationY = GameState.RandomNumberGenerator.Next(GameState.Map.Height - 1);


            var aStarSolver = new AStarSolver(GameState.PathfindingMap.GetNodeAt(MapPosition), GameState.PathfindingMap.GetNodeAt(new Coordinate(newLocationX, newLocationY)));
            aStarSolver.Solve();
            if (aStarSolver.Solution != null)
            {
                AvoidActorsBehavior = new AvoidActorsBehavior();
                FollowPathBehavior = new FollowPathBehavior(aStarSolver.Solution);
            }
            ContainmentBehavior = new ContainmentBehavior();
            AvoidanceBehavior = new AvoidanceBehavior();
            QueueBehavior = new QueueBehavior();
            
            Hunger = 20;
        }

        public override void Update()
        {
            //_mind.Think();
            //CurrentState.Update(this);

            //Hunger -= GameState.GameTime.ElapsedGameTime.TotalSeconds;
            if (FollowPathBehavior == null)
            {
                
                var newLocationX = GameState.RandomNumberGenerator.Next(GameState.Map.Width - 1);
                var newLocationY = GameState.RandomNumberGenerator.Next(GameState.Map.Height - 1);
                var aStarSolver = new AStarSolver(GameState.PathfindingMap.GetNodeAt(MapPosition), GameState.PathfindingMap.GetNodeAt(new Coordinate(newLocationX, newLocationY)));
                aStarSolver.Solve();
                if (aStarSolver.Solution != null)
                {
                    FollowPathBehavior = new FollowPathBehavior(aStarSolver.Solution);
                }
                
            }

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
    }
}
