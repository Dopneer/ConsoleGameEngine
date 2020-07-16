using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleGameEngine
{

    public class Select : GameObject
    {

        public ConsoleKey UserSelect(ConsoleKey[] CorrectButtons)
        {

            Active = true;

            core.DrawContent();

            while(!CorrectButtons.Contains(core.LastInput))
            {
                System.Threading.Thread.Sleep(50);
            }

            Active = false;

            core.DrawContent();

            return core.LastInput;

        }

        public Select(int PosY, int PosX, Symbol[,] Content, string[] Text, bool AddToCore = true) : base (PosY, PosX, Content, AddToCore)
        {

            Active = false;

            for (int i = 0; i < Content.GetLength(0); i++)
            {
                for (int j = 0; j < Content.GetLength(1); j++)
                {
                    Content[i, j] = new Symbol(' ', 0xEE);
                }
            }

            // For each variant
            for (int i = 0; i < Text.Length; i++)
            {

                // Draw Text
                for(int j = 0; j < Text[i].Length; j++)
                {
                    Content[i * 2 + 1, (Content.GetLength(1) / 2) - (Text[i].Length / 2) + j] = new Symbol(Text[i][j], 0xFE);
                }

            }
        }
    }

    public class DialogField : GameObject
    {

        private int CursorPosX;
        private int CursorPosY;

        public DialogField(int PosY, int PosX, Symbol[,] Content, bool AddToCore = true) : base (PosY, PosX, Content, AddToCore)
        {
            CursorPosX = 2;
            CursorPosY = 2;

            for (int i = 0; i < Content.GetLength(0); i++)
            {
                for (int j = 0; j < Content.GetLength(1); j++)
                {
                    Content[i, j] = new Symbol(' ', 0x52);
                }
            }

            for (int i = 0; i < Content.GetLength(0); i++)
            {
                Content[i, 0] = new Symbol(' ', 0x00);
                Content[i, Content.GetLength(1) - 1] = new Symbol(' ', 0x00);

            }

            for (int i = 0; i < Content.GetLength(1); i++)
            {
                Content[0, i] = new Symbol(' ', 0x00);
                Content[Content.GetLength(0) - 1, i] = new Symbol(' ', 0x00);
            }
        }

        

        public void ClearField()
        {
            CursorPosX = 2;
            CursorPosY = 2;

            for (int i = 0; i < Content.GetLength(0); i++)
            {
                for (int j = 0; j < Content.GetLength(1); j++)
                {
                    Content[i, j] = new Symbol(' ', 0x52);
                }
            }

            for (int i = 0; i < Content.GetLength(0); i++)
            {
                Content[i, 0] = new Symbol(' ', 0x00);
                Content[i, Content.GetLength(1) - 1] = new Symbol(' ', 0x00);

            }

            for (int i = 0; i < Content.GetLength(1); i++)
            {
                Content[0, i] = new Symbol(' ', 0x00);
                Content[Content.GetLength(0) - 1, i] = new Symbol(' ', 0x00);
            }

            core.DrawContent();

        }

        public void DrawText(string Text, int delay = 20)
        {
            foreach(char chr in Text)
            {
                if (CursorPosX >= Content.GetLength(1) - 2)
                {
                    CursorPosY += 2;
                    CursorPosX = 2;
                }
                    
                if (CursorPosY >= Content.GetLength(0) - 2)
                {
                    return;
                }

                CursorPosX++;

                try
                {
                    Content[CursorPosY, CursorPosX] = new Symbol(chr, 0xFF);

                    Console.SetCursorPosition(PosX + CursorPosX, PosY + CursorPosY);
                    Console.Write("\x1b[48;5;" + 0xFF + "m" + chr);

                    System.Threading.Thread.Sleep(delay);


                }
                catch(Exception e)
                {
                    
                }

            }

            core.LastInput = new ConsoleKey();

            while(!core.clickButtons.Contains(core.LastInput))
            {
                System.Threading.Thread.Sleep(50);
            }

            core.LastInput = new ConsoleKey();

        }
    }

    public class GameObject
    {

        public int PosX { get; private set; }
        public int PosY { get; private set; }

        public bool Active = true;

        public Symbol[,] Content;

        public static Core core;

        public List<GameObjectDelegate> OnClick = new List<GameObjectDelegate>();
        public List<GameObjectDelegate> Update = new List<GameObjectDelegate>();

        public void Move(string direction)
        {
            switch (direction)
            {

                case "up":
                    PosY--;
                    break;
                case "down":
                    PosY++;
                    break;
                case "left":
                    PosX--;
                    break;
                case "right":
                    PosX++;
                    break;
            }
        }

        private void Flip(string direction)
        {
            direction = direction.ToLower();
            byte[,] Result;
            switch (direction)
            {
                case "clockwise":
                    Result = new byte[Content.GetLength(1), Content.GetLength(0)];

                    break;
                case "counterclockwise":
                    Result = new byte[Content.GetLength(1), Content.GetLength(0)];
                    break;
            }
        }

        public void Move(int PosY, int PosX)
        {
            this.PosY = PosY;
            this.PosX = PosX;
        }

        public GameObject(int PosY, int PosX, string path, bool AddToCore = true)
        {
            try
            {
                this.PosX = PosX;
                this.PosY = PosY;

                string[] line = File.ReadAllLines(path);

                // If file empty create invisible pixel (more safe than empty array)
                if(line.Length < 1)
                {
                    Content = new Symbol[,] { { new Symbol(' ', 0x52) } };

                    return;
                }

                Content = new Symbol[line.Length, line[0].Replace(" ", "").Length / 2];

                int i = 0;
                foreach (string str in line)
                {
                    byte[] Result = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(i => byte.Parse(i, System.Globalization.NumberStyles.HexNumber)).ToArray();

                    for (int j = 0; j < line[0].Replace(" ", "").Length / 2; j++)
                    {
                        try
                        {
                            Content[i, j] = new Symbol(' ', Result[j]);
                        }
                        catch(Exception e)
                        {
                            Content[i, j] = new Symbol(' ', 0x52);
                            continue;
                        }
                    }

                    i++;

                }

                if(core != null & AddToCore)
                {
                    core.Objects.Add(this);
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public GameObject(int PosY, int PosX, Symbol[,] Content, bool AddToCore)
        {
            this.PosY = PosY;
            this.PosX = PosX;

            this.Content = Content;

            if (core != null & AddToCore)
            {
                core.Objects.Add(this);
            }
        }

        public void Move(ConsoleKey direction)
        {
            switch (direction)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    PosY--;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    PosY++;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    PosX--;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    PosX++;
                    break;
            }
        }

    }

}
