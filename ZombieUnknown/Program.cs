﻿#region Using Statements
using System;

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
        private static void Main()
        {
            using (var game = new ZombieGameMain())
                game.Run();
        }
    }
#endif
}
