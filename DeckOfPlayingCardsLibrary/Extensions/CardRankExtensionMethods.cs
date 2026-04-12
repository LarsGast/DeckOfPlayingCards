using DeckOfPlayingCardsLibrary.Enums;

namespace DeckOfPlayingCardsLibrary.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="CardRank"/>.
    /// </summary>
    public static class CardRankExtensionMethods
    {
        /// <summary>
        /// Gets the value of the rank as a string for displaying.
        /// </summary>
        /// <param name="rank">The card rank.</param>
        /// <param name="displayTenAsT">Whether the rank "Ten" should be displayed as "T". Default is false (displays "10").</param>
        /// <returns>A one or two character string that represents the value of the rank.</returns>
        public static string GetDisplayString(this CardRank rank, bool displayTenAsT = false)
        {
            return rank switch
            {
                CardRank.Two => "2",
                CardRank.Three => "3",
                CardRank.Four => "4",
                CardRank.Five => "5",
                CardRank.Six => "6",
                CardRank.Seven => "7",
                CardRank.Eight => "8",
                CardRank.Nine => "9",
                CardRank.Ten when displayTenAsT => "T",
                CardRank.Ten => "10",
                CardRank.Jack => "J",
                CardRank.Queen => "Q",
                CardRank.King => "K",
                CardRank.Ace => "A",
                _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null),
            };
        }
    }
}
