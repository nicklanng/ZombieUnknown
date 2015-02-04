using System;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public interface IInteractable
    {
        AccessPosition[] AccessPositions { get; }
        Vector2 MapPosition { get; }
    }
}

