using System.Collections.Generic;
using Engine.Entities.Interactions;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public interface IInteractable
    {
        AccessPosition[] AccessPositions { get; }
        Vector2 MapPosition { get; }
        Dictionary<string, IInteraction> Interactions { get; }
    }
}

