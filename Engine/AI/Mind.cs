using System.Collections.Generic;
using System.Linq;
using Engine.Entities;
using Microsoft.Xna.Framework;

namespace Engine.AI
{
    public class Mind<T> where T : Entity
    {
        private T _entity;
        private readonly Stack<IGoal> _goals;

        public Mind(T entity)
        {
            _entity = entity;

            _goals = new Stack<IGoal>();
        }

        public void Think(GameTime gameTime)
        {
            if (!_goals.Any())
            {
                return;
            }

            _goals.Peek().Process(gameTime);
        }
    }
}
