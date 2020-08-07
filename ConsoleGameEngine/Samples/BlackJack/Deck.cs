using System;
using System.Collections.Generic;

namespace ConsoleGameEngine.Samples.BlackJack
{
    public class Deck
    {

        private List<Card> Cards = new List<Card>();

        private static Random random = new Random();

        public Deck(List<Card> Cards)
        {
            this.Cards = Cards;
        }


        // Get card from top of deck
        public Card GetCard()
        {
            try
            {
                Card card = Cards[0];
                Cards.RemoveAt(0);
                return card;
            }
            catch(IndexOutOfRangeException)
            {
                Console.WriteLine("Deck empty!");
                return new Card(new GameObject(1, 1, new Symbol[,] { { new Symbol(' ', 0x55) } }), 0, "Empty card");
            }
        }


        // Get card by ID
        public Card GetCard(int id)
        {
            try
            {
                Card card = Cards[id];
                Cards.RemoveAt(id);
                return card;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("You get wrong id, the cards club 2 blocks down");
                return new Card(new GameObject(1, 1, new Symbol[,] { { new Symbol(' ', 0x52) } }), 0, "Empty card");
            }
        }

        public void Shuffle()
        {


            List<Card> cards = new List<Card>();

            int cardCount = Cards.Count;

            for(int i = 0; i < cardCount; i++)
            {
                int num = (random.Next(0, Cards.Count) % Cards.Count);
                cards.Add(Cards[num]);
                Cards.RemoveAt(num);
            }

            Cards = cards;

        }

    }

    public class Card
    {
        // Self object
        public GameObject obj;

        // Meta info
        public int Value;
        public string Name;
        public bool Hidden = false;

        private Symbol[,] HiddenTexture; // Save texture of hidden card (to open them later)
        private string HiddenName;

        public TextField textField;

        public Card(GameObject obj, int Value, string Name)
        {
            this.obj = obj;
            this.Value = Value;
            this.Name = Name;
        }

        public Card(GameObject obj, int Value, string Name, bool Hidden)
        {
            this.obj = obj;
            this.Value = Value;
            this.Name = Name;
            this.Hidden = Hidden;
        }

        public void HideCard()
        {
            HiddenTexture = obj.Content;
            HiddenName = Name;

            Name = "(Unknown)";

            Hidden = true;
            obj.LoadTexture(Program.rootPath + "/blackjack/cards/emptycard.txt");
        }

        public void ShowCard()
        {
            if(!Hidden)
            {
                return;
            }

            Hidden = false;

            obj.Content = HiddenTexture;
            Name = HiddenName;
        }


    }

}
