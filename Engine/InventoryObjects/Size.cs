namespace Engine.InventoryObjects
{
    public class Size
    {
        public short Width { get; private set; }
        public short Height { get; private set; }

        public Size(short width, short height)
        {
            Width = width;
            Height = height;
        }

        public static bool operator <(Size left, Size right)
        {
            if (left.Width >= right.Width) return false;
            if (left.Height >= right.Height) return false;

            return true;
        }

        public static bool operator >(Size left, Size right)
        {
            if (right.Width >= left.Width) return false;
            if (right.Height >= left.Height) return false;

            return true;
        }
    }
}
