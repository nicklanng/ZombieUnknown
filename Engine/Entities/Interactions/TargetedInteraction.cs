namespace Engine.Entities.Interactions
{
    public class TargetedInteraction<TInteraction, TSubject, TActor> : ITargetedInteraction where TInteraction : InteractionSingleton<TSubject, TActor>
                                                                                            where TActor : MobileEntity
                                                                                            where TSubject : PhysicalEntity
    {
        public MobileEntity Actor
        {
            get { return _actor; }
        }

        public PhysicalEntity Subject
        {
            get { return _subject; }
        }

        private readonly TInteraction _interaction;
        private readonly TSubject _subject;
        private readonly TActor _actor;

        public TargetedInteraction(TInteraction interaction, TSubject subject, TActor actor)
        {
            _interaction = interaction;
            _subject = subject;
            _actor = actor;
        }

        public void Interact()
        {
            _interaction.Interact(_subject, _actor);
        }

        public bool IsPossible()
        {
            return _interaction.IsPossible(_subject, _actor);
        }

        public int MillisToCompleteAction
        {
            get { return _interaction.MillisToCompleteAction; }
        }
    }
}