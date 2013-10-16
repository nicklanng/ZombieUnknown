#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;

#endregion

namespace ZombieUnknown
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using(var kernel = new StandardKernel(new ZombieGameModule()))
            {
                using (var game = kernel.Get<ZombieGameMain>())
                {
                    game.Run();
                }
            }
        }
    }
#endif
}
