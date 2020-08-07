using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

// Бюджет игры и движка: 250 кошкодевочек и 1038 рублей 17.07.2020
// Бюджет игры и движка: 280 кошкодевочек и 1038 рублей 20.07.2020

namespace ConsoleGameEngine
{

    public delegate void InputDelegate(ConsoleKey input);
    public delegate void GameObjectDelegate(GameObject gameObject);
    public delegate void VoidDelegate();


    class Program
    {

        public static readonly int SizeY = Console.WindowHeight - 1;
        public static readonly int SizeX = Console.WindowWidth - 1;

        public static List<ConsoleKey> Inputs = new List<ConsoleKey>();

        public static bool GoWithMe;

        public static Random random = new Random();


        public static string rootPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())))) + "/assets/";


        [DllImport("libc")]
        private static extern int system(string exec);




        static void Main(string[] args)
        {






            //Uncomment necessary game to play

            //Samples.BlackJack.Start.StartGame();
            //Samples.Cnake.Start.StartGame();
            //Samples.EverlsatingSummer.Start.StartGame();








            /* dialogField.Animation = new VoidDelegate(() =>
            {

                for(int i = 0; i < 30; i++)
                {
                    dialogField.PosY--;
                    Thread.Sleep(50);
                }

            }); */



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

        /// <summary>
        /// May be null!
        /// </summary>
        public GameObject CursorHover; // Объект по вверх которого курсок



       


        public void UpdateInput()
        {
            while (true)
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

            if(Cursor != null)
            {
                
            }

            for (int i = 0; i < window.Content.GetLength(0); i++)
            {
                for (int j = 0; j < window.Content.GetLength(1); j++)
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


        }

        private void DrawObject(GameObject obj)
        {
            if (obj.Active == false)
            {
                return;
            }

            if (obj.Update != null)
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

            if(Cursor != null)
            {
                Console.WriteLine(CursorHover == null);
                // If we have cursorHover and cursor leave (not cross) cursor hover
                if(CursorHover != null && !IsCrossing(Cursor, CursorHover))
                {
                    
                    // Call cursor mouse out
                    foreach(VoidDelegate onMouseOut in CursorHover.OnMouseOut)
                    {
                        onMouseOut();
                    }
                    
                    // Delete cursor hover (cause now cursor not hover)
                    CursorHover = null;
                }

                for(int i = Objects.Count - 1; i >= 0; i--)
                {
                    if(IsCrossing(Cursor, Objects[i]))
                    {
                        CursorHover = Objects[i];
                        foreach(VoidDelegate onMouseOver in Objects[i].OnMouseOver)
                        {
                            onMouseOver();
                        }
                        
                    }
                }

            }


            if(BackGround != null)
            {
                DrawObject(BackGround);
            }
            
            if(Cursor != null)
            {
                DrawObject(Cursor);
            }

            foreach (GameObject obj in Objects)
            {

                DrawObject(obj);

            }

            if (Effect != null)
            {
                DrawObject(Effect);
            }

            foreach (GameObject obj in UI)
            {

                DrawObject(obj);

            }


        }

    }

}
