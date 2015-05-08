using Engine.AI.Steering;
using Engine.Entities;

namespace Engine
{
    public static class GameController
    {
        public static void DeleteEntity(PhysicalEntity entity)
        {
            entity.Delete();
            GameState.Map.RemoveEntity(entity);
        }

        public static void SpawnEntity(PhysicalEntity entity)
        {
            GameState.Map.AddEntity(entity);
            if (entity is IActor)
            {
                GameState.Actors.Add(entity as IActor);
            }
        }
    }
}
