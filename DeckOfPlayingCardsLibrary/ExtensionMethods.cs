namespace DeckOfPlayingCardsLibrary
{
    /// <summary>
    /// A static class that provides extension methods for the `Card.Rank` and `Card.Suit` enums to facilitate the display of card ranks and suits as strings or Unicode characters.
    /// These extensions are useful for creating user-friendly representations of playing cards in card games and applications.
    /// </summary>
    public static class ExtensionMethods
    {
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
