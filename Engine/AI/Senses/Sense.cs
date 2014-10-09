namespace Engine.AI.Senses
{
    public abstract class Sense
    {
        public int Range;
        protected int MapSize;
        protected float[,] SenseMap { get; private set; }

        protected Sense(int range)
        {
            Range = range;
            MapSize = 2 * range + 1;
            SenseMap = new float[MapSize, MapSize];
        }
    }
}
