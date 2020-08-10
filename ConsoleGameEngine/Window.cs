using System;
using System.Collections.Generic;

namespace ConsoleGameEngine
{
    public class Window
    {
        public int SizeY { get; private set; } // Height
        public int SizeX { get; private set; } // Width

        public Symbol[,] Content { get; private set; }

        public Window(int sizeY, int sizeX)
        {
            SizeY = sizeY;
            SizeX = sizeX;

            Content = new Symbol[sizeY, sizeX];

            for (int i = 0; i < SizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    Content[i, j] = new Symbol(' ', 0x00);
                }
            }
        }

        public void DrawObject(GameObject obj)
        {
            Console.SetCursorPosition(obj.PosX, obj.PosY);

            for(int i = 0; i < obj.Content.GetLength(0); i++)
            {
                for(int j = 0; j < obj.Content.GetLength(1); j++)
                {
                    if(obj.Content[i, j].Color == 0x52)
                    {
                        continue;
                    }
                    Console.Write("\x1b[48;5;" + obj.Content[i, j].Color + "m" + obj.Content[i, j].Value);
                }
                Console.WriteLine();
            }
        }

        public void Draw()
        {
            Console.Clear();
            if (Program.OperationSystem == "windows")
            {
                for (int i = 0; i < SizeY; i++)
                {
                    for (int j = 0; j < SizeX; j++)
                    {
                        if (Content[i, j].Color == 0x52)
                        {
                            continue;
                        }
                        Console.BackgroundColor = (ConsoleColor)(Content[i, j].Color % 16);
                        Console.Write(Content[i, j].Value);
                    }
                }
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                for (int i = 0; i < SizeY; i++)
                {
                    for (int j = 0; j < SizeX; j++)
                    {
                        if (Content[i, j].Color == 0x52)
                        {
                            continue;
                        }
                        Console.Write("\x1b[48;5;" + Content[i, j].Color + "m" + Content[i, j].Value);
                    }
                    Console.WriteLine();
                }
                Console.Write("\x1b[48;5;255m");
            }
        }

    }
}
