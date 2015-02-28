#region Using Statements
using System;

#endregion

namespace ZombieUnknown.Windows
{
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
            using (var game = new ZombieGameMain())
                game.Run();
        }
    }
}
