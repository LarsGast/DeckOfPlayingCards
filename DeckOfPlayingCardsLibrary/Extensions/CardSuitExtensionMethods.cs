using DeckOfPlayingCardsLibrary.Enums;

namespace DeckOfPlayingCardsLibrary.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="CardSuit"/>.
    /// </summary>
    public static class CardSuitExtensionMethods
    {
        /// <summary>
        /// Gets the unicode value of the suit as a string for displaying.
        /// </summary>
        /// <param name="suit">The card suit.</param>
        /// <returns>A one character Unicode string that represents the value of the suit.</returns>
        public static string GetDisplayString(this CardSuit suit)
        {
            return suit switch
            {
                CardSuit.Hearts => "\u2665", // Heart symbol
                CardSuit.Spades => "\u2660", // Spade symbol
                CardSuit.Diamonds => "\u2666", // Diamond symbol
                CardSuit.Clubs => "\u2663", // Club symbol
                _ => throw new ArgumentOutOfRangeException(nameof(suit), suit, null),
            };
        }
    }
}
