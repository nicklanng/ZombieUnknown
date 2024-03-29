﻿using System.Collections.Generic;
using Engine.Entities.Interactions;

namespace Engine.AI.FiniteStateMachines
{
    public interface IInteractableState
    {
        Dictionary<string, Interaction> Interactions { get; }
    }
}
