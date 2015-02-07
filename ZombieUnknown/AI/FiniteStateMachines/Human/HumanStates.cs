using Engine.AI.FiniteStateMachines;

namespace ZombieUnknown.AI.FiniteStateMachines.Human
{
    public class HumanStates
    {
        private static HumanStates _instance;

        public State IdleState;
        public State WalkingState;
        public State InteractingState;

        private HumanStates()
        {
            IdleState = new IdleState();
            WalkingState = new WalkingState();
            InteractingState = new InteractingState();

            IdleState.AddTransition("walk", WalkingState);
            IdleState.AddTransition("interact", InteractingState);

            WalkingState.AddTransition("idle", IdleState);
            WalkingState.AddTransition("interact", InteractingState);

            InteractingState.AddTransition("walk", WalkingState);
            InteractingState.AddTransition("idle", IdleState);
        }

        public static HumanStates Instance
        {
            get { return _instance ?? (_instance = new HumanStates()); }
        }

    }
}
