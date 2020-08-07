using System;
using System.Collections.Generic;

namespace ConsoleGameEngine.Samples.BlackJack
{

    public class Start : IStartGame
    {

        private static Deck deck;
        private static Core core;

        private static List<List<Card>> PlayersCards = new List<List<Card>>();
        private static List<int> PlayerScore = new List<int>();
        private static List<int> PlayerSoftScore = new List<int>();

        private static TextField DealerScoreField;
        private static TextField PlayerScoreField;

        


        public static List<Card> cards = new List<Card>()
        {
            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/6spades.txt"), 6, "Six of spades"),
            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/7spades.txt"), 7, "Seven of spades"),
            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/8spades.txt"), 8, "Eight of spades"),
            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/9spades.txt"), 9, "Nine of spades"),
            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/10spades.txt"), 10, "Ten of spades"),

            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/jackspades.txt"), 10, "Jack of spades"),
            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/kingspades.txt"), 10, "King of spades"),

            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/acehearts.txt"), 11, "Ace of hearts"),
            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/aceclubs.txt"), 11, "Ace of clubs"),
            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/acediamonds.txt"), 11, "Ace of diamonds"),
            new Card(new GameObject(2, 1, Program.rootPath + "blackjack/cards/acespades.txt"), 11, "Ace of spades"),
        };

        public static void StartGame()
        {

            
            

            deck = new Deck(cards);

            core = new Core();

            System.Threading.Thread inputReader = new System.Threading.Thread(core.UpdateInput);

            inputReader.Start();

            GameObject.core = core;

            core.CreateWindow(Console.WindowHeight, Console.WindowWidth);

            deck.Shuffle();
            deck.Shuffle();
            deck.Shuffle();




            PlayersCards.Add(new List<Card>());
            PlayersCards.Add(new List<Card>());

            PlayerScore.Add(0);
            PlayerScore.Add(0);

            PlayerSoftScore.Add(0);
            PlayerSoftScore.Add(0);

            // Give 2 cards to dealer
            GiveCard(0, true, true);
            GiveCard(0, false, true);

            if(PlayersCards[0][0].Value + PlayersCards[0][1].Value == 21)
            {
                PlayersCards[0][0].ShowCard();
                EndGame();
            }

            // Give 2 cards to player

            GiveCard(1, false, true);
            GiveCard(1, false, true);

            if (PlayersCards[1][0].Value + PlayersCards[1][1].Value == 21)
            {
                EndGame();
            }

            CalculateScore();

            UpdateFields();

            core.DrawContent();






            while (true)
            {

                // Player turn

                ConsoleKey lastInput = ConsoleKey.A; // Any key

                while (lastInput != ConsoleKey.D2 && PlayerSoftScore[1] < 21)
                {


                    core.DrawContent();

                    Select select = new Select((Console.WindowHeight / 2) - 2, (Program.SizeX / 2) - (70 / 2), new Symbol[5, 70], new string[] { "1. Взять карту", "2. Пас" }, true);

                    lastInput = select.UserSelect(new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.D2 });


                    if (lastInput == ConsoleKey.D1)
                    {
                        GiveCard(1, false, true);
                    }

                    CalculateScore();

                    UpdateFields();

                    if(PlayerSoftScore[1] > 21)
                    {
                        EndGame();
                    }
                    else if(PlayerSoftScore[1] == 21)
                    {
                        break;
                    }

                    System.Threading.Thread.Sleep(100);

                }

                core.DrawContent();

                System.Threading.Thread.Sleep(500);

                PlayersCards[0][0].ShowCard(); // Open hidden card

                core.DrawContent();

                System.Threading.Thread.Sleep(1000);

                while(PlayerSoftScore[0] < 17)
                {
                    core.DrawContent();

                    GiveCard(0, false, true);

                    CalculateScore();

                    UpdateFields();

                    EndGame(); // Check win

                    System.Threading.Thread.Sleep(500);
                }

            }

        }

        private static void UpdateFields()
        {
            core.Objects.Remove(PlayerScoreField);
            core.Objects.Remove(DealerScoreField);
            
            DealerScoreField = new TextField(Console.WindowHeight / 2 - 2, (1 + PlayersCards[0][0].obj.Content.GetLength(1) / 2 - (PlayerSoftScore[0].ToString().Length) / 2), new Symbol[1, 1], PlayerSoftScore[0].ToString(), new List<string>() { "autoresize" }, false);
            PlayerScoreField = new TextField(Console.WindowHeight / 2 + 2, (1 + PlayersCards[0][0].obj.Content.GetLength(1) / 2 - (PlayerSoftScore[1].ToString().Length) / 2), new Symbol[1, 1], PlayerSoftScore[1].ToString(), new List<string>() { "autoresize" }, false);

            core.CreateObject(DealerScoreField);
            core.CreateObject(PlayerScoreField);

            for(int i = 0; i < PlayersCards.Count; i++)
            {

                foreach(Card card in PlayersCards[i])
                {
                    core.Objects.Remove(card.textField);

                    string name;
                    if (card.Hidden)
                    {
                        name = "(Unknown)";
                    }
                    else
                    {
                        name = card.Name + " (" + card.Value + ")";
                    }

                    if (i == 0)
                    {
                        card.textField = new TextField((card.obj.PosY + card.obj.Content.GetLength(0) + 1), (card.obj.PosX + (card.obj.Content.GetLength(1) / 2) - (name.Length / 2)), new Symbol[1, 1], name, new List<string>() { "autoresize" }, true); // Sorry for this too
                    }
                    else
                    {
                        card.textField = new TextField((card.obj.PosY - 2), (card.obj.PosX + (card.obj.Content.GetLength(1) / 2) - (name.Length / 2)), new Symbol[1, 1], name, new List<string>() { "autoresize" }, true); // Sorry for this too
                    }

                    core.CreateObject(card.textField);

                }

            }

        }

        private static void GiveCard(int playerID, bool Hidden, bool AddToCore = false)
        {
            // Add card to player cards array
            PlayersCards[playerID].Add(deck.GetCard());

            if(Hidden)
            {
                PlayersCards[playerID][PlayersCards[playerID].Count - 1].HideCard();
            }

            // Move card
            PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.PosX = (1 + ((PlayersCards[playerID].Count - 1) * (PlayersCards[playerID][0].obj.Content.GetLength(1) + 1)));

            if(playerID == 1)
            {
                PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.PosY = Console.WindowHeight - PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.Content.GetLength(0);
            }

            // Set Hidden attribute to last card of player
            PlayersCards[playerID][PlayersCards[playerID].Count - 1].Hidden = Hidden;
            // Set text in the middle


            string name;
            if(Hidden)
            {
                name = PlayersCards[playerID][PlayersCards[playerID].Count - 1].Name;
            }
            else
            {
                name = PlayersCards[playerID][PlayersCards[playerID].Count - 1].Name + " (" + PlayersCards[playerID][PlayersCards[playerID].Count - 1].Value + ")";
            }
            
            
            

            if(AddToCore)
            {
                core.CreateObject(PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj);
            }
            if(playerID == 0)
            {
                PlayersCards[playerID][PlayersCards[playerID].Count - 1].textField = new TextField((PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.PosY + PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.Content.GetLength(0) + 1), (PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.PosX + (PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.Content.GetLength(1) / 2) - (name.Length / 2)), new Symbol[1, 1], name, new List<string>() { "autoresize" }, true); // Sorry for this too
            }
            else
            {
                PlayersCards[playerID][PlayersCards[playerID].Count - 1].textField = new TextField((PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.PosY - 2), (PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.PosX + (PlayersCards[playerID][PlayersCards[playerID].Count - 1].obj.Content.GetLength(1) / 2) - (name.Length / 2)), new Symbol[1, 1], name, new List<string>() { "autoresize" }, true); // Sorry for this too
            }
            
        }


        private static void CalculateScore()
        {


            for(int i = 0; i < PlayersCards.Count; i++)
            {
                PlayerSoftScore[i] = 0;
                PlayerScore[i] = 0;
                int AcesCount = 0;

                List<Card> Aces = new List<Card>();

                foreach (Card card in PlayersCards[i])
                {
                    if(card.Hidden)
                    {
                        continue;
                    }

                    if(card.Value == 11)
                    {
                        AcesCount++;
                        Aces.Add(card);
                    }
                    PlayerScore[i] += card.Value;
                }

                PlayerSoftScore[i] += PlayerScore[i];

                while(AcesCount > 0 && PlayerSoftScore[i] > 21)
                {
                    Aces[AcesCount - 1].Value = 1;
                    PlayerSoftScore[i] -= 10;
                    AcesCount--;
                }

            }
        }


        private static void EndGame()
        {

           
            if(PlayerSoftScore[0] == PlayerSoftScore[1] || (PlayerSoftScore[0] > 21 && PlayerSoftScore[1] > 21))
            {
                Console.WriteLine("Ничья");
            }

            if((PlayerSoftScore[0] > PlayerSoftScore[1] && PlayerSoftScore[0] <= 21) || PlayerSoftScore[1] > 21)
            {
                Console.WriteLine("Победил дилер");
            }
            else if(PlayerSoftScore[1] > PlayerSoftScore[0] && PlayerSoftScore[1] <= 21)
            {
                Console.WriteLine("Победил игрок");
            }
            else
            {
                return;
            }

            PlayersCards[0][0].ShowCard();

            
            CalculateScore();
            UpdateFields();
            core.DrawContent();

            System.Environment.Exit(0);
            
        }


    }
}
