using DeckOfPlayingCardsLibrary.Enums;

namespace DeckOfPlayingCardsLibrary
{
    /// <summary>
    /// Represents a playing card with a rank and suit.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Gets or sets the rank of the card.
        /// Ranks range from "Two" to "Ten," and "Jack" to "Ace."
        /// </summary>
        public CardRank Rank { get; private set; }

        /// <summary>
        /// Gets or sets the suit of the card.
        /// Suits include "Spades," "Hearts," "Diamonds," and "Clubs."
        /// </summary>
        public CardSuit Suit { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Card class with the specified rank and suit.
        /// </summary>
        /// <param name="rank">The rank of the card.</param>
        /// <param name="suit">The suit of the card.</param>
        public Card(CardRank rank, CardSuit suit)
        {
            this.Rank = rank;
            this.Suit = suit;
        }
    }
}
