using System.Collections.ObjectModel;

namespace DeckOfPlayingCardsLibrary
{
    /// <summary>
    /// Represents a deck of playing cards.
    /// This class allows you to create, shuffle, draw from, and reset a deck of 52 cards.
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// The list of 52 cards in this deck, possibly shuffled.
        /// </summary>
        public ReadOnlyCollection<Card> Cards => this._cards.AsReadOnly();

        /// <summary>
        /// The list of 52 cards in this deck, possibly shuffled.
        /// </summary>
        private List<Card> _cards;

        /// <summary>
        /// The index of the card that is currently at the top of the deck and next to be drawn.
        /// 0 if no cards have been drawn yet or if the deck has been reset.
        /// </summary>
        private int _index;

        /// <summary>
        /// Constructor for creating a new deck.
        /// </summary>
        /// <param name="cards">The initial set of cards to populate the deck.</param>
        private Deck(IEnumerable<Card> cards)
        {
            this._cards = cards.ToList();
            this._index = 0;
        }

        /// <summary>
        /// Creates a new deck of cards, which can be initialized with a custom list of cards if desired.
        /// </summary>
        /// <param name="cards">An optional list of cards to initialize the deck. If not provided, it results in a standard 52-card deck.</param>
        /// <returns>A new deck of cards. If the "cards" parameter is not provided, it will be a standard deck of 52 cards.</returns>
        public static Deck Get(List<Card>? cards = null) {
            return cards != null ? new Deck(cards) : Deck.GetStandardDeck();
        }

        /// <summary>
        /// Creates and returns a standard 52-card deck.
        /// </summary>
        /// <returns>A new Deck object representing a standard deck of 52 playing cards.</returns>
        private static Deck GetStandardDeck()
        {
            // Initialize an empty list to hold the cards.
            var cards = new List<Card>();

            // Generate cards for each suit and rank combination.
            foreach (var suit in Enum.GetValues<Suit>())
            {
                foreach (var rank in Enum.GetValues<Rank>())
                {
                    cards.Add(new Card(rank, suit));
                }
            }

            // Create and return a new Deck with the generated cards.
            return new Deck(cards);
        }

        /// <summary>
        /// Shuffles the cards in the deck.
        /// </summary>
        /// <param name="includeDrawnCards">Indicates whether drawn cards should be shuffled back into the deck.</param>
        /// <param name="random">A pre-defined random variable. A new one will be created if not provided.</param>
        public void Shuffle(bool includeDrawnCards = true, Random? random = null)
        {
            // Initialize random if not done already.
            random ??= new Random();

            var cardsToNotShuffle = new List<Card>();
            var cardsToShuffle = this._cards;

            // If we don't want to include the already drawn cards, then exclude them when shuffling the deck.
            if (!includeDrawnCards)
            {
                cardsToNotShuffle = this
                    ._cards.Where(card => this._cards.IndexOf(card) <= this._index)
                    .ToList();

                cardsToShuffle = this
                    ._cards.Where(card => this._cards.IndexOf(card) > this._index)
                    .ToList();
            }
            else
            {
                // Reset the index so cards get drawn from the top again.
                this.Reset();
            }

            // Actually shuffle the cards.
            // Add the already drawn cards to the front of the deck (in order) if necessary.
            var shuffledCards = cardsToShuffle.OrderBy(card => random.Next()).ToList();
            this._cards = cardsToNotShuffle.Union(shuffledCards).ToList();
        }

        /// <summary>
        /// Draw a card from the deck based on the current index.
        /// The card does not actually get removed from the deck.
        /// </summary>
        /// <returns>The card corresponding to the current index, or null if the current index is out of bounds.</returns>
        public Card? Draw()
        {
            if (this._index >= this._cards.Count)
            {
                return null;
            }

            var card = this._cards[this._index];

            this._index++;

            return card;
        }

        /// <summary>
        /// Resets the deck by setting the index to 0.
        /// This adds the already drawn cards back into the deck, in the same order.
        /// This does NOT mean un-shuffling the deck.
        /// </summary>
        public void Reset()
        {
            this._index = 0;
        }
    }
}
