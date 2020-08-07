using System;
using System.Threading;

namespace ConsoleGameEngine.Samples.Tetris
{
    public class Start : IStartGame
    {

        private const int SizeY = 30;
        private const int SizeX = 50;

        private static Core core;

        public static void StartGame()
        {
            // Создаем движок
            core = new Core();
            // Создаем окно отрисовки
            core.CreateWindow(SizeY, SizeX);

            GameObject.core = core;

            GameObject backGround = new GameObject(0, 0, new Symbol[SizeY, SizeX], true);

            for(int i = 0; i < SizeY; i++)
            {
                for(int j = 0; j < SizeX; j++)
                {
                    backGround.Content[i, j] = new Symbol(' ', 0xEF);
                }
            }
            

            



            Thread ContentDrawer = new Thread(DrawContent);
            ContentDrawer.Start();


        }

        private static void DrawContent()
        {
            while(true)
            {
                core.DrawContent();

                Thread.Sleep(100);
            }
        }

    }
}
