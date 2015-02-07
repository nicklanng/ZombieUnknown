using Engine.AI.FiniteStateMachines;

namespace ZombieUnknown.AI.FiniteStateMachines.Wheat
{
    public class WheatStates
    {
        private static WheatStates _instance;

        public State SownState { get; set; }
        public State GrownState { get; set; }
        public State GrowingState { get; set; }

        private WheatStates()
        {
            SownState = new SownState();
            GrowingState = new GrowingState();
            GrownState = new GrownState();

            SownState.AddTransition("growing", GrowingState);
            GrowingState.AddTransition("grown", GrownState);
        }

        public static WheatStates Instance
        {
            get { return _instance ?? (_instance = new WheatStates()); }
        }

    }
}
