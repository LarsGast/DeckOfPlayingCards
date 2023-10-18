﻿namespace DeckOfCardsLibrary {
	/// <summary>
	/// Represents a deck of playing cards. 
	/// This class allows you to create, shuffle, draw from, and reset a deck of 52 cards.
	/// </summary>
	public class Deck {

		/// <summary>
		/// The list of 52 cards in this deck, possibly sorted.
		/// </summary>
		private List<Card> _cards { get; set; }

		/// <summary>
		/// The index of the card that is currently at the top of the deck and next to be drawn.
		/// 0 if no cards have been drawn yet.
		/// </summary>
		private int _index { get; set; }

		/// <summary>
		/// Constructor for creating a new deck.
		/// </summary>
		/// <param name="cards">The initial set of cards to populate the deck.</param>
		private Deck(IEnumerable<Card> cards) {
			this._cards = cards.ToList();
			this._index = 0;
		}

		/// <summary>
		/// Gets an unsorted deck of 52 cards.
		/// </summary>
		/// <returns>A new unshuffled deck.</returns>
		public static Deck get() {
			var cards = new List<Card>();

			// Create a card for each suit-rank combination.
			foreach (Card.Suit suit in Enum.GetValues(typeof(Card.Suit))) {
				foreach (Card.Rank rank in Enum.GetValues(typeof(Card.Rank))) {
					cards.Add(new Card(rank, suit));
				}
			}

			return new Deck(cards);
		}

		/// <summary>
		/// Shuffles the cards in the deck.
		/// </summary>
		/// <param name="includeDrawnCards">Indicates whether drawn cards should be shuffled back into the deck.</param>
		/// <param name="random">A pre-defined random variable. A new one will be created if not provided.</param>
		public void shuffle(bool includeDrawnCards = true, Random? random = null) {

			// Initialize random if not done already.
			if (random == null) {
				random = new Random();
			}

			var cardsToNotShuffle = new List<Card>();
			var cardsToShuffle = this._cards;

			// If we don't want to include the already drawn cards, then exclude them when shuffling the deck.
			if (!includeDrawnCards) {

				cardsToNotShuffle = this._cards
					.Where(card => this._cards.IndexOf(card) <= this._index)
					.ToList();

				cardsToShuffle = this._cards
					.Where(card => this._cards.IndexOf(card) > this._index)
					.ToList();
			}
			else {
				// Reset the index so cards get drawn from the top again.
				this.reset();
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
		public Card? draw() {

			if (this._index >= this._cards.Count) {
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
		public void reset() {
			this._index = 0;
		}
	}
}