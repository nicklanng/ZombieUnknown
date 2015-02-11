using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Entities.Interactions
{
    public static class InteractionManager
    {
        private static readonly IList<IInteractionTypeProvider> Interactions = new List<IInteractionTypeProvider>();

        public static IEnumerable<IInteraction<TSubject, TActor>> GetInteractionsForType<TActor, TSubject>()
        {
            return Interactions.Where(i => i.ActorType.IsParentOrSameTypeAs<TActor>() &&
                                           i.SubjectType.IsParentOrSameTypeAs<TSubject>())
                               .Cast<IInteraction<TSubject, TActor>>();
        }

        public static void RegisterInteraction<TInteraction>() where TInteraction : IInteractionTypeProvider, new()
        {
            Interactions.Add(Activator.CreateInstance<TInteraction>());
        }

        public static TargetedInteractionCreator<TSubject, TActor> CreateTargetedInteraction<TSubject, TActor>(TSubject subject, TActor actor) where TSubject : PhysicalEntity where TActor : MobileEntity
        {
            return new TargetedInteractionCreator<TSubject, TActor>(subject, actor);
        }

        private static TInteraction GetInteractionOfType<TInteraction>()
        {
            return (TInteraction)Interactions.Single(i => i is TInteraction);
        }

        private static bool IsParentOrSameTypeAs<T>(this Type type)
        {
            var comparisonType = typeof(T);
            return (comparisonType == type ||
                    comparisonType.IsSubclassOf(type));
        }

        public class TargetedInteractionCreator<TSubject, TActor> where TSubject : PhysicalEntity where TActor : MobileEntity
        {
            private readonly TActor _actor;
            private readonly TSubject _subject;

            public TargetedInteractionCreator(TSubject subject, TActor actor)
            {
                _actor = actor;
                _subject = subject;
            }

            public TargetedInteraction<TInteraction, TSubject, TActor> ForInteraction<TInteraction>() where TInteraction : InteractionSingleton<TSubject, TActor>
            {
                var interaction = GetInteractionOfType<TInteraction>();
                return new TargetedInteraction<TInteraction, TSubject, TActor>(interaction, _subject, _actor);
            }
        }
    }
}