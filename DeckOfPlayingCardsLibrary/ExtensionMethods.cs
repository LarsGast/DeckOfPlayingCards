namespace DeckOfPlayingCardsLibrary
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
        public static string GetDisplayString(this Rank rank, bool displayTenAsT = false)
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
        public static string? GetDisplayString(this Suit suit)
        {
            return suit switch
            {
                Suit.Hearts => "\u2665", // Heart symbol
                Suit.Spades => "\u2660", // Spade symbol
                Suit.Diamonds => "\u2666", // Diamond symbol
                Suit.Clubs => "\u2663", // Club symbol
                _ => throw new ArgumentException("Undefined suit value"),
            };
        }
    }
}
