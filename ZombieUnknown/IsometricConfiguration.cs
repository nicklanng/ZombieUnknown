using Engine;

namespace ZombieUnknown
{
    class IsometricConfiguration : IIsometricConfiguration
    {
        public short FloorWidth { get { return 32; } }
        public short FloorHeight { get { return 16; } }
        public short TileHeight { get { return 48; } }
    }
}
