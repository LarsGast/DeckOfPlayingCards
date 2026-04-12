using DeckOfPlayingCardsLibrary.Enums;

namespace DeckOfPlayingCardsLibrary
{
    /// <summary>
    /// Represents a playing card with a rank and suit.
    /// </summary>
    /// <param name="Rank"><see cref="CardRank"/> of the card.</param>
    /// <param name="Suit"><see cref="CardSuit"/> of the card.</param>
    public record struct Card(CardRank Rank, CardSuit Suit);
}
