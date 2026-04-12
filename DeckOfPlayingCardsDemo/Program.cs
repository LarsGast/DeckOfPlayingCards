using DeckOfPlayingCardsLibrary;

namespace DeckOfPlayingCardsDemo
{
    class Program
    {
        private static void Main(string[] args)
        {
            // Initialize some variables.
            Deck? deck = null;
            Card? card = null;
            var shuffled = false;
            var drawnCards = new List<Card>();

            while (true)
            {
                // Clear the console so the user always sees the same screen.
                Console.Clear();

                // Show the deck, shuffled, and card status to the user.
                Program.ShowStatus(deck, shuffled, card);

                // Add a white space
                Console.WriteLine();

                // Show the options that the user has to the user.
                Program.ShowOptions();

                // Get the input of the user based on the options.
                var input = Console.ReadLine();

                // Check if we actually have an input.
                // If so, de-capitalize it.
                if (input == null)
                {
                    Program.ShowMessage("No input received");
                    continue;
                }
                else
                {
                    input = input.ToLower();
                }

                // If the user wants to exit, let them exit.
                if (input == "e")
                {
                    break;
                }

                // Handle the request based on the input of the user.
                Program.HandleRequest(ref deck, ref card, ref shuffled, ref drawnCards, input);
            }
        }

        #region Console writeline methods

        /// <summary>
        /// Shows the status of the deck and the last drawn card.
        /// </summary>
        /// <param name="deck">The deck. Can be null.</param>
        /// <param name="shuffled">Whether the deck has been shuffled.</param>
        /// <param name="lastDrawnCard">The last drawn card from the deck. Can be null.</param>
        private static void ShowStatus(Deck? deck, bool shuffled, Card? lastDrawnCard)
        {
            if (deck == null)
            {
                Console.WriteLine("No deck found");
            }
            else
            {
                Console.WriteLine("Deck found");
                if (!shuffled)
                {
                    Console.WriteLine("Deck is not shuffled");
                }
                else
                {
                    Console.WriteLine("Deck is shuffled");
                }

                if (lastDrawnCard != null)
                {
                    Console.WriteLine(
                        $"Last card drawn: {lastDrawnCard.GetDisplayString(displayTenAsT: true)}"
                    );
                }
            }
        }

        /// <summary>
        /// Shows the user the options they have to continue.
        /// </summary>
        private static void ShowOptions()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("G: Get a new (unsorted) deck");
            Console.WriteLine("S: Shuffle the deck");
            Console.WriteLine("D: Draw a card from the deck");
            Console.WriteLine("R: Reset the deck");
            Console.WriteLine("H: History. View all the drawn cards");
            Console.WriteLine("E: Exit (close the console)");
        }

        /// <summary>
        /// Shows a message to the user and halts the program until user interferance.
        /// </summary>
        /// <param name="message">The message to show to the user before continuing.</param>
        private static void ShowMessage(string message = "Error")
        {
            Console.Clear();
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue");
            _ = Console.ReadKey();
        }

        #endregion

        #region Logic methods

        /// <summary>
        /// Handles the request based on the input of the user.
        /// </summary>
        /// <param name="deck">The deck. Can be null.</param>
        /// <param name="lastDrawnCard">The last drawn card. Can be null.</param>
        /// <param name="shuffled">Whether the deck has been shuffled.</param>
        /// <param name="drawnCards">The already drawn cards.</param>
        /// <param name="input">The user input.</param>
        private static void HandleRequest(
            ref Deck? deck,
            ref Card? lastDrawnCard,
            ref bool shuffled,
            ref List<Card> drawnCards,
            string? input
        )
        {
            switch (input)
            {
                case "g":
                    Program.GetDeck(ref deck, ref lastDrawnCard, ref shuffled, ref drawnCards);
                    break;
                case "s":
                    Program.ShuffleDeck(deck, ref shuffled);
                    break;
                case "d":
                    Program.DrawCard(deck, drawnCards, ref lastDrawnCard);
                    break;
                case "r":
                    Program.ResetDeck(deck, ref lastDrawnCard, ref drawnCards);
                    break;
                case "h":
                    Program.ShowHistory(drawnCards);
                    break;
                default:
                    Program.ShowMessage("Unknown input. Try again.");
                    break;
            }
        }

        /// <summary>
        /// Gets a new deck and sets the shuffled status to false.
        /// </summary>
        /// <param name="deck">The deck. Can be null.</param>
        /// <param name="shuffled">Whether the deck has been shuffled.</param>
        /// <param name="drawnCards">The already drawn cards.</param>
        private static void GetDeck(
            ref Deck? deck,
            ref Card? lastDrawnCard,
            ref bool shuffled,
            ref List<Card> drawnCards
        )
        {
            deck = Deck.Get();
            lastDrawnCard = null;
            shuffled = false;
            drawnCards = new List<Card>();
        }

        /// <summary>
        /// Shuffles the deck and sets the shuffled status to true.
        /// </summary>
        /// <param name="deck">The deck. Can be null.</param>
        /// <param name="shuffled">Whether the deck has been shuffled.</param>
        private static void ShuffleDeck(Deck? deck, ref bool shuffled)
        {
            if (deck == null)
            {
                Program.ShowMessage("Deck not found");
                return;
            }

            deck.Shuffle(includeDrawnCards: false);
            shuffled = true;
        }

        /// <summary>
        /// Draws a card from the deck and adds it to the drawnCards list.
        /// </summary>
        /// <param name="deck">The deck. Can be null.</param>
        /// <param name="drawnCards">The already drawn cards.</param>
        /// <param name="lastDrawnCard">The last drawn card. Can be null.</param>
        private static void DrawCard(Deck? deck, List<Card> drawnCards, ref Card? lastDrawnCard)
        {
            if (deck == null)
            {
                Program.ShowMessage("Deck not found");
                return;
            }

            lastDrawnCard = deck.Draw();
            if (lastDrawnCard != null)
            {
                drawnCards.Add(lastDrawnCard);
            }
        }

        /// <summary>
        /// Resets the deck.
        /// </summary>
        /// <param name="deck">The deck. Can be null.</param>
        /// <param name="lastDrawnCard">The last drawn card. Can be null.</param>
        /// <param name="drawnCards">The already drawn cards.</param>
        private static void ResetDeck(
            Deck? deck,
            ref Card? lastDrawnCard,
            ref List<Card> drawnCards
        )
        {
            if (deck == null)
            {
                Program.ShowMessage("Deck not found");
                return;
            }

            deck.Reset();
            lastDrawnCard = null;
            drawnCards = new List<Card>();

            Program.ShowMessage("Deck will be reset");
        }

        /// <summary>
        /// Shows all the cards drawn since last reset.
        /// </summary>
        /// <param name="drawnCards">The already drawn cards.</param>
        private static void ShowHistory(List<Card> drawnCards)
        {
            if (drawnCards.Count == 0)
            {
                Program.ShowMessage("No cards drawn");
                return;
            }

            Console.Clear();
            Console.WriteLine("History:");
            foreach (var drawnCard in drawnCards)
            {
                Console.WriteLine(drawnCard.GetDisplayString(displayTenAsT: true));
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            _ = Console.ReadKey();
        }

        #endregion
    }
}
