using System;
using System.Threading;
using System.Collections.Generic;

namespace ConsoleGameEngine.Samples.Cnake
{
    public class Start : IStartGame
    {

        // Cnake = Console snake =3

        private static bool FoodSpawn = false; // Food exist
        public static GameObject Food;
        public static List<GameObject> SnakeBody = new List<GameObject>();

        private static int SizeX = 20;
        private static int SizeY = 20;

        public static Random random = new Random();

        private static Core core;

        public static void StartGame()
        {
            // Создаем движок
            core = new Core();
            // Создаем окно отрисовки
            core.CreateWindow(SizeX, SizeY);


            GameObject.core = core;
            core.BackgroundColor = 0x00;


            Food = new GameObject(-10, -10, new Symbol[,] { { new Symbol(' ', 0x04) } }, true);

            SnakeBody.Add(new GameObject(5, 5, new Snake[,]
            {
                { new Snake(' ', 0x02, "right") }
            }, true));
            SnakeBody.Add(new GameObject(5, 4, new Snake[,]
           {
                { new Snake(' ', 0x02, "right") }
           },true));
            SnakeBody.Add(new GameObject(5, 3, new Snake[,]
           {
                { new Snake(' ', 0x02, "right") }
           }, true));
            SnakeBody.Add(new GameObject(5, 2, new Snake[,]
           {
                { new Snake(' ', 0x02, "right") }
           }, true));


            


            core.DrawContent();



            // Создаем новый поток для чтения ввода пользователя
            Thread inputReader = new Thread((SnakeBody[0].Content[0, 0] as Snake).Control);
            inputReader.Start();

            Thread gameDrawer = new Thread(DrawContent);
            gameDrawer.Start();
        }

        private static void DrawContent()
        {
            while (true)
            {
                SpawnFood(Food);

                MoveSnake();

                core.DrawContent();

                Thread.Sleep(100);
            }
        }

        public static void SpawnFood(GameObject Food)
        {
            if (!FoodSpawn & random.Next(0, 100) <= 7)
            {
                Food.Move(random.Next(2, SizeY - 3), random.Next(2, SizeX - 3));
                FoodSpawn = true;
            }
        }

        public static void MoveSnake()
        {
            (SnakeBody[0].Content[0, 0] as Snake).Direction = (SnakeBody[0].Content[0, 0] as Snake).setDir;
            switch ((SnakeBody[0].Content[0, 0] as Snake).Direction)
            {
                case "up":
                    SnakeBody[0].Content[0, 0].Value = '^';
                    break;
                case "left":
                    SnakeBody[0].Content[0, 0].Value = '<';
                    break;
                case "right":
                    SnakeBody[0].Content[0, 0].Value = '>';
                    break;
                case "down":
                    SnakeBody[0].Content[0, 0].Value = 'V';
                    break;
            }

            bool spawn = false;
            int posX = 0;
            int posY = 0;
            string Dir = "up";

            for (int i = 0; i < SnakeBody.Count; i++)
            {
                (SnakeBody[i].Content[0, 0] as Snake).Direction = (SnakeBody[i].Content[0, 0] as Snake).setDir;
                if (i != 0 && SnakeBody[i].PosX == SnakeBody[0].PosX && SnakeBody[i].PosY == SnakeBody[0].PosY)
                {
                    Console.WriteLine("You snake Ded(");
                    System.Environment.Exit(0);
                }


                GameObject gameObject = SnakeBody[i];


                gameObject.Move((gameObject.Content[0, 0] as Snake).Direction);

                if (Food.PosY == gameObject.PosY && Food.PosX == gameObject.PosX)
                {
                    Food.Move(-10, -10);
                    FoodSpawn = false;

                    GameObject lastSnake = SnakeBody[SnakeBody.Count - 1];

                    spawn = true;
                    posY = lastSnake.PosY;
                    posX = lastSnake.PosX;
                    Dir = (lastSnake.Content[0, 0] as Snake).Direction;

                    SnakeBody.Add(new GameObject(posY, posX, new Snake[,] { { new Snake(' ', 0x02, Dir) } }, true));

                }


                // If out of map (Y)
                if (gameObject.PosY >= SizeY)
                    // Move on 0 coordinate
                    gameObject.Move(0, gameObject.PosX);

                // If out of map (Y)
                if (gameObject.PosY < 0)
                    // Move on window sizee coordinate
                    gameObject.Move(SizeY, gameObject.PosX);



                // If out of map (X)
                if (gameObject.PosX >= SizeX)
                    // Move on 0 coordinate
                    gameObject.Move(gameObject.PosY, 0);

                // If out of map (Y)
                if (gameObject.PosX < 0)
                    // Move on window sizee coordinate
                    gameObject.Move(gameObject.PosY, SizeX); 
            }

            for (int i = SnakeBody.Count - 1; i > 0; i--)
            {

                (SnakeBody[i].Content[0, 0] as Snake).Direction = (SnakeBody[i - 1].Content[0, 0] as Snake).Direction;
                (SnakeBody[i].Content[0, 0] as Snake).setDir = (SnakeBody[i - 1].Content[0, 0] as Snake).Direction;

            }

            if (spawn)
            {
                spawn = false;
                (SnakeBody[SnakeBody.Count - 1].Content[0, 0] as Snake).Direction = Dir;
                SnakeBody[SnakeBody.Count - 1].Move(posY, posX);
            }

        }

    }
}
