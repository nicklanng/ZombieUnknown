using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Entities.Interactions
{
    public static class InteractionManager
    {
        private static readonly IList<IInteraction> Interactions = new List<IInteraction>();

        public static IEnumerable<IInteraction<TSubject, TActor>> GetInteractionsForTypes<TActor, TSubject>()
        {
            return Interactions.Where(i => i.ActorType.IsParentOrSameTypeAs<TActor>() &&
                                           i.SubjectType.IsParentOrSameTypeAs<TSubject>())
                               .Cast<IInteraction<TSubject, TActor>>();
        }

        public static void RegisterInteraction<TInteraction>() where TInteraction : IInteraction, new()
        {
            Interactions.Add(Activator.CreateInstance<TInteraction>());
        }

        public static TInteraction GetInteractionOfType<TInteraction>() where TInteraction : IInteraction
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