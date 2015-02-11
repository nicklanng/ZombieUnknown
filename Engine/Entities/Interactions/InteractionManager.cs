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

        public static ITargetedInteraction CreateTargetedInteraction<TInteraction, TSubject, TActor>(TSubject subject, TActor actor) where TInteraction : InteractionSingleton<TSubject, TActor> where TSubject : PhysicalEntity where TActor : MobileEntity
        {
            var interaction = GetInteractionOfType<TInteraction>();
            return new TargetedInteraction<TInteraction, TSubject, TActor>(interaction, subject, actor);
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
    }
}