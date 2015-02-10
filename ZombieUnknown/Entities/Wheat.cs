using Engine.Entities;
using Engine.Maps;
using Engine;
using ZombieUnknown.AI.FiniteStateMachines.Wheat;

namespace ZombieUnknown.Entities
{
    public class Wheat : VisibleEntity
    {
        public double Growth = 0;

        public Wheat(string name, Coordinate mapPosition)
            : base(name, ResourceManager.GetSprite("wheat"), mapPosition)
        {
            CurrentState = WheatStates.Instance.SownState;
            CurrentState.OnEnter(this);
        }

        public override void Update()
        {
            CurrentState = CurrentState.Update(this);

            base.Update();
        }

        public override AccessPosition[] AccessPositions
        {
            get
            {
                return new[]
                       {
                           new AccessPosition(Direction.South.Coordinate, Direction.North),
                           new AccessPosition(Direction.North.Coordinate, Direction.South),
                           new AccessPosition(Direction.West.Coordinate, Direction.East),
                           new AccessPosition(Direction.East.Coordinate, Direction.West),
                           new AccessPosition(Direction.NorthEast.Coordinate, Direction.SouthWest),
                           new AccessPosition(Direction.SouthEast.Coordinate, Direction.NorthWest),
                           new AccessPosition(Direction.NorthWest.Coordinate, Direction.SouthEast),
                           new AccessPosition(Direction.SouthWest.Coordinate, Direction.NorthEast),
                       };
            }
        }

        public bool IsGrown
        {
            get { return CurrentState == WheatStates.Instance.GrownState; }
        }
    }
}