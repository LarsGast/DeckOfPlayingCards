namespace DeckOfPlayingCardsTests.Unit
{
    [TestFixture]
    public class DeckTests
    {
        [Test]
        public void GetDeck_WhenNoCardsProvided_ReturnsStandardDeck()
        {
            // Arrange & Act
            var deck = Deck.Get();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(deck.Cards, Has.Count.EqualTo(52));

                // uniqueness by (Rank,Suit)
                Assert.That(deck.Cards.Select(Key).Distinct().Count(), Is.EqualTo(52));
            });
        }

        [Test]
        public void GetDeck_WhenCustomCardsProvided_ReturnsDeckWithCustomCards()
        {
            // Arrange
            var customCards = new List<Card>
            {
                new(Rank.Ace, Suit.Hearts),
                new(Rank.King, Suit.Spades),
                new(Rank.Queen, Suit.Diamonds),
            };

            // Act
            var deck = Deck.Get(customCards);

            // Assert
            Assert.That(deck.Cards.Select(Key), Is.EquivalentTo(customCards.Select(Key)));
        }

        [Test]
        public void Draw_WhenDrawingWholeDeck_ReturnsAllCardsThenNull()
        {
            // Arrange
            var deck = Deck.Get();
            var drawn = new List<Card?>();

            // Act
            for (var i = 0; i < 52; i++)
            {
                drawn.Add(deck.Draw());
            }

            var after = deck.Draw();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(drawn.All(c => c is not null), Is.True, "Expected 52 non-null draws.");
                Assert.That(
                    drawn.Select(c => Key(c!)).Distinct().Count(),
                    Is.EqualTo(52),
                    "All drawn cards should be unique."
                );
                Assert.That(after, Is.Null, "53rd draw should return null.");
            });
        }

        [Test]
        public void Reset_AfterDrawing_AllowsDrawingSameFirstCardAgain()
        {
            // Arrange
            var deck = Deck.Get();
            var firstBeforeReset = deck.Draw()!;

            // Act
            _ = deck.Draw();
            _ = deck.Draw();
            deck.Reset();

            var firstAfterReset = deck.Draw()!;

            // Assert
            Assert.That(
                Key(firstBeforeReset),
                Is.EqualTo(Key(firstAfterReset)),
                "Reset should set the draw position back to the top (without changing card order)."
            );
        }

        [Test]
        public void Shuffle_AfterShuffle_PreservesAllCards()
        {
            // Arrange
            var deck = Deck.Get();
            var before = deck.Cards.Select(Key).OrderBy(x => x).ToList();

            // Act
            deck.Shuffle(random: new Random(12345));

            var after = deck.Cards.Select(Key).OrderBy(x => x).ToList();

            // Assert
            Assert.That(
                after,
                Is.EqualTo(before),
                "Shuffle must only permute the deck; it must not add/remove/duplicate cards."
            );
        }

        [Test]
        public void Shuffle_WhenShufflingWithDifferentSeeds_ProducesDifferentOrders()
        {
            // Arrange
            var firstDeck = Deck.Get();
            var secondDeck = Deck.Get();

            // Act
            firstDeck.Shuffle(random: new Random(1));
            secondDeck.Shuffle(random: new Random(2));

            // Assert
            Assert.That(
                firstDeck.Cards.Select(Key).ToList(),
                Is.Not.EqualTo(secondDeck.Cards.Select(Key).ToList()),
                "Shuffling with different seeds should produce different card orders."
            );
        }

        [Test]
        public void Shuffle_WhenIncludingDrawnCards_ResetsDrawPosition()
        {
            // Arrange
            var deck = Deck.Get();

            // Draw some cards to move index forward...
            _ = deck.Draw();
            _ = deck.Draw();
            _ = deck.Draw();

            // Act
            deck.Shuffle(true, new Random(1));

            // After shuffle+reset, the next drawn card must be the top of the *shuffled* deck (Cards[0]).
            var drawnAfterShuffle = deck.Draw();

            // Assert
            Assert.That(
                Key(drawnAfterShuffle!),
                Is.EqualTo(Key(deck.Cards[0])),
                "After Shuffle(includeDrawnCards: true), drawing should start from the top of the shuffled deck."
            );
        }

        [Test]
        public void Shuffle_WhenExcludingDrawnCards_PreservesOrderOfDrawnCards()
        {
            // Arrange
            var deck = Deck.Get();
            deck.Shuffle(random: new Random(999));

            // Draw N cards
            const int n = 10;
            var drawn = new List<Card>();
            for (var i = 0; i < n; i++)
            {
                drawn.Add(deck.Draw()!);
            }

            // Act
            deck.Shuffle(false, new Random(1));

            // Reset so we can inspect the "top" of the deck via Draw()
            deck.Reset();

            // Expect the first N drawn cards to still be the first N after reset
            for (var i = 0; i < n; i++)
            {
                var now = deck.Draw()!;
                Assert.That(
                    Key(now),
                    Is.EqualTo(Key(drawn[i])),
                    $"Card {i} in the drawn prefix should remain unchanged when includeDrawnCards=false."
                );
            }
        }

        [Test]
        public void Shuffle_WhenExcludingDrawnCards_ShufflesNonDrawnCards()
        {
            // Arrange
            var deck = Deck.Get();

            // Draw N cards
            const int n = 10;
            for (var i = 0; i < n; i++)
            {
                _ = deck.Draw();
            }

            var nonDrawnBefore = new List<Card>();
            while (deck.Draw() is Card card)
            {
                nonDrawnBefore.Add(card);
            }

            deck.Reset();
            for (var i = 0; i < n; i++)
            {
                _ = deck.Draw();
            }

            // Act
            deck.Shuffle(false, new Random(999));

            var nonDrawnAfter = new List<Card>();
            while (deck.Draw() is Card card)
            {
                nonDrawnAfter.Add(card);
            }

            Assert.Multiple(() =>
            {
                Assert.That(
                    nonDrawnAfter,
                    Has.Count.EqualTo(nonDrawnBefore.Count),
                    "Shuffling should not reset index."
                );
                Assert.That(
                    nonDrawnAfter.Select(Key).ToList(),
                    Is.Not.EqualTo(nonDrawnBefore.Select(Key).ToList()),
                    "Shuffling should produce different card order."
                );
            });
        }

        #region Helpers

        /// <summary>
        /// Create a unique key for a card based on its Rank and Suit, for easy comparison and uniqueness checks.
        /// </summary>
        /// <param name="card"></param>
        /// <returns>"2-0" for 2 of spades, "13-2" for king of diamonds, etc.</returns>
        private static string Key(Card card)
        {
            return $"{(int)card.Rank}-{(int)card.Suit}";
        }

        #endregion
    }
}

