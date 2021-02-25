using System;
using System.Collections.Generic;

namespace ConsoleGameEngine.Samples.minesweeper
{
    public class Start : IStartGame
    {

        public readonly static int SizeX = 20;
        public readonly static int SizeY = 20;

        public static int CursorX;
        public static int CursorY;


        public static int MineCount = 0; // Mine count (может быть меньше 0, если игрок укажит неверные мины)
        public static int ActualMineCount = 0; // Real mine count (уменьшается только если укажут верную мину)

        private static Core core;

        public static Cell[,] GameField;


        private static Random random = new Random();

        public static void StartGame()
        {

            CursorX = 0;
            CursorY = 0;

            // Создаем движок
            core = new Core();
            // Создаем окно отрисовки
            core.CreateWindow(SizeX, SizeY);


            GameObject.core = core;

            GameField = new Cell[SizeY, SizeX];


            // Initialize field;
            for (int i = 0; i < SizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    GameField[i, j] = new Cell(false);
                    GameField[i, j].IsOpened = false;
                }
            }

            MineCount = (SizeX * SizeX) / 4;
            ActualMineCount = MineCount;

            // Spawn bombs (25% of cells)
            for (int i = 0; i < MineCount; i++)
            {
                Cell cell = GameField[random.Next(0, SizeY), random.Next(0, SizeY)];

                // If bomb on cell
                if (cell.IsBomb)
                {
                    // Get another iteration
                    i--;
                    continue;
                }

                // if not bomb
                cell.SetBomb();


            }

            GameField[0, 0].IsBomb = false;
            GameField[0, 0].BombCount = 0;


            GameField[0, 1].IsBomb = false;
            GameField[0, 1].BombCount = 0;

            GameField[0, 2].IsBomb = false;
            GameField[0, 2].BombCount = 0;

            GameField[1, 0].IsBomb = false;
            GameField[1, 0].BombCount = 0;

            GameField[1, 1].IsBomb = false;
            GameField[1, 1].BombCount = 0;

            GameField[1, 2].IsBomb = false;
            GameField[1, 2].BombCount = 0;

            GameField[2, 0].IsBomb = false;
            GameField[2, 0].BombCount = 0;

            GameField[2, 1].IsBomb = false;
            GameField[2, 1].BombCount = 0;

            GameField[2, 2].IsBomb = false;
            GameField[2, 2].BombCount = 0;

            GameField[0, 0].OpenCell();


            // Set numbers of bombs

            SetNumbers(); // Тут пиздец

            while (ActualMineCount > 0)
            {
                

                DrawField();

                Console.SetCursorPosition(CursorX, CursorY);
                Console.Write('#');

                CursorControl();

            }

            Console.WriteLine("You win!");

        }

        public static void DrawField()
        {
            Console.Clear();
            for (int i = 0; i < SizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    Console.Write(GameField[i, j].Texture);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Mines left: " + MineCount);

        }

        private static void CursorControl()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    CursorX--;
                    if (CursorX < 0)
                        CursorX = SizeX - 1;
                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    CursorX++;
                    if (CursorX >= SizeX)
                        CursorX = 0;
                    break;

                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    CursorY--;
                    if (CursorY < 0)
                        CursorY = SizeY - 1;
                    break;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    CursorY++;
                    if (CursorY >= SizeY)
                        CursorY = 0;
                    break;
                case ConsoleKey.Spacebar:
                    GameField[CursorY, CursorX].OpenCell();
                    break;
                default:
                    GameField[CursorY, CursorX].MarkMine();
                    break;
            }
        }














        private static void SetNumbers()
        {
            // А я ведь говорил
            // Top and bottom of field
            for (int i = 1; i < SizeX - 1; i++)
            {
                if (!GameField[0, i].IsBomb)
                {

                    int BombCount = 0;

                    if (GameField[1, i - 1].IsBomb)
                        BombCount++;

                    if (GameField[1, i].IsBomb)
                        BombCount++;

                    if (GameField[1, i + 1].IsBomb)
                        BombCount++;

                    if (GameField[0, i - 1].IsBomb)
                        BombCount++;

                    if (GameField[0, i + 1].IsBomb)
                        BombCount++;

                    GameField[0, i].BombCount = BombCount;

                }

                if (!GameField[SizeY - 1, i].IsBomb)
                {

                    int BombCount = 0;

                    if (GameField[SizeY - 2, i - 1].IsBomb)
                        BombCount++;

                    if (GameField[SizeY - 2, i].IsBomb)
                        BombCount++;

                    if (GameField[SizeY - 2, i + 1].IsBomb)
                        BombCount++;

                    if (GameField[SizeY - 1, i - 1].IsBomb)
                        BombCount++;

                    if (GameField[SizeY - 1, i + 1].IsBomb)
                        BombCount++;

                    GameField[SizeY - 1, i].BombCount = BombCount;

                }

            }


            // Left side

            for (int i = 1; i < SizeY - 1; i++)
            {
                if (!GameField[i, 0].IsBomb)
                {

                    int BombCount = 0;

                    if (GameField[i - 1, 1].IsBomb)
                        BombCount++;

                    if (GameField[i, 1].IsBomb)
                        BombCount++;

                    if (GameField[i + 1, 1].IsBomb)
                        BombCount++;

                    if (GameField[i + 1, 0].IsBomb)
                        BombCount++;

                    if (GameField[i - 1, 0].IsBomb)
                        BombCount++;

                    GameField[i, 0].BombCount = BombCount;

                }

                if (!GameField[i, SizeX - 1].IsBomb)
                {

                    int BombCount = 0;

                    if (GameField[i - 1, SizeX - 2].IsBomb)
                        BombCount++;

                    if (GameField[i, SizeX - 2].IsBomb)
                        BombCount++;

                    if (GameField[i + 1, SizeX - 2].IsBomb)
                        BombCount++;

                    if (GameField[i + 1, SizeX - 1].IsBomb)
                        BombCount++;

                    if (GameField[i - 1, SizeX - 1].IsBomb)
                        BombCount++;

                    GameField[i, SizeX - 1].BombCount = BombCount;

                }

            }



            // Center of field
            for (int i = 1; i < SizeY - 1; i++)
            {
                for (int j = 1; j < SizeX - 1; j++)
                {

                    if (GameField[i, j].IsBomb)
                    {
                        continue;
                    }

                    int BombCount = 0;

                    if (GameField[i - 1, j - 1].IsBomb)
                        BombCount++;

                    if (GameField[i - 1, j].IsBomb)
                        BombCount++;

                    if (GameField[i - 1, j + 1].IsBomb)
                        BombCount++;

                    if (GameField[i, j - 1].IsBomb)
                        BombCount++;

                    if (GameField[i, j + 1].IsBomb)
                        BombCount++;

                    if (GameField[i + 1, j - 1].IsBomb)
                        BombCount++;

                    if (GameField[i + 1, j].IsBomb)
                        BombCount++;

                    if (GameField[i + 1, j + 1].IsBomb)
                        BombCount++;

                    GameField[i, j].BombCount = BombCount;

                }
            }

            // Corners
            if (!GameField[0, 0].IsBomb)
            {
                int BombCount = 0;

                if (GameField[0, 1].IsBomb)
                    BombCount++;

                if (GameField[1, 0].IsBomb)
                    BombCount++;

                if (GameField[1, 1].IsBomb)
                    BombCount++;

                GameField[0, 0].BombCount = BombCount;

            }

            if (!GameField[SizeY - 1, SizeX - 1].IsBomb)
            {
                int BombCount = 0;

                if (GameField[SizeY - 1, SizeX - 2].IsBomb)
                    BombCount++;

                if (GameField[SizeY - 2, SizeX - 1].IsBomb)
                    BombCount++;

                if (GameField[SizeY - 2, SizeX - 2].IsBomb)
                    BombCount++;

                GameField[SizeY - 1, SizeX - 1].BombCount = BombCount;

            }

            if (!GameField[SizeY - 1, 0].IsBomb)
            {
                int BombCount = 0;

                if (GameField[SizeY - 1, 1].IsBomb)
                    BombCount++;

                if (GameField[SizeY - 2, 0].IsBomb)
                    BombCount++;

                if (GameField[SizeY - 2, 1].IsBomb)
                    BombCount++;

                GameField[SizeY - 1, 0].BombCount = BombCount;

            }

            if (!GameField[0, SizeX - 1].IsBomb)
            {
                int BombCount = 0;

                if (GameField[1, SizeX - 2].IsBomb)
                    BombCount++;

                if (GameField[1, SizeX - 1].IsBomb)
                    BombCount++;

                if (GameField[0, SizeX - 2].IsBomb)
                    BombCount++;

                GameField[0, SizeX - 1].BombCount = BombCount;

            }
        }


    }



    


}
