using System;
using System.Threading;

namespace ConsoleGameEngine.Samples.EverlsatingSummer
{
    public class Start : IStartGame
    {

        private static string rootPath = ConsoleGameEngine.Program.rootPath;

        public static void StartGame()
        {
            Core core = new Core();

            core.clickButtons.Add(ConsoleKey.Spacebar);

            GameObject.core = core;

            Thread InputReader = new Thread(core.UpdateInput);
            InputReader.Start();




            core.CreateWindow(Program.SizeY, Program.SizeX);


            GameObject gameObject = new GameObject(0, 0, new Symbol[,] { { new Symbol(' ', 0x66), new Symbol(' ', 0x66) } }, true);

            // Create dialog field. 20% height of console. 100% - 2px width of console.
            DialogField dialogField = new DialogField((Program.SizeY) - (int)Math.Ceiling((float)Program.SizeY / 100 * 20), 1, new Symbol[(int)Math.Ceiling((float)Program.SizeY / 100 * 20) - 1, Program.SizeX - 2]);

            core.UI.Add(dialogField);

            core.DrawContent();

            // Main menu
            /*
            while (true)
            {
                switch (core.LastInput)
                {
                    case ConsoleKey.W:
                        core.Cursor.PosY--;
                        break;
                    case ConsoleKey.S:
                        core.Cursor.PosY++;
                        break;
                    case ConsoleKey.D:
                        core.Cursor.PosX++;
                        break;
                    case ConsoleKey.A:
                        core.Cursor.PosX--;
                        break;
                }


                core.DrawContent();

                core.LastInput = ConsoleKey.O;

                Thread.Sleep(100);
            } */
        }
    }
}
