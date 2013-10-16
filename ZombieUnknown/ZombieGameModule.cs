using Ninject.Modules;

namespace ZombieUnknown
{
    class ZombieGameModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ZombieGameMain>().ToSelf();
        }
    }
}
