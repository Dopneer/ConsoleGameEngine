using System;
using System.Collections.Generic;


namespace ConsoleGameEngine.Samples.CRex
{
    public class Start : IStartGame
    {

        private static Core core;

        private static int SizeY = Console.WindowHeight;
        private static int SizeX = Console.WindowWidth;



        private static GameObject Cactus;

        private static GameObject TRex;

        private static Random ranom;

        private static bool IsGrounded = true;

        private static int LastSpawn = 0; // When last object was spawn (frames)


        private static List<GameObject> ActiveObjects = new List<GameObject>();

        public static void InitializeGameObjects()
        {
            Cactus = new GameObject(0, 0, new Symbol[,] { { new Symbol('|', 0x0F), new Symbol('|', 0x0F), }, { new Symbol('|', 0x0F), new Symbol('|', 0x0F), }, { new Symbol('|', 0x0F), new Symbol('|', 0x0F), } });

            TRex = new GameObject(0, 10, Program.rootPath + "TRex/dino1.txt");
            TRex.PosY = SizeY - (2 + TRex.Content.GetLength(0));

        }

        public static void SpawnCopy(GameObject obj)
        {
            GameObject clone = obj.GetClone();

            clone.PosX = SizeX;
            clone.PosY = SizeY - (clone.Content.GetLength(0) + 2);

            ActiveObjects.Add(clone);
            core.CreateObject(clone);
        }

        public static void StartGame()
        {

            SizeY = 30;
            SizeX = 80;

            InitializeGameObjects();

            

            // Создаем движок
            core = new Core();
            // Создаем окно отрисовки
            core.CreateWindow(SizeY, SizeX);

            GameObject.core = core;

            GameObject floor = new GameObject(SizeY - 2, 0, new Symbol[2, SizeX]);



            // Initialize texture of floor
            for(int i = 0; i < floor.Content.GetLength(1); i++)
            {
                floor.Content[0, i] = new Symbol('*', 0x0F);
                floor.Content[1, i] = new Symbol('*', 0x0F);
            }

            core.CreateObject(floor);
            core.CreateObject(TRex);

            SpawnCopy(Cactus);

            System.Threading.Thread Updater = new System.Threading.Thread(UpdateData);
            System.Threading.Thread InputReader = new System.Threading.Thread(core.UpdateInput);

            Updater.Start();
            InputReader.Start();


            // Main cicle
            while(true)
            {
                SpawnObjects();

                // Update field (move objects and other)
                UpdateField();

                core.DrawContent();

                System.Threading.Thread.Sleep(30);
            }


        }

        private static void SpawnObjects()
        {
            if(ranom.Next(0, 100) < 20 && LastSpawn > 20)
            {
                ActiveObjects.Add(GetRandomObject());
            }
        }

        private static GameObject GetRandomObject()
        {
            return Cactus.GetClone();
        }

        private static void UpdateData()
        {


            while(true)
            {

                if(core.LastInput == new ConsoleKey())
                {
                    System.Threading.Thread.Sleep(30);
                    continue;
                }
                
                    

                for (float i = 0; i <= 3.141; i += 0.02f)
                {
                    // Yeah science! Sin is cool!
                    TRex.PosY = SizeY - (2 + TRex.Content.GetLength(0)) - (int)Math.Ceiling(Math.Sin(i) * 15);

                    System.Threading.Thread.Sleep(10);
                }



                System.Threading.Thread.Sleep(100);

                core.LastInput = new ConsoleKey();

                TRex.PosY = SizeY - (2 + TRex.Content.GetLength(0));
            }
        }


        private static void Die()
        {
            foreach (GameObject obj in ActiveObjects)
            {
                obj.PosX--;
            }

            core.DrawContent();

            System.Threading.Thread.Sleep(30);

            foreach (GameObject obj in ActiveObjects)
            {
                obj.PosX--;
            }

            core.DrawContent();

            System.Threading.Thread.Sleep(30);

            foreach (GameObject obj in ActiveObjects)
            {
                obj.PosX--;
            }

            core.DrawContent();

            System.Environment.Exit(0);

        }

        private static void UpdateField()
        {
            List<GameObject> toRemove = new List<GameObject>();
            foreach(GameObject obj in ActiveObjects)
            {
                obj.PosX--;

                if(core.IsCrossing(TRex, obj))
                {
                    Die();
                }

                // If object leave screen
                if(obj.PosX + obj.Content.GetLength(1) < 0)
                {

                    toRemove.Add(obj);
                    
                }
            }

            foreach(GameObject obj in toRemove)
            {
                ActiveObjects.Remove(obj);
                core.Objects.Remove(obj);
            }



        }

    }
}
