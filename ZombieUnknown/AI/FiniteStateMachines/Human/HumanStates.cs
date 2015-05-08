using Engine.AI.FiniteStateMachines;

namespace ZombieUnknown.AI.FiniteStateMachines.Human
{
    public class HumanStates
    {
        private static HumanStates _instance;

        public State IdleState;
        public State WalkingState;
        public State InteractingState;
        public State DyingState;

        private HumanStates()
        {
            IdleState = new IdleState();
            WalkingState = new WalkingState();
            InteractingState = new InteractingState();
            DyingState = new DyingState();

            IdleState.AddTransition("walk", WalkingState);
            IdleState.AddTransition("interact", InteractingState);
            IdleState.AddTransition("die", DyingState);

            WalkingState.AddTransition("idle", IdleState);

            InteractingState.AddTransition("walk", WalkingState);
            InteractingState.AddTransition("idle", IdleState);
            InteractingState.AddTransition("die", DyingState);
        }

        public static HumanStates Instance
        {
            get { return _instance ?? (_instance = new HumanStates()); }
        }

    }
}
