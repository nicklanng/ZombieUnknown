using System;

namespace Engine.Entities.Interactions
{
    public class TargetInteraction<TSubject, TActor, TInteraction> : ITargetInteraction where TInteraction : class, IInteraction<TSubject, TActor>, new()
                                                                                        where TActor : MobileEntity
                                                                                        where TSubject : PhysicalEntity
    {
        private TInteraction _interaction;

        private TInteraction Interaction
        {
            get { return _interaction ?? (_interaction = Activator.CreateInstance<TInteraction>()); }
        }

        public MobileEntity Actor
        {
            get { return _actor; }
        }

        public PhysicalEntity Subject
        {
            get { return _subject; }
        }

        private readonly TActor _actor;
        private readonly TSubject _subject;

        private TargetInteraction(TSubject subject, TActor actor)
        {
            _actor = actor;
            _subject = subject;
        }

        public static ITargetInteraction Create(TSubject subject, TActor actor)
        {
            return new TargetInteraction<TSubject, TActor, TInteraction>(subject, actor);
        }

        public void Interact()
        {
            Interaction.Interact(_subject, _actor);
        }

        public bool IsPossible()
        {
            return Interaction.IsPossible(_subject, _actor);
        }

        public int MillisToCompleteAction
        {
            get { return Interaction.MillisToCompleteAction; }
        }
    }
}