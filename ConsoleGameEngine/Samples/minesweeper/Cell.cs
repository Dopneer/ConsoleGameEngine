using System;
namespace ConsoleGameEngine.Samples.minesweeper
{
    public class Cell
    {

        public bool IsBomb;

        public int BombCount;

        public char Texture;

        public bool IsOpened;

        public bool IsMarked;
        

        public void SetBomb()
        {
            IsBomb = true;
        }
        

        public Cell(int BombCount)
        {
            IsBomb = false;
            this.BombCount = BombCount;

            IsOpened = false;

            Texture = '.';
        }

        public Cell(bool IsBomb)
        {
            this.IsBomb = IsBomb;

            IsOpened = false;

            Texture = '.';
        }

        public void OpenCell()
        {
            IsOpened = true;
            if(IsBomb)
            {
                OpenField();
                Start.DrawField();
                Console.WriteLine("You lose!k");
                System.Environment.Exit(0);
            }
            else
            {
                Texture = BombCount.ToString()[0];
            }
        }

        

        public void MarkMine()
        {
            // If marked
            if(IsMarked)
            {
                // Turn off mark
                IsMarked = false;

                Texture = '.';

                //If bomb
                if(IsBomb)
                {
                    Start.ActualMineCount++;
                }
                Start.MineCount++;
            }
            else
            {
                // Turn on mark
                IsMarked = true;

                Texture = '&';

                if(IsBomb)
                {
                    Start.ActualMineCount--;
                }
                Start.MineCount--;
            }
        }

        public void OpenField()
        {
            for(int i = 0; i < Start.SizeX; i++)
            {
                for(int j = 0; j < Start.SizeY; j++)
                {
                    if(Start.GameField[i, j].IsBomb)
                    {
                        Start.GameField[i, j].Texture = '*';
                    }
                    else
                    {
                        Start.GameField[i, j].Texture = Start.GameField[i, j].BombCount.ToString()[0];
                    }
                    Start.GameField[i, j].IsOpened = true;
                }
            }
        }

    }
}
