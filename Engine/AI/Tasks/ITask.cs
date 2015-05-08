using Engine.Entities;

namespace Engine.AI.Tasks
{
    public interface ITask
    {
        PhysicalEntity Assigned { get; }
        string Action { get; }
        PhysicalEntity Target { get; }
        bool IsComplete { get; }

        bool IsAvailable();
        void Pick(PhysicalEntity actor);
        void PutBack();
        void Complete();
    }
}
