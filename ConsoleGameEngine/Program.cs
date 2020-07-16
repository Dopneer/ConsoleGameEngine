using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConsoleGameEngine.Scenes;

namespace ConsoleGameEngine
{

    public delegate void InputDelegate(ConsoleKey input);
    public delegate void GameObjectDelegate(GameObject gameObject);


    class Program
    {

        public static readonly int SizeY = Console.WindowHeight - 1;
        public static readonly int SizeX = Console.WindowWidth - 1;

        public static List<ConsoleKey> Inputs = new List<ConsoleKey>();

        public static bool GoWithMe;

        public static Random random = new Random();


        private static string rootPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())))) + "/assets/";

        static void Main(string[] args)
        {


            Core core = new Core();

            core.clickButtons.Add(ConsoleKey.Spacebar);

            GameObject.core = core;

            Thread InputReader = new Thread(core.UpdateInput);
            InputReader.Start();

            

            GameObject Mush = new GameObject(0, 0, new Symbol[SizeY, SizeX], false);

            Mush.Update.Add(new GameObjectDelegate(Mush =>
            {
                byte[] BadColors = new byte[] { 0xFC, 0xFD, 0xFE, 0xFF };

                for (int i = 0; i < Mush.Content.GetLength(0); i++)
                {
                    for (int j = 0; j < Mush.Content.GetLength(1); j++)
                    {
                        Mush.Content[i, j] = new Symbol(' ', 0x52);
                    }
                }

                // Random count of bad lines
                for (int i = 0; i < new Random().Next((int)Math.Ceiling((float)SizeY / 100 * 10), (int)Math.Ceiling((float)SizeY / 100 * 20)); i++)
                {
                    int Height = new Random().Next(0, Mush.Content.GetLength(0)); // Random height to bad line
                    
                    // For all width
                    for(int j = 0; j < Mush.Content.GetLength(1); j++)
                    {
                        Mush.Content[Height, j] = new Symbol(' ', BadColors[new Random().Next(0, BadColors.Length)]);
                    }
                }
            }));

            

            core.BackGround = new GameObject(0, 0, rootPath + "backgrounds/menu.txt");

            core.CreateWindow(SizeY, SizeX);

            DialogField dialogField = new DialogField((SizeY) - (int)Math.Ceiling((float)SizeY / 100 * 20), 1, new Symbol[(int)Math.Ceiling((float)SizeY / 100 * 20) - 1, SizeX - 2]);

            core.UI.Add(dialogField);



            Prologue.Story(core, dialogField, Mush);
            

            
        }

    }

    public class Core
    {

        public Window window { get; private set; }

        public List<GameObject> Objects { get; private set; } = new List<GameObject>();
        public List<GameObject> UI { get; private set; } = new List<GameObject>();

        public List<InputDelegate> InputSubscribers { get; private set; } = new List<InputDelegate>();

        public ConsoleKey LastInput;

        // Кнопки которые симулируют левый кноп мышкой
        public List<ConsoleKey> clickButtons = new List<ConsoleKey>();

        public GameObject Effect;

        public GameObject Cursor = null;
        public GameObject BackGround;



        public void UpdateInput()
        {
            while(true)
            {
                LastInput = Console.ReadKey().Key;
 
            }
        }


        // Is objects crossing
        public bool IsCrossing(GameObject gameObject1, GameObject gameObject2)
        {
            if ((gameObject1.PosX + gameObject1.Content.GetLength(1) - 1) >= gameObject2.PosX && gameObject1.PosX <= (gameObject2.PosX + gameObject2.Content.GetLength(1) - 1))
            {
                if ((gameObject1.PosY + gameObject1.Content.GetLength(0) - 1) >= gameObject2.PosY && gameObject1.PosY <= (gameObject2.PosY + gameObject2.Content.GetLength(0) - 1))
                {
                    return true;
                }
            }
            return false;
        }

        public void ReadInput()
        {
            while (true)
            {

                ConsoleKey input = LastInput;

                // If user press special button, which simulate click
                if (clickButtons.Contains(input) && Cursor != null)
                {
                    // Check crossing of objects
                    foreach (GameObject obj in Objects)
                    {
                        // If cursor cross object
                        if (IsCrossing(Cursor, obj))
                        {
                            // Call onclick object functions
                            foreach (GameObjectDelegate gameObjectDelegate in obj.OnClick)
                            {
                                if (gameObjectDelegate != null)
                                {
                                    gameObjectDelegate(obj);
                                }
                            }

                            // Call onclick cursor functions
                            foreach (GameObjectDelegate gameObjectDelegate in Cursor.OnClick)
                            {
                                if (gameObjectDelegate != null)
                                {
                                    gameObjectDelegate(obj);
                                }
                            }
                        }
                    }

                }

                foreach (InputDelegate subscriber in InputSubscribers)
                {
                    subscriber(input);
                }

            }
        }

        public void CreateWindow(int height, int width)
        {
            window = new Window(height, width);

            for (int i = 0; i < window.SizeY; i++)
            {
                for (int j = 0; j < window.SizeX; j++)
                {
                    window.Content[i, j] = new Symbol(' ', 0xFF);
                }
            }
        }

        public void CreateObject(GameObject gameObject)
        {
            Objects.Add(gameObject);
        }


        public void DrawContent()
        {

            for(int i = 0; i < window.Content.GetLength(0); i++)
            {
                for(int j = 0; j < window.Content.GetLength(1); j++)
                {
                    window.Content[i, j] = new Symbol(' ', 0xFF);
                }
            }

            foreach (GameObject obj in Objects)
            {

                if (obj.Update != null)
                {
                    foreach (GameObjectDelegate gameObjectDelegate in obj.Update)
                    {
                        gameObjectDelegate(obj);
                    }
                }
            }

            UpdateData();

            window.Draw();

            Thread.Sleep(66);


        }

        private void DrawObject(GameObject obj)
        {
            if (obj.Active == false)
            {
                return;
            }

            if(obj.Update != null)
            {
                foreach (GameObjectDelegate gameObjectDelegate in obj.Update)
                {
                    gameObjectDelegate(obj);
                }
            }

            // Если объект уходит в минус по координатам
            // То его мы его обрезаем (отрисовываем с нужного места)
            int drawFromY = 0;

            if (obj.PosY < 0)
            {
                drawFromY = -obj.PosY;
            }

            int drawFromX = 0;

            if (obj.PosX < 0)
            {
                drawFromX = -obj.PosX;
            }

            int posY = obj.PosY;
            int posX = obj.PosX;

            // Если объект ушел в минус по координатам
            // То мы обрезаем его часть, которая ушла в минус
            if (posY < 0)
                posY = 0;
            if (posX < 0)
                posX = 0;


            for (int i = 0; (drawFromY + i) < obj.Content.GetLength(0) && (posY + i) < window.SizeY; i++)
            {
                for (int j = 0; (drawFromX + j) < obj.Content.GetLength(1) && (posX + j) < window.SizeX; j++)
                {

                    if (obj.Content[drawFromY + i, drawFromX + j].Color == 0x52)
                    {
                        continue;
                    }

                    window.Content[i + posY, j + posX] = obj.Content[drawFromY + i, drawFromX + j];

                }
            }
        }

        /// <summary>
        /// Отрисовываем каждый объект, что есть в массиве с объектами.
        /// </summary>
        private void UpdateData()
        {

            DrawObject(BackGround);

            foreach (GameObject obj in Objects)
            {

                DrawObject(obj);

            }

            if(Effect != null)
            {
                DrawObject(Effect);
            }

            foreach(GameObject obj in UI)
            {

                DrawObject(obj);

            }


        }

    }

}
