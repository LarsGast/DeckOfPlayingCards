using static DeckOfPlayingCards.Card;

namespace DeckOfPlayingCards
{
    /// <summary>
    /// A static class that provides extension methods for the `Card.Rank` and `Card.Suit` enums to facilitate the display of card ranks and suits as strings or Unicode characters.
    /// These extensions are useful for creating user-friendly representations of playing cards in card games and applications.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extension method for `Card.Rank`.
        /// Gets the value of the rank as a string for displaying.
        /// </summary>
        /// <param name="rank">The card rank.</param>
        /// <param name="displayTenAsT">Whether the rank "Ten" should be displayed as "T". Default is false (displays "10").</param>
        /// <returns>A one or two character string that represents the value of the rank.</returns>
        public static string getDisplayString(this Rank rank, bool displayTenAsT = false)
        {
            switch (rank)
            {
                case Rank.Ten:
                    if (displayTenAsT)
                    {
                        return "T";
                    }
                    break;
                case Rank.Jack:
                    return "J";
                case Rank.Queen:
                    return "Q";
                case Rank.King:
                    return "K";
                case Rank.Ace:
                    return "A";
            }

            return ((int)rank).ToString();
        }

        /// <summary>
        /// Extension method for `Card.Suit`.
        /// Gets the unicode value of the suit as a string for displaying.
        /// </summary>
        /// <param name="suit">The card suit.</param>
        /// <returns>A one character Unicode string that represents the value of the suit.</returns>
        public static string? getDisplayString(this Suit suit)
        {
            switch (suit)
            {
                case Suit.Hearts:
                    return "\u2665"; // Heart symbol
                case Suit.Spades:
                    return "\u2660"; // Spade symbol
                case Suit.Diamonds:
                    return "\u2666"; // Diamond symbol
                case Suit.Clubs:
                    return "\u2663"; // Club symbol
                default:
                    throw new ArgumentException("Undefined suit value");
            }
        }
    }
}
