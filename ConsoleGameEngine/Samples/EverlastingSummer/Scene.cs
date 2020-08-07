using System;
namespace ConsoleGameEngine
{

    public interface IScene
    {

        public static void Story(Core core, DialogField dialogField)
        {

        }

    }

    public class Dialog
    {

    }

    public class Choose
    {

        public IScene ToLoad; // Load this cene if user select this choose

        public string Text; // Text of choose

        public Choose(IScene ToLoad, string Text)
        {
            this.Text = Text;
            this.ToLoad = ToLoad;
        }

    }

}
