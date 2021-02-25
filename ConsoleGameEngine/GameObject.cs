using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleGameEngine
{

    public class TextField : GameObject
    {

        // TextField attributes:

        // newline - автоперенос текста на новую линюю, если дашел до конца.
        // autoRrsize - автоматическое расширение Content, чтобы текст не ухадил за пределы текстуры.
        // margintop - отступ текста от 0 координаты по Y
        // marginleft - Отступ текста от 0 координаты по X


        private int MarginTop = 0;
        private int MarginLeft = 0;

        private bool NewLine = false;
        private bool autoResize = false;


        // Нужен чтобы удалить текст. Мы рисут текст прямо в Content поэтому чтобы вернуть обычную текстуру мы копируем ее.
        private Symbol[,] originalContent;


        public TextField(int PosY, int PosX, Symbol[,] Content, string Text, List<string> Attributes = null, bool AddToCore = false) : base (PosY, PosX, Content, AddToCore)
        {
            this.Attributes = Attributes;

            // Дали ли нам пустой массив Content
            bool emptyContent = false;

            for(int i = 0; i < Content.GetLength(0); i++)
            {
                for(int j = 0; j < Content.GetLength(1); j++)
                {
                    if (Content[i, j] == null)
                    {
                        emptyContent = true;
                        break;
                    }
                       
                }

                if (emptyContent)
                    break;

            }

            if(emptyContent)
            {
                for (int i = 0; i < Content.GetLength(0); i++)
                {
                    for (int j = 0; j < Content.GetLength(1); j++)
                    {

                        Content[i, j] = new Symbol(' ', 0xFF);

                    }

                }
            }

            originalContent = Content;

            if(Attributes == null)
            {
                Attributes = new List<string>();
                Attributes.Add("empty");
            }

            for(int i = 0; i < Attributes.Count; i++)
            {

                Attributes[i] = Attributes[i].ToLower();

                if(Attributes[i].Contains("margintop"))
                {
                    Attributes[i].Replace("margintop", "");

                    try
                    {
                        MarginTop = Int32.Parse(Attributes[i]);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Wrong attribute margintop");
                    }
                }
                else if (Attributes[i].Contains("marginleft"))
                {
                    Attributes[i].Replace("marginleft", "");

                    try
                    {
                        MarginLeft = Int32.Parse(Attributes[i]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Wrong attribute marginleft");
                    }
                }
                else if (Attributes[i].Contains("autoresize"))
                {
                    autoResize = true;
                }
                else if(Attributes[i].Contains("newline"))
                {
                    NewLine = true;
                }
                else if (Attributes[i].Contains("autoresize"))
                {

                }


            }

            DrawText(Text);

            
        }

        public void DrawText(string Text)
        {
            // If text out of bounds
            if(MarginTop > Content.GetLength(0) || MarginLeft > Content.GetLength(1))
            {
                // Don't draw -_-
                return;
            }

            // Resize
            if(autoResize && !NewLine)
            {

                int newWidth = (Text.Length + MarginLeft);

                // If new width bigger then old with
                if(newWidth > Content.GetLength(1))
                {
                    Symbol[,] NewContent = new Symbol[Content.GetLength(0), newWidth];

                    // Copy old content
                    for(int i = 0; i < Content.GetLength(0); i++)
                    {
                        for(int j = 0; j < Content.GetLength(1); j++)
                        {
                            NewContent[i, j] = Content[i, j];
                        }
                    }

                    // Fill empty places in new content
                    for(int i = 0; i < NewContent.GetLength(0); i++)
                    {
                        for(int j = Content.GetLength(1) - 1; j < NewContent.GetLength(1); j++)
                        {
                            NewContent[i, j] = new Symbol(' ', 0xFF);
                        }
                    }

                    Content = NewContent;

                }
            }

            if(NewLine)
            {
                // Сколько нужно символов чтобы переход на новую строку сработал
                int needFill = Content.GetLength(1) - MarginLeft;
                int fildedLines = (Text.Length / needFill);
                int newHeight = fildedLines + MarginTop;

                if(newHeight > Content.GetLength(0))
                {
                    Symbol[,] NewContent = new Symbol[Content.GetLength(0), newHeight];

                    // Copy old content
                    for (int i = 0; i < Content.GetLength(0); i++)
                    {
                        for (int j = 0; j < Content.GetLength(1); j++)
                        {
                            NewContent[i, j] = Content[i, j];
                        }
                    }

                    // Fill empty places in new content
                    for (int i = Content.GetLength(0) - 1; i < NewContent.GetLength(0); i++)
                    {
                        for (int j = 0; j < NewContent.GetLength(1); j++)
                        {
                            NewContent[i, j] = new Symbol(' ', 0xFF);
                        }
                    }

                    Content = NewContent;

                }
            }




            int offsetY = 0;
            int offsetX = 0;

            for(int i = 0; i < Text.Length; i++)
            {
                if(MarginLeft + offsetX > Content.GetLength(1))
                {

                    if(NewLine)
                    {
                        offsetX = 0;
                        offsetY++;
                    }
                    else
                    {
                        break;
                    }
                }

                Content[MarginTop + offsetY, MarginLeft + offsetX] = new Symbol(Text[i], Content[MarginTop, MarginLeft].Color);



                offsetX++;
            }
        }

        public void DeleteText()
        {
            Content = originalContent;
        }

    }

    public class Select : GameObject
    {
        /// <summary>
        /// Create thread with method core.UpdateInput()
        /// </summary>
        /// <param name="CorrectButtons"></param>
        /// <returns></returns>
        public ConsoleKey UserSelect(ConsoleKey[] CorrectButtons)
        {

            Active = true;


            while(!CorrectButtons.Contains(core.LastInput))
            {
                System.Threading.Thread.Sleep(50);
            }

            ConsoleKey lastInput = core.LastInput;

            Active = false;

            core.LastInput = new ConsoleKey();



            return lastInput;

        }

        public Select(int PosY, int PosX, Symbol[,] Content, string[] Text, bool AddToCore = false, byte color = 0xFF) : base (PosY, PosX, Content, AddToCore)
        {

            if(Program.OperationSystem == "windows")
            {
                color = (byte)(color % 16);
            }

            Active = true;

            for (int i = 0; i < Content.GetLength(0); i++)
            {
                for (int j = 0; j < Content.GetLength(1); j++)
                {
                    Content[i, j] = new Symbol(' ', color);
                }
            }

            // For each variant
            for (int i = 0; i < Text.Length; i++)
            {

                // Draw Text
                for(int j = 0; j < Text[i].Length; j++)
                {
                    Content[i * 2 + 1, (Content.GetLength(1) / 2) - (Text[i].Length / 2) + j] = new Symbol(Text[i][j], 0xFF);
                }

            }
        }
    }

    public class DialogField : GameObject
    {

        private int CursorPosX;
        private int CursorPosY;

        public DialogField(int PosY, int PosX, Symbol[,] Content, bool AddToCore = false) : base (PosY, PosX, Content, AddToCore)
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

            core.LastInput = ConsoleKey.A; // Clear last input (set any input)

            foreach(char chr in Text)
            {
                if (CursorPosX >= Content.GetLength(1) - 2)
                {
                    CursorPosY += 2;
                    CursorPosX = 2;
                }
                    
                if (CursorPosY >= Content.GetLength(0) - 3)
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

                if(core.clickButtons.Contains(core.LastInput))
                {
                    delay = 0;
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

        public int PosX;
        public int PosY;

        public bool Active = true;

        public Symbol[,] Content;

        public List<string> Attributes;

        public static Core core;

        public List<GameObjectDelegate> OnClick = new List<GameObjectDelegate>();
        public List<GameObjectDelegate> Update = new List<GameObjectDelegate>();

        public VoidDelegate Animation;
        public VoidDelegate Hover; // While cursor hover object call this method

        public List<VoidDelegate> OnMouseOver = new List<VoidDelegate>(); // Calls when the mouse join into object
        public List<VoidDelegate> OnMouseOut = new List<VoidDelegate>(); // Calls when the mouse leaves object

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

        public void LoadTexture(string path)
        {
            string[] line = File.ReadAllLines(path);

            // If file empty create invisible pixel (more safe than empty array)
            if (line.Length < 1)
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
                    catch (Exception e)
                    {
                        Content[i, j] = new Symbol(' ', 0x52);
                        continue;
                    }
                }

                i++;

            }
        }

        public GameObject GetClone()
        {
            return (GameObject)this.MemberwiseClone();
        }

        public GameObject(int PosY, int PosX)
        {
            Active = false;

            Content = new Symbol[,] { { new Symbol(' ', 0x52) } };
        }

        public GameObject(int PosY, int PosX, string path, bool AddToCore = false)
        {
            try
            {
                this.PosX = PosX;
                this.PosY = PosY;

                LoadTexture(path);
                

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

        public GameObject(int PosY, int PosX, Symbol[,] Content, bool AddToCore = false)
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
