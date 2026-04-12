using System.Collections.ObjectModel;
using DeckOfPlayingCardsLibrary.Enums;

namespace DeckOfPlayingCardsLibrary.Entities
{
    /// <summary>
    /// Represents a deck of playing cards.
    /// </summary>
    /// <remarks>
    /// This class allows you to create, shuffle, draw from, and reset a deck of cards.
    /// </remarks>
    public class Deck
    {
        /// <summary>
        /// Immutable list of all the cards in this deck, possibly shuffled.
        /// </summary>
        /// <remarks>
        /// This includes both drawn and non-drawn cards, in the same order as they are in the deck.
        /// </remarks>
        public ReadOnlyCollection<Card> Cards => this._cards.AsReadOnly();

        /// <summary>
        /// Mutable list of all cards in this deck, possibly shuffled.
        /// </summary>
        /// <remarks>
        /// Used internally to manage the state of the deck, including shuffling and drawing cards.
        /// Outside callers only see <see cref="Cards"/>, which is immutable.
        /// </remarks>
        private List<Card> _cards;

        /// <summary>
        /// The index of the card that is currently at the top of the deck and next to be drawn.
        /// </summary>
        /// <value>
        /// 0 if no cards have been drawn yet or if the deck has been reset.
        /// </value>
        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        /// <param name="cards">The cards which the deck should hold.</param>
        private Deck(IEnumerable<Card> cards)
        {
            this._cards = cards.ToList();
            this._index = 0;
        }

        /// <summary>
        /// Creates a new deck of cards, which can be initialized with a custom list of cards if desired.
        /// </summary>
        /// <param name="cards">An optional list of cards to initialize the deck.</param>
        /// <returns>A new deck of cards. If the "cards" parameter is not provided, returns a standard, unshuffled 52-card deck.</returns>
        public static Deck Get(IEnumerable<Card>? cards = null)
        {
            return cards != null ? new Deck(cards) : Deck.GetStandardDeck();
        }

        /// <summary>
        /// Creates and returns a standard 52-card deck.
        /// </summary>
        /// <returns>A new <see cref="Deck"/> object representing a standard, unshuffled deck of 52 playing cards.</returns>
        private static Deck GetStandardDeck()
        {
            // Every combination of suit and rank corresponds to a unique card in a standard deck.
            var suits = Enum.GetValues<CardSuit>();
            var ranks = Enum.GetValues<CardRank>();
            var cards = suits.SelectMany(suit => ranks.Select(rank => new Card(rank, suit)));

            return new Deck(cards);
        }

        /// <summary>
        /// Shuffles the cards in the deck.
        /// </summary>
        /// <remarks>
        /// Resets the deck if <paramref name="includeDrawnCards"/> is equal to <see langword="true"/>.
        /// Otherwise, the already drawn cards will be left at the front of the deck in the same order, and only the remaining cards will be shuffled.
        /// (See <see cref="Reset"/>)
        /// </remarks>
        /// <param name="includeDrawnCards">Indicates whether drawn cards should be shuffled back into the deck.</param>
        /// <param name="random">A pre-defined random variable. One will be created if not provided.</param>
        public void Shuffle(bool includeDrawnCards = true, Random? random = null)
        {
            // Initialize random if not done already.
            random ??= new Random();

            // Shuffle ALL cards back into the deck, and start drawing from the top again.
            if (includeDrawnCards)
            {
                this._cards = this._cards.OrderBy(card => random.Next()).ToList();
                this.Reset();
                return;
            }

            // Determine which cards to shuffle and which cards to leave in place.
            var drawnCards = this._cards.Where(card => this._cards.IndexOf(card) <= this._index);
            var nonDrawnCards = this._cards.Where(card => this._cards.IndexOf(card) > this._index);

            // Shuffle the non-drawn cards.
            var shuffledCards = nonDrawnCards.OrderBy(card => random.Next()).ToList();

            // Add the already drawn cards to the front of the deck in order.
            this._cards = drawnCards.Union(shuffledCards).ToList();
        }

        /// <summary>
        /// Draw the top card from the deck.
        /// </summary>
        /// <remarks>
        /// The next time this method is called, the next card in the deck will be drawn, and so on until the end of the deck is reached.
        /// Cards drawn this way are not physically removed from the deck, meaning that the deck can be reset via <see cref="Reset"/>.
        /// </remarks>
        /// <returns>The card currently on top of the deck, or null if all cards are already drawn.</returns>
        public Card? Draw()
        {
            // If all cards have already been drawn, return null.
            if (this._index >= this._cards.Count)
            {
                return null;
            }

            // Get card at current index.
            var card = this._cards[this._index];

            // Increment index so that the next card will be drawn next time this method is called.
            this._index++;

            return card;
        }

        /// <summary>
        /// Resets the deck by putting all drawn cards back onto the top of the deck, in the same order as they were drawn.
        /// </summary>
        /// <remarks>
        /// This does NOT mean un-shuffling the deck.
        /// </remarks>
        public void Reset()
        {
            this._index = 0;
        }
    }
}
