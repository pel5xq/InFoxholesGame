#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace GameName1
{
//#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static Game mainGame;
        public static Game menuGame;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //using (var game = new Game1())
               // game.Run();
            //mainGame = new Game1();
            
            //mainGame.Run();
            menuGame = new Menu();

            menuGame.Exiting += delegate(object o, System.EventArgs e)
            {
                mainGame = new Game1();
                mainGame.Run();
            };

            menuGame.Run();
        }
    }
//#endif
}
